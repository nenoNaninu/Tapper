// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY Tapper.Test
// </auto-generated>
#nullable enable
using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tapper.Test.SourceTypes;
using Tapper.TypeMappers;
using Xunit;
using Xunit.Abstractions;

namespace Tapper.Tests;

public class DictionaryMapTest
{
    private readonly ITestOutputHelper _output;

    public DictionaryMapTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test_ClassIncludeDictionaryFieldDictionaryintstring()
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

        var type = typeof(ClassIncludeDictionaryFieldDictionaryintstring);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryFieldDictionaryintstring)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_ClassIncludeDictionaryFieldIDictionaryfloatGuid()
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

        var type = typeof(ClassIncludeDictionaryFieldIDictionaryfloatGuid);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryFieldIDictionaryfloatGuid)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_ClassIncludeDictionaryFieldIReadOnlyDictionarystringDateTime()
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

        var type = typeof(ClassIncludeDictionaryFieldIReadOnlyDictionarystringDateTime);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryFieldIReadOnlyDictionarystringDateTime)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_ClassIncludeDictionaryFieldIReadOnlyDictionaryEnum1long()
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

        var type = typeof(ClassIncludeDictionaryFieldIReadOnlyDictionaryEnum1long);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryFieldIReadOnlyDictionaryEnum1long)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }


    [Fact]
    public void Test_ClassIncludeDictionaryPropertyDictionaryintstring()
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

        var type = typeof(ClassIncludeDictionaryPropertyDictionaryintstring);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryPropertyDictionaryintstring)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_ClassIncludeDictionaryPropertyIDictionaryfloatGuid()
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

        var type = typeof(ClassIncludeDictionaryPropertyIDictionaryfloatGuid);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryPropertyIDictionaryfloatGuid)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_ClassIncludeDictionaryPropertyIReadOnlyDictionarystringDateTime()
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

        var type = typeof(ClassIncludeDictionaryPropertyIReadOnlyDictionarystringDateTime);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryPropertyIReadOnlyDictionarystringDateTime)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void Test_ClassIncludeDictionaryPropertyIReadOnlyDictionaryEnum1long()
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

        var type = typeof(ClassIncludeDictionaryPropertyIReadOnlyDictionaryEnum1long);
        var typeSymbol = compilation.GetTypeByMetadataName(type.FullName!)!;

        var writer = new CodeWriter();

        codeGenerator.AddType(typeSymbol, ref writer);

        var code = writer.ToString();
        var gt = DictionaryTypeTranspilationAnswer.Dict[nameof(ClassIncludeDictionaryPropertyIReadOnlyDictionaryEnum1long)];

        _output.WriteLine(code);
        _output.WriteLine(gt);

        Assert.Equal(gt, code, ignoreLineEndingDifferences: true);
    }

}
