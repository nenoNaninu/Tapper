using Tapper.Test.SourceTypes;
using Xunit;

namespace Tapper.Tests;

public class PartialClassTest
{
    private readonly ITestOutputHelper _output;

    public PartialClassTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.CamelCase,
            EnumStyle.Value,
            NewLineOption.Lf,
            2,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(PartialClass);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.PartialClass */
export type PartialClass = {
  /** Transpiled from int */
  value1: number;
  /** Transpiled from int */
  value2: number;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
