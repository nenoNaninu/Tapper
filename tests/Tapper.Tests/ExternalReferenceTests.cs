using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Space1;
using Space2;
using Tapper.Test.SourceTypes.Reference;
using Xunit;
using Xunit.Abstractions;

namespace Tapper.Tests;
public class ExternalReferenceTests
{
    private readonly ITestOutputHelper _output;

    static ExternalReferenceTests()
    {
        MSBuildLocator.RegisterDefaults();
    }

    public ExternalReferenceTests(ITestOutputHelper output)
    {

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
import { CustomType2 } from './Space2';

";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);

        type = typeof(CustomType2);
        typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        group = targetTypeLookupTable.Where(x => SymbolEqualityComparer.Default.Equals(x.Key, typeSymbol.ContainingNamespace)).First()!;

        writer = new CodeWriter();

        codeGenerator.AddHeader(group, ref writer);

        code = writer.ToString();
        gt = @"/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */
import { CustomType1 } from './Space1';

";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);

        type = typeof(CustomType1);
        typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        group = targetTypeLookupTable.Where(x => SymbolEqualityComparer.Default.Equals(x.Key, typeSymbol.ContainingNamespace)).First()!;

        writer = new CodeWriter();

        codeGenerator.AddHeader(group, ref writer);

        code = writer.ToString();
        gt = @"/* THIS (.ts) FILE IS GENERATED BY Tapper */
/* eslint-disable */
/* tslint:disable */

";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task Test1()
    {
        var compilation = await CompilationSingleton.GetCompilationWithExternalReferences();
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, SerializerOption.Json, NamingStyle.None, EnumNamingStyle.None, Logger.Empty);

        var type = typeof(ReferencedType);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.Reference.ReferencedType */
export type ReferencedType = {
  /** Transpiled from Space2.CustomType2? */
  CustomType2?: CustomType2;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}