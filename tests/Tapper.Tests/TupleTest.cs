using System;
using Tapper.Test.SourceTypes.Tuple;
using Xunit;
using Xunit.Abstractions;

namespace Tapper.Tests;

public class TupleTest
{
    private readonly ITestOutputHelper _output;

    public TupleTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test_SimpleTupleClass()
    {
        var compilation = CompilationSingleton.Compilation;
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, SerializerOption.Json, NamingStyle.None, Logger.Empty);

        var type = typeof(SimpleTupleClass);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpied from Tapper.Test.SourceTypes.Tuple.SimpleTupleClass */
export type SimpleTupleClass = {
  /** Transpied from (int, string) */
  TapluField: [number, string];
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_TupleClassIncludeNullable()
    {
        var compilation = CompilationSingleton.Compilation;
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, SerializerOption.Json, NamingStyle.None, Logger.Empty);

        var type = typeof(TupleClassIncludeNullable);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpied from Tapper.Test.SourceTypes.Tuple.TupleClassIncludeNullable */
export type TupleClassIncludeNullable = {
  /** Transpied from (int?, System.Guid, System.DateTime) */
  TapluField: [(number | undefined), string, (Date | string)];
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_TupleClassNullableField()
    {
        var compilation = CompilationSingleton.Compilation;
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, SerializerOption.Json, NamingStyle.None, Logger.Empty);

        var type = typeof(TupleClassNullableField);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpied from Tapper.Test.SourceTypes.Tuple.TupleClassNullableField */
export type TupleClassNullableField = {
  /** Transpied from (int?, System.Guid, System.DateTime) */
  TapluField?: [(number | undefined), string, (Date | string)];
}
";
        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_TupleClassIncludeCustomType()
    {
        var compilation = CompilationSingleton.Compilation;
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, SerializerOption.Json, NamingStyle.None, Logger.Empty);

        var type = typeof(TupleClassIncludeCustomType);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpied from Tapper.Test.SourceTypes.Tuple.TupleClassIncludeCustomType */
export type TupleClassIncludeCustomType = {
  /** Transpied from (int?, Tapper.Test.SourceTypes.Tuple.CustomType, System.DateTime) */
  TapluField?: [(number | undefined), CustomType, (Date | string)];
  /** Transpied from (int, Tapper.Test.SourceTypes.Tuple.CustomType?, System.DateTime?) */
  TapluPropery: [number, (CustomType | undefined), ((Date | string) | undefined)];
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
