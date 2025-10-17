using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Tapper;

public class Transpiler
{
    private readonly INamedTypeSymbol[] _targetTypes;
    private readonly ILookup<INamespaceSymbol, INamedTypeSymbol> _targetTypeLookupTable;
    private readonly string _newLine;
    private readonly ILogger _logger;
    private readonly ICodeGenerator _codeGenerator;

    public Transpiler(
        Compilation compilation,
        ITranspilationOptions options,
        ILogger logger)
    {
        _newLine = options.NewLine.ToNewLineString();
        _logger = logger;

        _codeGenerator = new TypeScriptCodeGenerator(
            compilation,
            options
        );

        _targetTypes = compilation.GetSourceTypes(options.ReferencedAssembliesTranspilation);
        _targetTypeLookupTable = _targetTypes.ToLookup<INamedTypeSymbol, INamespaceSymbol>(static x => x.ContainingNamespace, SymbolEqualityComparer.Default);
    }

    public IReadOnlyList<GeneratedSourceCode> Transpile()
    {
        var outputScripts = new List<GeneratedSourceCode>();

        foreach (var group in _targetTypeLookupTable)
        {
            var writer = new CodeWriter();

            _logger.Log(LogLevel.Information, "Add Header...");
            _codeGenerator.AddHeader(group, ref writer);

            foreach (var type in group)
            {
                try
                {
                    _logger.Log(LogLevel.Information, "Transpile {typename}...", type.ToDisplayString());

                    _codeGenerator.AddType(type, ref writer);
                    writer.Append(_newLine);
                }
                catch
                {
                    _logger.Log(LogLevel.Error, "{typename} can not be transpiled.", type.ToDisplayString());

                    throw;
                }
            }

            outputScripts.Add(new GeneratedSourceCode($"{group.Key.ToDisplayString()}.ts", writer.ToString()));
        }

        return outputScripts;
    }
}

public readonly record struct GeneratedSourceCode(string SourceName, string Content);
