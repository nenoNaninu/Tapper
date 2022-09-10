using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper;

public static partial class RoslynExtensions
{
    private static INamedTypeSymbol[]? NamedTypeSymbols;

    /// <summary>
    /// Get NamedTypeSymbols from target project.
    /// </summary>
    public static IEnumerable<INamedTypeSymbol> GetNamedTypeSymbols(this Compilation compilation)
    {
        if (NamedTypeSymbols is not null)
        {
            return NamedTypeSymbols;
        }

        NamedTypeSymbols = compilation
            .SyntaxTrees
            .SelectMany(syntaxTree =>
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                return syntaxTree.GetRoot()
                    .DescendantNodes()
                    .Select(x => semanticModel.GetDeclaredSymbol(x))
                    .OfType<INamedTypeSymbol>();
            }).ToArray();

        return NamedTypeSymbols;
    }

    private static INamedTypeSymbol[]? GlobalNamedTypeSymbols;

    /// <summary>
    /// Get NamedTypeSymbols from target project and reference assemblies.
    /// </summary>
    public static IEnumerable<INamedTypeSymbol> GetGlobalNamedTypeSymbols(this Compilation compilation)
    {
        if (GlobalNamedTypeSymbols is not null)
        {
            return GlobalNamedTypeSymbols;
        }

        var typeCollector = new GlobalNamedTypeCollector();
        typeCollector.Visit(compilation.GlobalNamespace);

        GlobalNamedTypeSymbols = typeCollector.ToArray();
        return GlobalNamedTypeSymbols;
    }

    private static INamedTypeSymbol[]? TargetTypes;

    public static INamedTypeSymbol[] GetSourceTypes(this Compilation compilation, bool includeRefarenceAssemblies)
    {
        if (TargetTypes is not null)
        {
            return TargetTypes;
        }

        var annotationSymbol = compilation.GetTypeByMetadataName("Tapper.TranspilationSourceAttribute");

        var namedTypes = includeRefarenceAssemblies ? compilation.GetGlobalNamedTypeSymbols() : compilation.GetNamedTypeSymbols();

        TargetTypes = namedTypes
            .Where(x =>
            {
                var attributes = x.GetAttributes();

                if (attributes.IsEmpty)
                {
                    return false;
                }

                return attributes.Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, annotationSymbol));
            })
            .ToArray();

        return TargetTypes;
    }
}