namespace Tapper;

public interface ITranspilationOptions
{
    ITypeMapperProvider TypeMapperProvider { get; }
    SerializerOption SerializerOption { get; }
    NamingStyle NamingStyle { get; }
    EnumStyle EnumStyle { get; }
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
    UnderlyingValue, // default
    NameString,
    NameStringCamel,
    NameStringPascal,
    Union,
    UnionCamel,
    UnionPascal,
}
