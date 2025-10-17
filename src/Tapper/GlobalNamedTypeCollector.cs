using Microsoft.CodeAnalysis;

namespace Tapper;

internal sealed class GlobalNamedTypeCollector : SymbolVisitor
{
    // Ignore the widely used lib namespace
    private static readonly string[] IgnoreNamespaces = new[]
    {
        "System",
        "Microsoft",
        "Dapper",
        "MySqlConnector",
        "Npgsql",
        "StackExchange.Redis",
        "MimeKit",
        "Azure",
        "Google",
        "Amazon"
    };

    private readonly HashSet<INamedTypeSymbol> _namedTypeSymbols = new(1024 * 16, SymbolEqualityComparer.Default);

    public override void VisitNamespace(INamespaceSymbol symbol)
    {
        foreach (var ignoreNamespace in IgnoreNamespaces)
        {
            if (symbol.Name.StartsWith(ignoreNamespace))
            {
                return;
            }
        }

        foreach (var member in symbol.GetMembers())
        {
            member.Accept(this);
        }
    }

    public override void VisitNamedType(INamedTypeSymbol symbol)
    {
        _namedTypeSymbols.Add(symbol);

        foreach (var member in symbol.GetTypeMembers())
        {
            member.Accept(this);
        }
    }

    public INamedTypeSymbol[] ToArray() => _namedTypeSymbols.ToArray();
}
