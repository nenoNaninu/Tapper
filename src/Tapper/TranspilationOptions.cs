namespace Tapper;

public class TranspilationOptions : ITranspilationOptions
{
    public ITypeMapperProvider TypeMapperProvider { get; }

    public SerializerOption SerializerOption { get; }

    public NamingStyle NamingStyle { get; }

    public EnumStyle EnumStyle { get; }

    public NewLineOption NewLine { get; }

    public int Indent { get; }

    public bool ReferencedAssembliesTranspilation { get; }

    public TranspilationOptions(
        ITypeMapperProvider typeMapperProvider,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        NewLineOption newLineOption,
        int indent,
        bool referencedAssembliesTranspilation)
    {
        TypeMapperProvider = typeMapperProvider;
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
        EnumStyle = enumStyle;
        NewLine = newLineOption;
        Indent = indent;
        ReferencedAssembliesTranspilation = referencedAssembliesTranspilation;
    }
}
