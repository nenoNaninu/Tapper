using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITypeFormatter
{
    void Transpile(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options);
}
