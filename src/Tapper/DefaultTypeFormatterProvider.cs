using Microsoft.CodeAnalysis;

namespace Tapper;

internal class DefaultTypeFormatterProvider : ITypeFormatterProvider
{
    private readonly ITypeFormatter _messageTypeFormatter;
    private readonly ITypeFormatter _enumTypeFormatter;

    public DefaultTypeFormatterProvider(ITypeFormatter messageTypeFormatter, ITypeFormatter enumTypeFormatter)
    {
        _messageTypeFormatter = messageTypeFormatter;
        _enumTypeFormatter = enumTypeFormatter;
    }

    public ITypeFormatter GetTypeFormatter(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.TypeKind == TypeKind.Enum)
        {
            return _enumTypeFormatter;
        }

        return _messageTypeFormatter;
    }
}
