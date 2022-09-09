using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper;

public class ReferencedTypeCollector : SymbolVisitor
{
    private readonly INamedTypeSymbol[] _targetTypes;
    private readonly INamedTypeSymbol? _attributeSymbol;
    private readonly HashSet<INamedTypeSymbol> _referencedTypes;
    public ReferencedTypeCollector(INamedTypeSymbol[] targetTypes, INamedTypeSymbol? attributeSymbol)
    {
        _referencedTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        _targetTypes = targetTypes;
        _attributeSymbol = attributeSymbol;
    }

    public ImmutableArray<INamedTypeSymbol> GetReferencedTypes() => _referencedTypes.ToImmutableArray();

    private static IEnumerable<INamedTypeSymbol> GetReferenceTree(IEnumerable<INamedTypeSymbol> types)
    {
        var baseTypes = types.GetBaseTypesAndSelfFiltered();

        var membersMemberTypes = baseTypes
            .SelectMany(static x => x.GetPublicFieldsAndProperties()
                .IgnoreStatic()
                .SelectMany(RoslynExtensions.GetRelevantTypesFromMemberSymbol))
            .OfType<INamedTypeSymbol>()
            .Where(static x => x.SpecialType == SpecialType.None)
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        var checkTypes = baseTypes.Concat(membersMemberTypes)
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        if (checkTypes.All(x => types.Contains(x, SymbolEqualityComparer.Default)))
        {
            return checkTypes;
        }

        return checkTypes.Concat(GetReferenceTree(checkTypes))
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default);
    }

    private static bool References(IEnumerable<INamedTypeSymbol> types, INamedTypeSymbol type)
    {
        if (!types.Any())
        {
            return false;
        }

        var checkTypes = GetReferenceTree(types);

        var isReference = checkTypes
            .Contains(type, SymbolEqualityComparer.Default);

        if (isReference)
        {
            return true;
        }

        if (type.IsGenericType)
        {
            if (checkTypes
                    .Where(static x => x.IsGenericType)
                    .Select(static x => x.ConstructedFrom)
                    .Contains(type.ConstructedFrom, SymbolEqualityComparer.Default))
            {
                return true;
            }
        }

        return false;
    }

    public override void VisitAssembly(IAssemblySymbol symbol)
    {
        if (_targetTypes.Length == 0)
        {
            return;
        }

        if (symbol.Name == "System.Runtime")
        {
            return;
        }

        symbol.GlobalNamespace.Accept(this);

    }

    public override void VisitNamespace(INamespaceSymbol symbol)
    {
        if (_targetTypes.Length == 0)
        {
            return;
        }

        if (symbol.Name == "System")
        {
            return;
        }

        foreach (var namespaceOrType in symbol.GetMembers())
        {
            namespaceOrType.Accept(this);
        }

    }

    public override void VisitNamedType(INamedTypeSymbol type)
    {
        if (_targetTypes.Length == 0)
        {
            return;
        }

        if (!type.IsAttributeAnnotated(_attributeSymbol) ||
            type.SpecialType != SpecialType.None ||
            !type.IsAccessibleOutsideOfAssembly())
        {
            return;
        }

        if (!References(_targetTypes, type) ||
            !_referencedTypes.Add(type))
        {
            return;
        }

        var nestedTypes = type.GetTypeMembers();

        if (nestedTypes.IsDefaultOrEmpty)
            return;

        foreach (var nestedType in nestedTypes)
        {
            nestedType.Accept(this);
        }
    }


}

internal static class ISymbolExtensions
{
    /// <summary>
    /// Taken from https://stackoverflow.com/questions/64623689/get-all-types-from-compilation-using-roslyn
    /// </summary>
    public static bool IsAccessibleOutsideOfAssembly(this ISymbol symbol) =>
        symbol.DeclaredAccessibility switch
        {
            Accessibility.Private => false,
            Accessibility.Internal => false,
            Accessibility.ProtectedAndInternal => false,
            Accessibility.Protected => true,
            Accessibility.ProtectedOrInternal => true,
            Accessibility.Public => true,
            _ => true,    //Here should be some reasonable default
        };
}
