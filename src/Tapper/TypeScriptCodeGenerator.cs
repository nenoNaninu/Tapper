using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Tapper.TypeMappers;

namespace Tapper;

public class TypeScriptCodeGenerator : ICodeGenerator
{
    private readonly string _newLine;
    private readonly string _indent;
    private INamedTypeSymbol[] _sourceTypes;
    private readonly INamedTypeSymbol _nullableStructTypeSymbol;
    private readonly ITranspilationOptions _transpilationOptions;

    public ITranspilationOptions TranspilationOptions => _transpilationOptions;

    public TypeScriptCodeGenerator(
        Compilation compilation,
        string newLine,
        int indent,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumNamingStyle enumNamingStyle,
        ILogger _)
    {
        _transpilationOptions = new TranspilationOptions(new DefaultTypeMapperProvider(compilation), serializerOption, namingStyle, enumNamingStyle);

        _sourceTypes = compilation.GetSourceTypes();
        _nullableStructTypeSymbol = compilation.GetTypeByMetadataName("System.Nullable`1")!;
        _newLine = newLine;
        _indent = new string(' ', indent);
    }

    public void AddSourceTypes(IEnumerable<INamedTypeSymbol> sourceTypes)
    {
        foreach (var targetType in sourceTypes)
        {
            var sourceMapper = new SourceTypeMapper(targetType);
            TranspilationOptions.TypeMapperProvider.AddTypeMapper(sourceMapper);
        }
        _sourceTypes = _sourceTypes.Concat(sourceTypes)
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
            .ToArray();
    }

    public void AddHeader(IGrouping<INamespaceSymbol, INamedTypeSymbol> types, ref CodeWriter writer)
    {
        writer.Append($"/* THIS (.ts) FILE IS GENERATED BY Tapper */{_newLine}");
        writer.Append($"/* eslint-disable */{_newLine}");
        writer.Append($"/* tslint:disable */{_newLine}");

        var memberTypes = types
            .SelectMany(static x => x.GetPublicFieldsAndProperties().IgnoreStatic())
            .SelectMany(RoslynExtensions.GetRelevantTypesFromMemberSymbol);

        var baseTypes = types.GetBaseTypes();

        var diffrentNamespaceTypes = memberTypes
            .Concat(baseTypes)
            .OfType<INamedTypeSymbol>()
            .Where(x => !SymbolEqualityComparer.Default.Equals(x.ContainingNamespace, types.Key)
                && _sourceTypes.Contains(x, SymbolEqualityComparer.Default))
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
            .ToLookup<INamedTypeSymbol, INamespaceSymbol>(static x => x.ContainingNamespace, SymbolEqualityComparer.Default);

        foreach (var groupingType in diffrentNamespaceTypes)
        {
            writer.Append($"import {{ {string.Join(", ", groupingType.Select(x => x.Name))} }} from './{groupingType.Key.ToDisplayString()}';{_newLine}");
        }

        writer.Append(_newLine);
    }

    public void AddType(INamedTypeSymbol typeSymbol, ref CodeWriter writer)
    {
        if (typeSymbol.TypeKind == TypeKind.Enum)
        {
            AddEnum(typeSymbol, ref writer);
        }
        else
        {
            AddClassOrStruct(typeSymbol, ref writer);
        }
    }

    private void AddEnum(INamedTypeSymbol typeSymbol, ref CodeWriter writer)
    {
        var members = typeSymbol.GetPublicFieldsAndProperties().IgnoreStatic().ToArray();

        writer.Append($"/** Transpiled from {typeSymbol.ToDisplayString()} */{_newLine}");
        writer.Append($"export enum {typeSymbol.Name} {{{_newLine}");

        foreach (var member in members.OfType<IFieldSymbol>())
        {
            writer.Append($"{_indent}{Transform(member.Name, TranspilationOptions.EnumNamingStyle)} = {member.ConstantValue},{_newLine}");
        }

        writer.Append('}');
        writer.Append(_newLine);
    }

    private void AddClassOrStruct(INamedTypeSymbol typeSymbol, ref CodeWriter writer)
    {
        var members = typeSymbol.GetPublicFieldsAndProperties().IgnoreStatic().ToArray();

        writer.Append($"/** Transpiled from {typeSymbol.ToDisplayString()} */{_newLine}");
        writer.Append($"export type {typeSymbol.Name} = {{{_newLine}");

        foreach (var member in members)
        {
            var (memberTypeSymbol, isNullable) = GetMemberTypeSymbol(member);

            if (memberTypeSymbol is null)
            {
                throw new InvalidOperationException();
            }

            // Add jdoc comment
            writer.Append($"{_indent}/** Transpiled from {memberTypeSymbol.ToDisplayString()} */{_newLine}");
            writer.Append($"{_indent}{Transform(member.Name, TranspilationOptions.NamingStyle)}{(isNullable ? "?" : string.Empty)}: {TypeMapper.MapTo(memberTypeSymbol, TranspilationOptions)};{_newLine}");
        }

        writer.Append('}');

        if (typeSymbol.BaseType is not null &&
            typeSymbol.BaseType.IsType &&
            typeSymbol.BaseType.SpecialType != SpecialType.System_Object)
        {
            if (_sourceTypes.Contains(typeSymbol.BaseType, SymbolEqualityComparer.Default))
            {
                writer.Append($" & {typeSymbol.BaseType.Name};");
            }
        }

        writer.Append(_newLine);
    }

    private (ITypeSymbol? TypeSymbol, bool IsNullable) GetMemberTypeSymbol(ISymbol symbol)
    {
        if (symbol is IPropertySymbol propertySymbol)
        {
            if (propertySymbol.Type is ITypeSymbol typeSymbol)
            {
                if (typeSymbol.IsValueType)
                {
                    if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                    {
                        if (!namedTypeSymbol.IsGenericType)
                        {
                            return (typeSymbol, false);
                        }

                        if (SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, _nullableStructTypeSymbol))
                        {
                            return (namedTypeSymbol.TypeArguments[0], true);
                        }

                        return (typeSymbol, false);
                    }
                }

                var isNullable = propertySymbol.NullableAnnotation is not NullableAnnotation.NotAnnotated;
                return (typeSymbol, isNullable);
            }
        }
        else if (symbol is IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.Type is ITypeSymbol typeSymbol)
            {
                if (typeSymbol.IsValueType)
                {
                    if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                    {
                        if (!namedTypeSymbol.IsGenericType)
                        {
                            return (typeSymbol, false);
                        }

                        if (SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, _nullableStructTypeSymbol))
                        {
                            return (namedTypeSymbol.TypeArguments[0], true);
                        }

                        return (typeSymbol, false);
                    }
                }

                var isNullable = fieldSymbol.NullableAnnotation is not NullableAnnotation.NotAnnotated;
                return (typeSymbol, isNullable);
            }
        }

        return (null, false);
    }

    private static string Transform(string text, NamingStyle namingStyle)
    {
        return namingStyle switch
        {
            NamingStyle.PascalCase => $"{char.ToUpper(text[0])}{text[1..]}",
            NamingStyle.CamelCase => $"{char.ToLower(text[0])}{text[1..]}",
            _ => text,
        };
    }

    private static string Transform(string text, EnumNamingStyle enumNamingStyle)
    {
        return enumNamingStyle switch
        {
            EnumNamingStyle.PascalCase => $"{char.ToUpper(text[0])}{text[1..]}",
            EnumNamingStyle.CamelCase => $"{char.ToLower(text[0])}{text[1..]}",
            _ => text,
        };
    }


}
