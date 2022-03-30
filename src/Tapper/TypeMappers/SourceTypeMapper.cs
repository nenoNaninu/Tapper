using System;
using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

internal class SourceTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public SourceTypeMapper(INamedTypeSymbol sourceTypes)
    {
        Assign = sourceTypes;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (SymbolEqualityComparer.Default.Equals(typeSymbol, Assign))
        {
            return typeSymbol.Name;
        }

        throw new InvalidOperationException($"SourceTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}
