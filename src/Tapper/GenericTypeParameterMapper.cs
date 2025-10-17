using Microsoft.CodeAnalysis;

namespace Tapper;

internal class GenericTypeParameterMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; } = default!;

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is not ITypeParameterSymbol typeParameterSymbol)
            throw new InvalidOperationException($"{nameof(GenericTypeParameterMapper)} does not support {typeSymbol.ToDisplayString()}.");

        return typeParameterSymbol.Name;
    }
}
