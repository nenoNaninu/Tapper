using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tapper.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class TapperAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor AnnotationRule = new DiagnosticDescriptor(
        id: "TAPP001",
        title: "There is a user-defined type member field or property to which TranspilationSourceAttribute is not applied",
        messageFormat: "Apply the TranspilationSourceAttribute to the {0}",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "There is a user-defined type member variable or property to which TranspilationSourceAttribute is not applied.");

    private static readonly DiagnosticDescriptor NamedTypeRule = new DiagnosticDescriptor(
        id: "TAPP002",
        title: "Some members are not a named type",
        messageFormat: "{0} is not named type",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Some members are not a named type.");

    private static readonly DiagnosticDescriptor UnsupportedTypeRule = new DiagnosticDescriptor(
        id: "TAPP004",
        title: "Unsupported type",
        messageFormat: "{0} is unsupported type",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Unsupported type.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(AnnotationRule, NamedTypeRule, UnsupportedTypeRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private static readonly Type[] SupportTypes = new[]
    {
        // Primitive
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(char),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(short),
        typeof(ushort),
        typeof(object),
        typeof(string),
        typeof(Uri),
        typeof(Guid),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(Nullable<>),
        // Collection
        typeof(Array),
        typeof(ArraySegment<>),
        typeof(List<>),
        typeof(LinkedList<>),
        typeof(Queue<>),
        typeof(Stack<>),
        typeof(HashSet<>),
        typeof(IEnumerable<>),
        typeof(IReadOnlyCollection<>),
        typeof(IReadOnlyList<>),
        typeof(ICollection<>),
        typeof(IList<>),
        typeof(ISet<>),
        // Dictionary
        typeof(Dictionary<,>),
        typeof(IDictionary<,>),
        typeof(IReadOnlyDictionary<,>)
    };

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        var attributeSymbol = context.Compilation.GetTypeByMetadataName("Tapper.TranspilationSourceAttribute");
        var jsonIgnoreAttribute = context.Compilation.GetTypeByMetadataName("System.Text.Json.Serialization.JsonIgnoreAttribute");
        var messagePackIgnoreAttribute = context.Compilation.GetTypeByMetadataName("MessagePack.IgnoreMemberAttribute");

        var supportTypeSymbols = SupportTypes
            .Select(x => context.Compilation.GetTypeByMetadataName(x.FullName)!)
            .ToArray();

        if (attributeSymbol is null)
        {
            return;
        }

        if (context.Symbol is INamedTypeSymbol namedTypeSymbol)
        {
            var contaion = namedTypeSymbol.GetAttributes()
                .Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeSymbol));

            if (!contaion)
            {
                return;
            }

            foreach (var member in namedTypeSymbol.GetPublicFieldsAndProperties().IgnoreStatic())
            {
                if (member.GetAttributes().Any(x =>
                    SymbolEqualityComparer.Default.Equals(x.AttributeClass, jsonIgnoreAttribute)
                    || SymbolEqualityComparer.Default.Equals(x.AttributeClass, messagePackIgnoreAttribute)))
                {
                    continue;
                }

                foreach (var type in member.GetRelevantTypesFromMemberSymbol())
                {
                    if (type is ITypeParameterSymbol)
                    {
                        continue;
                    }

                    if (type is not INamedTypeSymbol memberNamedTypeSymbol)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            NamedTypeRule, member.Locations[0], type.ToDisplayString()));
                        continue;
                    }

                    if (type.IsTupleType)
                    {
                        continue;
                    }

                    var sourceType = memberNamedTypeSymbol.IsGenericType
                        ? memberNamedTypeSymbol.ConstructedFrom
                        : memberNamedTypeSymbol;

                    if (supportTypeSymbols.Contains(sourceType, SymbolEqualityComparer.Default))
                    {
                        continue;
                    }

                    if (type.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeSymbol)))
                    {
                        continue;
                    }

                    if (type.ContainingNamespace.ToDisplayString().StartsWith("System"))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            UnsupportedTypeRule, member.Locations[0], sourceType.ToDisplayString()));
                        continue;
                    }

                    context.ReportDiagnostic(Diagnostic.Create(
                        AnnotationRule, member.Locations[0], sourceType.ToDisplayString()));
                }
            }
        }
    }
}
