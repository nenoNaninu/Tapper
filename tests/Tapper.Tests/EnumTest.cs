using System;
using Tapper.Test.SourceTypes;
using Xunit;
using Xunit.Abstractions;

namespace Tapper.Tests;

public class EnumTest
{
    private readonly ITestOutputHelper _output;

    public EnumTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test_Enum1()
    {
        var compilation = CompilationSingleton.Compilation;
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, false, SerializerOption.Json, NamingStyle.None, EnumStyle.UnderlyingValue, Logger.Empty);

        var type = typeof(Enum1);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.Enum1 */
export enum Enum1 {
  None = 0,
  Value1 = 1,
  Value2 = 2,
}
";
        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_Enum2()
    {
        var compilation = CompilationSingleton.Compilation;
        var codeGenerator = new TypeScriptCodeGenerator(compilation, Environment.NewLine, 2, false, SerializerOption.Json, NamingStyle.None, EnumStyle.UnderlyingValue, Logger.Empty);

        var type = typeof(Enum2);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.Enum2 */
export enum Enum2 {
  None = 4,
  Value1 = 8,
  Value2 = 16,
}
";
        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
