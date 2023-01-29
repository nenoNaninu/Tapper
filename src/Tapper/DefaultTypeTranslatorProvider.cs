using Microsoft.CodeAnalysis;

namespace Tapper;

internal class DefaultTypeTranslatorProvider : ITypeTranslatorProvider
{
    private readonly ITypeTranslator _messageTypeTranslator;
    private readonly ITypeTranslator _enumTypeTranslator;

    public DefaultTypeTranslatorProvider(ITypeTranslator messageTypeTranslator, ITypeTranslator enumTypeTranslator)
    {
        _messageTypeTranslator = messageTypeTranslator;
        _enumTypeTranslator = enumTypeTranslator;
    }

    public ITypeTranslator GetTypeTranslator(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.TypeKind == TypeKind.Enum)
        {
            return _enumTypeTranslator;
        }

        return _messageTypeTranslator;
    }
}
