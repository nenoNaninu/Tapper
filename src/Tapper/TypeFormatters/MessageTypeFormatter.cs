using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tapper.TypeMappers;

namespace Tapper.TypeFormatters;

// The name "Message" is derived from the protobuf message.
// In other words, user defined type.
internal class DefaultMessageTypeFormatter : ITypeFormatter
{
    public void Transpile(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options)
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
            var (memberTypeSymbol, isNullable) = MessageTypeFormatterHelper.GetMemberTypeSymbol(member, options);

            // Add jsdoc comment
            codeWriter.Append($"{indent}/** Transpiled from {memberTypeSymbol.ToDisplayString()} */{newLineString}");
            codeWriter.Append($"{indent}{options.NamingStyle.Transform(member.Name)}{(isNullable ? "?" : string.Empty)}: {TypeMapper.MapTo(memberTypeSymbol, options)};{newLineString}");
        }

        codeWriter.Append('}');

        if (MessageTypeFormatterHelper.IsSourceType(typeSymbol.BaseType, options))
        {
            codeWriter.Append($" & {typeSymbol.BaseType.Name};");
        }

        codeWriter.Append(newLineString);
    }
}

file static class MessageTypeFormatterHelper
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
}
