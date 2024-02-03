using System;
using System.Linq;
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
        if (SymbolEqualityComparer.Default.Equals(typeSymbol.GetUnboundedType(), Assign))
        {
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                var mappedTypeParameters = namedTypeSymbol.TypeArguments.Select(param =>
                {
                    var mapper = options.TypeMapperProvider.GetTypeMapper(param);
                    return mapper.MapTo(param, options);
                });
                return $"{typeSymbol.Name}<{string.Join(", ", mappedTypeParameters)}>";
            }

            return typeSymbol.Name;
        }

        throw new InvalidOperationException($"SourceTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}
