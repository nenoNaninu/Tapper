using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITypeFormatterProvider
{
    ITypeFormatter GetTypeFormatter(INamedTypeSymbol typeSymbol);
}
