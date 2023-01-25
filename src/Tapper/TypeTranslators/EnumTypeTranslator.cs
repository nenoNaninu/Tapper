using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper.TypeTranslators;

internal sealed class EnumTypeTranslator : ITypeTranslator
{
    public void Translate(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options)
    {
        var indent = options.GetIndentString();
        var newLineString = options.NewLine.ToNewLineString();

        var members = typeSymbol.GetPublicFieldsAndProperties()
            .IgnoreStatic()
            .ToArray();

        codeWriter.Append($"/** Transpiled from {typeSymbol.ToDisplayString()} */{newLineString}");
        codeWriter.Append($"export enum {typeSymbol.Name} {{{newLineString}");

        foreach (var member in members.OfType<IFieldSymbol>())
        {
            codeWriter.Append($"{indent}{NamingStyle.PascalCase.Transform(member.Name)} = {member.ConstantValue},{newLineString}");
        }

        codeWriter.Append('}');
        codeWriter.Append(newLineString);
    }
}

internal sealed class StringEnumTypeTranslator : ITypeTranslator
{
    public void Translate(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options)
    {
        var indent = options.GetIndentString();
        var enumStyle = options.EnumStyle;
        var newLineString = options.NewLine.ToNewLineString();

        var members = typeSymbol.GetPublicFieldsAndProperties()
            .IgnoreStatic()
            .ToArray();

        codeWriter.Append($"/** Transpiled from {typeSymbol.ToDisplayString()} */{newLineString}");
        codeWriter.Append($"export enum {typeSymbol.Name} {{{newLineString}");

        foreach (var member in members.OfType<IFieldSymbol>())
        {
            codeWriter.Append($"{indent}{NamingStyle.PascalCase.Transform(member.Name)} = \"{enumStyle.Transform(member.Name)}\",{newLineString}");
        }

        codeWriter.Append('}');
        codeWriter.Append(newLineString);
    }
}

internal sealed class UnionEnumTypeTranslator : ITypeTranslator
{
    public void Translate(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options)
    {
        var enumStyle = options.EnumStyle;
        var newLineString = options.NewLine.ToNewLineString();

        var members = typeSymbol.GetPublicFieldsAndProperties()
            .IgnoreStatic()
            .ToArray();

        codeWriter.Append($"/** Transpiled from {typeSymbol.ToDisplayString()} */{newLineString}");
        codeWriter.Append($"export type {typeSymbol.Name} = ");

        var memberNames = members.OfType<IFieldSymbol>()
            .Select(x => $"\"{enumStyle.Transform(x.Name)}\"");

        codeWriter.Append(string.Join(" | ", memberNames));

        codeWriter.Append(';');
        codeWriter.Append(newLineString);
    }
}
