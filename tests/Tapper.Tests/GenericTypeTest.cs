using Space1;
using Tapper.Test.SourceTypes;
using Xunit;

namespace Tapper.Tests;

public class GenericTypeTest
{
    private readonly ITestOutputHelper _output;

    public GenericTypeTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test_GenericClass1()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(GenericClass1<>);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.GenericClass1<T> */
export type GenericClass1<T> = {
  /** Transpiled from string */
  StringProperty: string;
  /** Transpiled from T */
  GenericProperty: T;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_GenericClass2()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(GenericClass2<,>);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Space1.GenericClass2<T1, T2> */
export type GenericClass2<T1, T2> = {
  /** Transpiled from string */
  StringProperty: string;
  /** Transpiled from T1 */
  GenericProperty1: T1;
  /** Transpiled from T2 */
  GenericProperty2: T2;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_NestedGenericClass()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(NestedGenericClass<,>);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.NestedGenericClass<T1, T2> */
export type NestedGenericClass<T1, T2> = {
  /** Transpiled from string */
  StringProperty: string;
  /** Transpiled from T1 */
  GenericProperty: T1;
  /** Transpiled from Tapper.Test.SourceTypes.GenericClass1<T1> */
  GenericClass1Property: GenericClass1<T1>;
  /** Transpiled from Space1.GenericClass2<T1, T2> */
  GenericClass2Property: GenericClass2<T1, T2>;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_DeeplyNestedGenericClass()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(DeeplyNestedGenericClass<,,>);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.DeeplyNestedGenericClass<A, B, C> */
export type DeeplyNestedGenericClass<A, B, C> = {
  /** Transpiled from string */
  StringProperty: string;
  /** Transpiled from A */
  GenericPropertyA: A;
  /** Transpiled from B */
  GenericPropertyB: B;
  /** Transpiled from Tapper.Test.SourceTypes.GenericClass1<A> */
  GenericClass1Property: GenericClass1<A>;
  /** Transpiled from Space1.GenericClass2<B, C> */
  GenericClass2Property: GenericClass2<B, C>;
  /** Transpiled from Tapper.Test.SourceTypes.NestedGenericClass<string, B> */
  NestedGenericClassProperty: NestedGenericClass<string, B>;
}
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_InheritedGenericClass()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(InheritedGenericClass2<,>);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.InheritedGenericClass2<T1, T2> */
export type InheritedGenericClass2<T1, T2> = {
  /** Transpiled from T2 */
  GenericPropertyT2: T2;
} & GenericClass1<T1>;
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_InheritedConcreteGenericClass()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(InheritedConcreteGenericClass);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.InheritedConcreteGenericClass */
export type InheritedConcreteGenericClass = {
} & GenericClass2<boolean, number>;
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_InheritedGenericClassWithTheSameName()
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

        var codeGenerator = new TypeScriptCodeGenerator(compilation, options);

        var type = typeof(InheritedGenericClassWithTheSameName);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = @"/** Transpiled from Tapper.Test.SourceTypes.InheritedGenericClassWithTheSameName */
export type InheritedGenericClassWithTheSameName = {
} & InheritedGenericClassWithTheSameName<string>;
";

        _output.WriteLine(code);
        _output.WriteLine(gt);


        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }
}
