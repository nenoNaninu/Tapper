using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper;

public static class RoslynExtensions
{
    private static INamedTypeSymbol[]? NamedTypeSymbols;

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

    private static INamedTypeSymbol[]? ReferencedTypeSymbols;

    public static IEnumerable<INamedTypeSymbol> GetReferencedTypeSymbols(this Compilation compilation, INamedTypeSymbol[] targetTypes, INamedTypeSymbol? attributeSymbol, bool skipNullCheck = false)
    {
        if (!skipNullCheck && ReferencedTypeSymbols is not null)
        {
            return ReferencedTypeSymbols;
        }

        var referencedTypesCollector = new ReferencedTypeCollector(targetTypes, attributeSymbol);
        referencedTypesCollector.Visit(compilation.GlobalNamespace);

        ReferencedTypeSymbols = referencedTypesCollector.GetReferencedTypes()
            .ToArray();

        return ReferencedTypeSymbols;
    }

    private static INamedTypeSymbol[]? TargetTypes;

    public static INamedTypeSymbol[] GetSourceTypes(this Compilation compilation)
    {
        if (TargetTypes is not null)
        {
            return TargetTypes;
        }

        var annotationSymbol = compilation.GetTypeByMetadataName("Tapper.TranspilationSourceAttribute");

        TargetTypes = compilation.GetNamedTypeSymbols()
            .Where(x => x.IsAttributeAnnotated(annotationSymbol))
            .ToArray();

        TargetTypes = TargetTypes
            .Concat(compilation.GetReferencedTypeSymbols(TargetTypes, annotationSymbol))
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
            .ToArray();

        return TargetTypes;
    }

    public static IEnumerable<ISymbol> GetPublicFieldsAndProperties(this INamedTypeSymbol source)
    {
        return source.GetMembers()
            .Where(static x =>
            {
                if (x.DeclaredAccessibility is not Accessibility.Public)
                {
                    return false;
                }

                if (x is IFieldSymbol fieldSymbol)
                {
                    return fieldSymbol.AssociatedSymbol is null;
                }

                return x is IPropertySymbol;
            });
    }

    public static IEnumerable<ISymbol> IgnoreStatic(this IEnumerable<ISymbol> source)
    {
        return source
            .Where(static x =>
            {
                if (x.ContainingType.TypeKind is TypeKind.Enum)
                {
                    return true;
                }

                if (x is IFieldSymbol fieldSymbol)
                {
                    if (fieldSymbol.AssociatedSymbol is not null)
                    {
                        return false;
                    }

                    return !fieldSymbol.IsStatic;
                }

                if (x is IPropertySymbol propertySymbol)
                {
                    return !propertySymbol.IsStatic;
                }

                return false;
            });
    }

    public static IEnumerable<ITypeSymbol> GetRelevantTypesFromMemberSymbol(this ISymbol memberSymbol)
    {
        if (memberSymbol is IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.AssociatedSymbol is not null)
            {
                yield break;
            }

            foreach (var it in GetRelevantTypes(fieldSymbol.Type))
            {
                yield return it;
            }
        }
        else if (memberSymbol is IPropertySymbol propertySymbol)
        {
            foreach (var it in GetRelevantTypes(propertySymbol.Type))
            {
                yield return it;
            }
        }
    }

    public static IEnumerable<ITypeSymbol> GetRelevantTypes(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType)
            {
                yield return namedTypeSymbol;

                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    foreach (var it in GetRelevantTypes(typeArgument))
                    {
                        yield return it;
                    }
                }
            }
            else
            {
                yield return namedTypeSymbol;
            }
        }
        else if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
        {
            var elementType = arrayTypeSymbol.ElementType;

            foreach (var it in GetRelevantTypes(elementType))
            {
                yield return it;
            }
        }
        else
        {
            yield return typeSymbol;
        }
    }
    public static bool IsAttributeAnnotated(this INamedTypeSymbol source, INamedTypeSymbol? attributeSymbol)
    {
        return source.GetAttributes()
            .Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeSymbol));
    }
    public static IEnumerable<INamedTypeSymbol> GetBaseTypes(this IEnumerable<ITypeSymbol> types)
    {
        return types.Where(static x => x.BaseType is not null &&
            x.BaseType.SpecialType != SpecialType.System_Object &&
            x.BaseType.SpecialType != SpecialType.System_Enum &&
            x.BaseType.SpecialType != SpecialType.System_ValueType)
            .Select(static x => x.BaseType!)
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default);
    }
}
