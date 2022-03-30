namespace Tapper;

public class TranspilationOptions : ITranspilationOptions
{
    public ITypeMapperProvider TypeMapperProvider { get; }

    public SerializerOption SerializerOption { get; }

    public NamingStyle NamingStyle { get; }

    public TranspilationOptions(
        ITypeMapperProvider typeMapperProvider,
        SerializerOption serializerOption,
        NamingStyle namingStyle)
    {
        TypeMapperProvider = typeMapperProvider;
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
    }
}
