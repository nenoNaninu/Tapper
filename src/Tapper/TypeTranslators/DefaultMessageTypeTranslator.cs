using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        codeWriter.Append($"export type {MessageTypeTranslatorHelper.GetTypeDefinitionString(typeSymbol)} = {{{newLineString}");

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
            codeWriter.Append($" & {MessageTypeTranslatorHelper.GetConstructedTypeName(typeSymbol.BaseType, options)};");
        }

        codeWriter.Append(newLineString);
    }
}

file static class MessageTypeTranslatorHelper
{
    public static string GetTypeDefinitionString(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.IsGenericType)
        {
            return $"{typeSymbol.Name}<{string.Join(", ", typeSymbol.TypeParameters.Select(param => param.Name))}>";
        }

        return typeSymbol.Name;
    }

    public static string GetConstructedTypeName(INamedTypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol.IsGenericType)
        {
            var mappedGenericTypeArguments = typeSymbol.TypeArguments.Select(typeArg =>
            {
                var mapper = options.TypeMapperProvider.GetTypeMapper(typeArg);
                return mapper.MapTo(typeArg, options);
            });

            return $"{typeSymbol.Name}<{string.Join(", ", mappedGenericTypeArguments)}>";
        }

        return typeSymbol.Name;
    }

    public static (ITypeSymbol TypeSymbol, bool IsNullable) GetMemberTypeSymbol(ISymbol symbol, ITranspilationOptions options)
    {
        if (symbol is IPropertySymbol propertySymbol)
        {
            var typeSymbol = propertySymbol.Type;

            if (typeSymbol.IsValueType)
            {
                if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
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
            if (options.SourceTypes.Contains(typeSymbol.ConstructedFrom, SymbolEqualityComparer.Default))
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
                if (options.SpecialSymbols.JsonIgnoreAttributes.Any(x => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, x)))
                {
                    return (false, string.Empty);
                }

                if (options.SpecialSymbols.JsonPropertyNameAttributes.Any(x => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, x)))
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
                if (options.SpecialSymbols.MessagePackIgnoreMemberAttributes.Any(x => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, x)))
                {
                    return (false, string.Empty);
                }

                if (options.SpecialSymbols.MessagePackKeyAttributes.Any(x => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, x)))
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
