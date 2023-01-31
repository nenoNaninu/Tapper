using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Tapper;

public class TranspilationOptions : ITranspilationOptions
{
    public ITypeMapperProvider TypeMapperProvider { get; }

    public SpecialSymbols SpecialSymbols { get; }

    public IReadOnlyList<INamedTypeSymbol> SourceTypes { get; }

    public SerializerOption SerializerOption { get; }

    public NamingStyle NamingStyle { get; }

    public EnumStyle EnumStyle { get; }

    public NewLineOption NewLine { get; }

    public int Indent { get; }

    public bool ReferencedAssembliesTranspilation { get; }

    public bool EnableAttributeReference { get; }

    public TranspilationOptions(
        Compilation compilation,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        NewLineOption newLineOption,
        int indent,
        bool referencedAssembliesTranspilation,
        bool enableAttributeReference)
        : this(
              compilation,
              new DefaultTypeMapperProvider(compilation, referencedAssembliesTranspilation),
              serializerOption,
              namingStyle,
              enumStyle,
              newLineOption,
              indent,
              referencedAssembliesTranspilation,
              enableAttributeReference)
    {
    }

    public TranspilationOptions(
        Compilation compilation,
        ITypeMapperProvider typeMapperProvider,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        NewLineOption newLineOption,
        int indent,
        bool referencedAssembliesTranspilation,
        bool enableAttributeReference)
    {
        TypeMapperProvider = typeMapperProvider;
        SpecialSymbols = new SpecialSymbols(compilation);
        SourceTypes = compilation.GetSourceTypes(referencedAssembliesTranspilation);
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
        EnumStyle = enumStyle;
        NewLine = newLineOption;
        Indent = indent;
        ReferencedAssembliesTranspilation = referencedAssembliesTranspilation;
        EnableAttributeReference = enableAttributeReference;
    }
}
