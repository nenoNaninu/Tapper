using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITranspilationOptions
{
    ITypeMapperProvider TypeMapperProvider { get; }
    IReadOnlyList<INamedTypeSymbol> SourceTypes { get; }
    SerializerOption SerializerOption { get; }
    NamingStyle NamingStyle { get; }
    EnumStyle EnumStyle { get; }
    NewLineOption NewLine { get; }
    int Indent { get; }
    bool ReferencedAssembliesTranspilation { get; }
}

public enum SerializerOption
{
    Json, // default
    MessagePack
}

public enum NamingStyle
{
    None, // default
    CamelCase,
    PascalCase
}

public enum EnumStyle
{
    Value, // default
    Name,
    NameCamel,
    NamePascal,
    Union,
    UnionCamel,
    UnionPascal,
}

public enum NewLineOption
{
    Lf,
    Crlf,
    Cr,
}

public static class OptionExtensions
{
    public static string GetIndentString(this ITranspilationOptions options)
    {
        return options.Indent switch
        {
            0 => string.Empty,
            1 => " ",
            2 => "  ",
            3 => "   ",
            4 => "    ",
            _ => new string(' ', options.Indent),
        }; ;
    }

    public static string ToNewLineString(this NewLineOption option)
    {
        return option switch
        {
            NewLineOption.Crlf => "\r\n",
            NewLineOption.Lf => "\n",
            NewLineOption.Cr => "\r",
            _ => throw new InvalidOperationException("Invalid NewLineOption.")
        };
    }

    public static string Transform(this NamingStyle namingStyle, string text)
    {
        return namingStyle switch
        {
            NamingStyle.PascalCase => $"{char.ToUpper(text[0])}{text[1..]}",
            NamingStyle.CamelCase => $"{char.ToLower(text[0])}{text[1..]}",
            _ => text,
        };
    }

    public static string Transform(this EnumStyle enumStyle, string text)
    {
        return enumStyle switch
        {
            EnumStyle.NamePascal or EnumStyle.UnionPascal => $"{char.ToUpper(text[0])}{text[1..]}",
            EnumStyle.NameCamel or EnumStyle.UnionCamel => $"{char.ToLower(text[0])}{text[1..]}",
            _ => text,
        };
    }
}
