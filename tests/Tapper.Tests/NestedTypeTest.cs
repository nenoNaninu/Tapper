using System;
using Tapper.Test.SourceTypes;
using Tapper.Test.SourceTypes.Space3;
using Xunit;

namespace Tapper.Tests;

public class NestedTypeTest
{
    private readonly ITestOutputHelper _output;

    public NestedTypeTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test1()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.CamelCase,
            EnumStyle.Value,
            NewLineOption.Lf,
            4,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(NestedClassParent);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.NestedClassParent */
export type NestedClassParent = {
    /** Transpiled from System.Collections.Generic.List<Tapper.Test.SourceTypes.NestedClassParent.NestedClassChild>? */
    children?: NestedClassChild[];
}
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
            NamingStyle.CamelCase,
            EnumStyle.Value,
            NewLineOption.Lf,
            4,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(NestedClassParent.NestedClassChild);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.NestedClassParent.NestedClassChild */
export type NestedClassChild = {
    /** Transpiled from string */
    message: string;
    /** Transpiled from int */
    value: number;
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
            SerializerOption.Json,
            NamingStyle.CamelCase,
            EnumStyle.Value,
            NewLineOption.Lf,
            4,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(NestedRecordParent);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.NestedRecordParent */
export type NestedRecordParent = {
    /** Transpiled from System.Collections.Generic.List<Tapper.Test.SourceTypes.NestedRecordParent.NestedRecordChild>? */
    children?: NestedRecordChild[];
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test4()
    {
        var compilation = CompilationSingleton.Compilation;

        var options = new TranspilationOptions(
            compilation,
            SerializerOption.Json,
            NamingStyle.CamelCase,
            EnumStyle.Value,
            NewLineOption.Lf,
            4,
            false,
            true
        );

        var codeGenerator = new TypeScriptCodeGenerator(options);

        var type = typeof(NestedRecordParent.NestedRecordChild);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.NestedRecordParent.NestedRecordChild */
export type NestedRecordChild = {
    /** Transpiled from string */
    message: string;
    /** Transpiled from int */
    value: number;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
