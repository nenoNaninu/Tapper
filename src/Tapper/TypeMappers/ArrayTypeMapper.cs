using System;
using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

/// <summary>
/// ITypeMapperProvider must treat this class specially because Assign cannot be generated.
/// </summary>
internal class ArrayTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; } = default!;

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
        {
            var elementType = arrayTypeSymbol.ElementType;
            var mapper = options.TypeMapperProvider.GetTypeMapper(elementType);
            return $"{mapper.MapTo(elementType, options)}[]";
        }

        throw new InvalidOperationException($"ArrayTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}
