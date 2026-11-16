using Tapper.Test.SourceTypes;
using Xunit;

namespace Tapper.Tests;

public class InheritanceTests
{
    private readonly ITestOutputHelper _output;

    public InheritanceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test0()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.None,
            EnumStyle.Value,
            NewLineOption.Lf,
            2,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(InheritanceClass0);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.InheritanceClass0 */
export type InheritanceClass0 = {
  /** Transpiled from int */
  Value0: number;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test1()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.None,
            EnumStyle.Value,
            NewLineOption.Lf,
            2,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(InheritanceClass1);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.InheritanceClass1 */
export type InheritanceClass1 = {
  /** Transpiled from string */
  InheritanceString1: string;
} & InheritanceClass0;
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test2()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.None,
            EnumStyle.Value,
            NewLineOption.Lf,
            2,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(InheritanceClass2);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.InheritanceClass2 */
export type InheritanceClass2 = {
  /** Transpiled from string? */
  InheritanceString2?: string;
} & InheritanceClass0;
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test3()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.None,
            EnumStyle.Value,
            NewLineOption.Lf,
            2,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(Space2.CustomType2);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Space2.CustomType2 */
export type CustomType2 = {
  /** Transpiled from float */
  Value2: number;
  /** Transpiled from System.DateTime */
  DateTime2: (Date | string);
} & CustomType1;
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
