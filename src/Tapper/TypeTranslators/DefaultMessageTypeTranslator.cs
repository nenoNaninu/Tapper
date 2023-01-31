using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tapper.TypeMappers;

namespace Tapper.TypeTranslators;

// The name "Message" is derived from the protobuf message.
// In other words, user defined type.
internal class DefaultMessageTypeTranslator : ITypeTranslator
{
    public void Translate(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options)
    {
        var indent = options.GetIndentString();
        var newLineString = options.NewLine.ToNewLineString();

        var members = typeSymbol.GetPublicFieldsAndProperties()
            .IgnoreStatic()
            .ToArray();

        codeWriter.Append($"/** Transpiled from {typeSymbol.ToDisplayString()} */{newLineString}");
        codeWriter.Append($"export type {typeSymbol.Name} = {{{newLineString}");

        foreach (var member in members)
        {
            var (memberTypeSymbol, isNullable) = MessageTypeTranslatorHelper.GetMemberTypeSymbol(member, options);

            var (isValid, name) = MessageTypeTranslatorHelper.GetMemberName(member, options);

            if (!isValid)
            {
                continue;
            }

            // Add jsdoc comment
            codeWriter.Append($"{indent}/** Transpiled from {memberTypeSymbol.ToDisplayString()} */{newLineString}");
            codeWriter.Append($"{indent}{name}{(isNullable ? "?" : string.Empty)}: {TypeMapper.MapTo(memberTypeSymbol, options)};{newLineString}");
        }

        codeWriter.Append('}');

        if (MessageTypeTranslatorHelper.IsSourceType(typeSymbol.BaseType, options))
        {
            codeWriter.Append($" & {typeSymbol.BaseType.Name};");
        }

        codeWriter.Append(newLineString);
    }
}

file static class MessageTypeTranslatorHelper
{
    public static (ITypeSymbol TypeSymbol, bool IsNullable) GetMemberTypeSymbol(ISymbol symbol, ITranspilationOptions options)
    {
        if (symbol is IPropertySymbol propertySymbol)
        {
            var typeSymbol = propertySymbol.Type;

            if (typeSymbol.IsValueType)
            {
                if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
                    if (!namedTypeSymbol.IsGenericType)
                    {
                        return (typeSymbol, false);
                    }

                    if (namedTypeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
                    {
                        return (namedTypeSymbol.TypeArguments[0], true);
                    }

                    return (typeSymbol, false);
                }
            }

            var isNullable = propertySymbol.NullableAnnotation is not NullableAnnotation.NotAnnotated;
            return (typeSymbol, isNullable);
        }
        else if (symbol is IFieldSymbol fieldSymbol)
        {
            var typeSymbol = fieldSymbol.Type;

            if (typeSymbol.IsValueType)
            {
                if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
                    if (!namedTypeSymbol.IsGenericType)
                    {
                        return (typeSymbol, false);
                    }

                    if (namedTypeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
                    {
                        return (namedTypeSymbol.TypeArguments[0], true);
                    }

                    return (typeSymbol, false);
                }
            }

            var isNullable = fieldSymbol.NullableAnnotation is not NullableAnnotation.NotAnnotated;
            return (typeSymbol, isNullable);
        }

        throw new UnreachableException($"{nameof(symbol)} must be IPropertySymbol or IFieldSymbol");
    }

    public static bool IsSourceType([NotNullWhen(true)] INamedTypeSymbol? typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is not null && typeSymbol.SpecialType != SpecialType.System_Object)
        {
            if (options.SourceTypes.Contains(typeSymbol, SymbolEqualityComparer.Default))
            {
                return true;
            }
        }

        return false;
    }

    public static (bool IsValid, string Name) GetMemberName(ISymbol memberSymbol, ITranspilationOptions options)
    {
        if (options.SerializerOption == SerializerOption.Json)
        {
            foreach (var attr in memberSymbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, options.SpecialSymbols.JsonIgnoreAttribute))
                {
                    return (false, string.Empty);
                }

                if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, options.SpecialSymbols.JsonPropertyNameAttribute))
                {
                    var name = attr.ConstructorArguments[0].Value!.ToString()!;
                    return (true, name);
                }
            }
        }
        else if (options.SerializerOption == SerializerOption.MessagePack)
        {
            foreach (var attr in memberSymbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, options.SpecialSymbols.MessagePackIgnoreMemberAttribute))
                {
                    return (false, string.Empty);
                }

                if (SymbolEqualityComparer.Default.Equals(attr.AttributeClass, options.SpecialSymbols.MessagePackKeyAttribute))
                {
                    if (attr.ConstructorArguments[0].Type?.SpecialType == SpecialType.System_String)
                    {
                        var name = attr.ConstructorArguments[0].Value!.ToString()!;
                        return (true, name);
                    }
                }
            }
        }

        return (true, options.NamingStyle.Transform(memberSymbol.Name));
    }
}
