using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ICodeGenerator
{
    void AddHeader(IGrouping<INamespaceSymbol, INamedTypeSymbol> types, ref CodeWriter writer);
    void AddType(INamedTypeSymbol typeSymbol, ref CodeWriter writer);
}
