namespace Tapper;

public class TranspilationOptions : ITranspilationOptions
{
    public ITypeMapperProvider TypeMapperProvider { get; }

    public SerializerOption SerializerOption { get; }

    public NamingStyle NamingStyle { get; }

    public EnumNamingStyle EnumNamingStyle { get; }

    public TranspilationOptions(
        ITypeMapperProvider typeMapperProvider,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumNamingStyle enumNamingStyle)
    {
        TypeMapperProvider = typeMapperProvider;
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
        EnumNamingStyle = enumNamingStyle;
    }
}
