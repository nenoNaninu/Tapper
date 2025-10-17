using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

/// <summary>
/// ITypeMapperProvider must treat this class specially because Assign cannot be generated.
/// </summary>
internal class TupleTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; } = default!;

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && typeSymbol.IsValueType)
        {
            return $"[{string.Join(", ", namedTypeSymbol.TypeArguments.Select(x => MapToCore(x, options)))}]";
        }

        throw new InvalidOperationException($"TupleTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }

    private static string MapToCore(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol.NullableAnnotation is NullableAnnotation.Annotated)
        {
            if (typeSymbol.IsValueType)
            {
                // System.Nullable<T>
                var mapper = options.TypeMapperProvider.GetTypeMapper(typeSymbol);
                return mapper.MapTo(typeSymbol, options);
            }
            else
            {
                // Reference type
                var mapper = options.TypeMapperProvider.GetTypeMapper(typeSymbol);
                return $"({mapper.MapTo(typeSymbol, options)} | undefined)";
            }
        }
        else
        {
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeSymbol);
            return mapper.MapTo(typeSymbol, options);
        }
    }
}
