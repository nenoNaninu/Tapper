namespace Tapper;

public class TranspilationOptions : ITranspilationOptions
{
    public ITypeMapperProvider TypeMapperProvider { get; }

    public SerializerOption SerializerOption { get; }

    public NamingStyle NamingStyle { get; }

    public EnumStyle EnumStyle { get; }

    public NewLineOption NewLine { get; }

    public int Indent { get; }

    public bool IncludeReferencedAssemblies { get; }

    public TranspilationOptions(
        ITypeMapperProvider typeMapperProvider,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        NewLineOption newLineOption,
        int indent,
        bool includeReferencedAssemblies)
    {
        TypeMapperProvider = typeMapperProvider;
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
        EnumStyle = enumStyle;
        this.NewLine = newLineOption;
        Indent = indent;
        IncludeReferencedAssemblies = includeReferencedAssemblies;
    }
}
