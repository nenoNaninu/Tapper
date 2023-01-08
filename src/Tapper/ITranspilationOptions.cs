using System;

namespace Tapper;

public interface ITranspilationOptions
{
    ITypeMapperProvider TypeMapperProvider { get; }
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
}
