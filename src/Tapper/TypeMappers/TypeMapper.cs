using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

public static class TypeMapper
{
    public static string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        var mapper = options.TypeMapperProvider.GetTypeMapper(typeSymbol);
        return mapper.MapTo(typeSymbol, options);
    }
}
