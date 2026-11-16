using Tapper.Test.SourceTypes;
using Xunit;

namespace Tapper.Tests;

public class AttributeAnnotatedClassesTest
{
    private readonly ITestOutputHelper _output;

    public AttributeAnnotatedClassesTest(ITestOutputHelper output)
    {
        _output = output;
    }

    // Issue:
    //     options.SpecialSymbo.JsonPropertyNameAttribute / JsonIgnoreAttribute is always null.
    //     In CompilationSingleton.cs, add MetadataReference.CreateFromFile(typeof(JsonPropertyNameAttribute).Assembly.Location)
    //     but not effect.
    //    [Fact]
    //    public void Test0()
    //    {
    //        var compilation = CompilationSingleton.Compilation;

    //        var options = new TranspilationOptions(
    //            compilation,
    //            SerializerOption.Json,
    //            NamingStyle.None,
    //            EnumStyle.Value,
    //            NewLineOption.Lf,
    //            4,
    //            false,
    //            true
    //        );

    //        var codeGenerator = new TypeScriptCodeGenerator(options);

    //        var type = typeof(AttributeAnnotatedClass1);
    //        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

    //        var writer = new CodeWriter();

    //        codeGenerator.AddType(typeSymbol, ref writer);

    //        var code = writer.ToString();
    //        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.AttributeAnnotatedClass1 */
    //export type AttributeAnnotatedClass1 = {
    //    /** Transpiled from string */
    //    name: string;
    //}
    //";

    //        _output.WriteLine(code);
    //        _output.WriteLine(gt);

    //        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    //    }


    //    [Fact]
    //    public void Test1()
    //    {
    //        var compilation = CompilationSingleton.Compilation;

    //        var options = new TranspilationOptions(
    //            compilation,
    //            SerializerOption.Json,
    //            NamingStyle.None,
    //            EnumStyle.Value,
    //            NewLineOption.Lf,
    //            4,
    //            false,
    //            true
    //        );

    //        var codeGenerator = new TypeScriptCodeGenerator(options);

    //        var type = typeof(AttributeAnnotatedClass2);
    //        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

    //        var writer = new CodeWriter();

    //        codeGenerator.AddType(typeSymbol, ref writer);

    //        var code = writer.ToString();
    //        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.AttributeAnnotatedClass2 */
    //export type AttributeAnnotatedClass2 = {
    //    /** Transpiled from int */
    //    Foo: number;
    //    /** Transpiled from string */
    //    name: string;
    //}
    //";

    //        _output.WriteLine(code);
    //        _output.WriteLine(gt);

    //        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    //    }


    [Fact]
    public void Test2()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.MessagePack,
            NamingStyle.None,
            EnumStyle.Value,
            NewLineOption.Lf,
            4,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(AttributeAnnotatedClass3);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.AttributeAnnotatedClass3 */
export type AttributeAnnotatedClass3 = {
    /** Transpiled from string */
    Name: string;
}
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
            SerializerOption.MessagePack,
            NamingStyle.None,
            EnumStyle.Value,
            NewLineOption.Lf,
            4,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(AttributeAnnotatedClass4);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.AttributeAnnotatedClass4 */
export type AttributeAnnotatedClass4 = {
    /** Transpiled from int */
    Bar: number;
    /** Transpiled from string */
    Name: string;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
