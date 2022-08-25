using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tapper;

internal class ReferencedTypeCollector : SymbolVisitor
{
    private readonly INamedTypeSymbol[] _targetTypes;
    private readonly HashSet<INamedTypeSymbol> _referencedTypes;
    public ReferencedTypeCollector(INamedTypeSymbol[] targetTypes)
    {
        _referencedTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        _targetTypes = targetTypes;
    }

    public ImmutableArray<INamedTypeSymbol> GetReferencedTypes() => _referencedTypes.ToImmutableArray();

    private static bool RecursiveReferences(INamedTypeSymbol type, INamedTypeSymbol[] types)
    {
        if (types.Length == 0)
        {
            return false;
        }

        var memberTypes = types
            .SelectMany(static x => x.GetPublicFieldsAndProperties().IgnoreStatic()
                .SelectMany(RoslynExtensions.GetRelevantTypesFromMemberSymbol))
            .Where(static x => x.SpecialType == SpecialType.None && !x.IsUnmanagedType)
            .Distinct(SymbolEqualityComparer.Default)
            .OfType<INamedTypeSymbol>()
            .ToArray();

        var isMemberReference = memberTypes.Contains(type, SymbolEqualityComparer.Default);

        var baseTypes = types
            .Where(static x => x.BaseType is not null && x.BaseType.SpecialType != SpecialType.System_Object && x.BaseType.SpecialType != SpecialType.System_Enum && x.BaseType.SpecialType != SpecialType.System_ValueType)
            .Select(static x => x.BaseType)
            .Distinct(SymbolEqualityComparer.Default)
            .OfType<INamedTypeSymbol>()
            .ToArray();

        var isBaseTypeReference = baseTypes.Any(x => SymbolEqualityComparer.Default.Equals(x, type));

        return isMemberReference || isBaseTypeReference || RecursiveReferences(type, memberTypes) || RecursiveReferences(type, baseTypes);

    }

    public override void VisitAssembly(IAssemblySymbol symbol)
    {
        if (symbol.Name == "System.Runtime")
        {
            return;
        }

        symbol.GlobalNamespace.Accept(this);

    }

    public override void VisitNamespace(INamespaceSymbol symbol)
    {
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
        if (type.SpecialType != SpecialType.None ||
            type.IsUnmanagedType ||
            !type.IsAccessibleOutsideOfAssembly() ||
            !RecursiveReferences(type, _targetTypes) ||
            !_referencedTypes.Add(type))
            return;

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
