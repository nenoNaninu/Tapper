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
    None,
    Json,
    MessagePack
}

public enum NamingStyle
{
    None,
    CamelCase,
    PascalCase
}

public enum EnumStyle
{
    UnderlyingValue,
    NameString,
    NameStringCamel,
    NameStringPascal,
    Union,
    UnionCamel,
    UnionPascal,
}
