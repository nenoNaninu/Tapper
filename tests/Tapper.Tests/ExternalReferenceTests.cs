using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Tapper.Test.SourceTypes.Reference;
using Xunit;
using Xunit.Abstractions;

namespace Tapper.Tests;
public class ExternalReferenceTests
{
    private readonly ITestOutputHelper _output;

    public ExternalReferenceTests(ITestOutputHelper output)
    {
        MSBuildLocator.RegisterDefaults();
        _output = output;
    }

    [Fact]
    public async Task Test_Header1()
    {
        var compilation = await CompilationSingleton.GetCompilationWithExternalReferences();
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, SerializerOption.Json, NamingStyle.None, EnumNamingStyle.None, Logger.Empty);

        var targetTypes = compilation.GetSourceTypes();
        var targetTypeLookupTable = targetTypes.ToLookup<INamedTypeSymbol, INamespaceSymbol>(static x => x.ContainingNamespace, SymbolEqualityComparer.Default);

        var type = typeof(ReferencedType);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var group = targetTypeLookupTable.Where(x => SymbolEqualityComparer.Default.Equals(x.Key, typeSymbol.ContainingNamespace)).First()!;

        var writer = new CodeWriter();

        codeGenerator.AddHeader(group, ref writer);

        var code = writer.ToString();
        var gt = @"/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */
import { InheritanceClass0 } from './Tapper.Test.SourceTypes';
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
