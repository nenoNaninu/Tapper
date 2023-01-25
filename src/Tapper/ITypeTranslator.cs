using Microsoft.CodeAnalysis;

namespace Tapper;

public interface ITypeTranslator
{
    void Translate(ref CodeWriter codeWriter, INamedTypeSymbol typeSymbol, ITranspilationOptions options);
}
