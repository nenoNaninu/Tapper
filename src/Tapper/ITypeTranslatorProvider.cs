using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITypeTranslatorProvider
{
    ITypeTranslator GetTypeTranslator(INamedTypeSymbol typeSymbol);
}
