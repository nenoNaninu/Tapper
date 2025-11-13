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
            new SpecialSymbols(compilation),
            compilation.GetSourceTypes(referencedAssembliesTranspilation),
            serializerOption,
            namingStyle,
            enumStyle,
            newLineOption,
            indent,
            referencedAssembliesTranspilation,
            enableAttributeReference)
    {
        TypeMapperProvider = new DefaultTypeMapperProvider(compilation, SourceTypes);
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
        : this(
            new SpecialSymbols(compilation),
            compilation.GetSourceTypes(referencedAssembliesTranspilation),
            serializerOption,
            namingStyle,
            enumStyle,
            newLineOption,
            indent,
            referencedAssembliesTranspilation,
            enableAttributeReference)
    {
        TypeMapperProvider = typeMapperProvider;
    }
    
    private TranspilationOptions(
        SpecialSymbols specialSymbols,
        IReadOnlyList<INamedTypeSymbol> sourceTypes,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        NewLineOption newLineOption,
        int indent,
        bool referencedAssembliesTranspilation,
        bool enableAttributeReference)
    {
        SpecialSymbols = specialSymbols;
        SourceTypes = sourceTypes;
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
        EnumStyle = enumStyle;
        NewLine = newLineOption;
        Indent = indent;
        ReferencedAssembliesTranspilation = referencedAssembliesTranspilation;
        EnableAttributeReference = enableAttributeReference;
    }
}
