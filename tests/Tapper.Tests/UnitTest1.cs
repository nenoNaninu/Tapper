//using System;
//using System.IO;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Tapper.Tests.SourceTypes;
//using Tapper.TypeMappers;
//using Xunit;
//using Xunit.Abstractions;

//namespace Tapper.Tests;

//public class UnitTest1
//{
//    private readonly ITestOutputHelper _output;

//    public UnitTest1(ITestOutputHelper output)
//    {
//        _output = output;
//    }

//    [Fact]
//    public void Test1()
//    {
//        var compilation = CompilationSingleton.Compilation;
//        var codeGenerator = new TypeScriptCodeGenerator(compilation, "\r\n", 2, Logger.Empty, SerializerOption.Json);

//        var type = typeof(ClassIncludePrimitiveFieldSystemBoolean);
//        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

//        var writer = new CodeWriter();

//        codeGenerator.AddType(typeSymbol, ref writer);

//        var code = writer.ToString();
//        var gt = PrimitiveTypeTranspilationAnswer.Dict[nameof(ClassIncludePrimitiveFieldSystemBoolean)];

//        _output.WriteLine(code);
//        _output.WriteLine(gt);

//        Assert.Equal(gt, code);
//    }
//}
