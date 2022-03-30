namespace Tapper;

public interface ITranspilationOptions
{
    ITypeMapperProvider TypeMapperProvider { get; }
    SerializerOption SerializerOption { get; }
    NamingStyle NamingStyle { get; }
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
