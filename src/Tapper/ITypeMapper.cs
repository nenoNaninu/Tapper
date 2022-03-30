using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITypeMapper
{
    public ITypeSymbol Assign { get; }
    string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options);
}
