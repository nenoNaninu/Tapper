using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ICodeGenerator
{
    ITranspilationOptions TranspilationOptions { get; }

    void AddHeader(IGrouping<INamespaceSymbol, INamedTypeSymbol> types, ref CodeWriter writer);
    void AddType(INamedTypeSymbol typeSymbol, ref CodeWriter writer);
    void AddSourceTypes(IEnumerable<INamedTypeSymbol> sourceTypes);
}
