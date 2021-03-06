using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITypeMapperProvider
{
    public ITypeMapper GetTypeMapper(ITypeSymbol type);
}
