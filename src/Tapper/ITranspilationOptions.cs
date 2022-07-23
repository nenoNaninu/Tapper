namespace Tapper;

public interface ITranspilationOptions
{
    ITypeMapperProvider TypeMapperProvider { get; }
    SerializerOption SerializerOption { get; }
    NamingStyle NamingStyle { get; }
    EnumNamingStyle EnumNamingStyle { get; }
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

public enum EnumNamingStyle
{
    None,
    CamelCase,
    PascalCase
}
