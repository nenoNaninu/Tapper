using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Tapper;

public class TranspilationOptions : ITranspilationOptions
{
    public ITypeMapperProvider TypeMapperProvider { get; }

    public IReadOnlyList<INamedTypeSymbol> SourceTypes { get; }

    public SerializerOption SerializerOption { get; }

    public NamingStyle NamingStyle { get; }

    public EnumStyle EnumStyle { get; }

    public NewLineOption NewLine { get; }

    public int Indent { get; }

    public bool ReferencedAssembliesTranspilation { get; }


    public TranspilationOptions(
        Compilation compilation,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        NewLineOption newLineOption,
        int indent,
        bool referencedAssembliesTranspilation)
    {
        TypeMapperProvider = new DefaultTypeMapperProvider(compilation, referencedAssembliesTranspilation);
        SourceTypes = compilation.GetSourceTypes(referencedAssembliesTranspilation);
        SerializerOption = serializerOption;
        NamingStyle = namingStyle;
        EnumStyle = enumStyle;
        NewLine = newLineOption;
        Indent = indent;
        ReferencedAssembliesTranspilation = referencedAssembliesTranspilation;
    }
}
