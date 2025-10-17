using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

public class NullableStructTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public NullableStructTypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Nullable`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var mapper = options.TypeMapperProvider.GetTypeMapper(namedTypeSymbol.TypeArguments[0]);
            return $"({mapper.MapTo(namedTypeSymbol.TypeArguments[0], options)} | undefined)";
        }

        throw new InvalidOperationException($"NullableStructTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}
