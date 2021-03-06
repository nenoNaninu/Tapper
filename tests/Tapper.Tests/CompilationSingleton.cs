using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tapper.TypeMappers;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Tapper.Tests;
public class CompilationSingleton
{
    public static readonly Compilation Compilation;

    static CompilationSingleton()
    {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp10);

        var attributeSyntaxTree = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../../src/Tapper.Attributes/TranspilationSourceAttribute.cs"),
            options);

        var primitiveSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/PrimitiveClasses.cs"),
            options);

        var collectionSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/CollectionClasses.cs"),
            options);

        var dictionarySyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/DictionaryClasses.cs"),
            options);

        var enumSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/Enums.cs"),
            options);

        var nestedNamespaceSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/NestedNamespace.cs"),
            options);

        var tupleSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/TupleClasses.cs"),
            options);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithNullableContextOptions(NullableContextOptions.Enable);

        // x: System.Core.dll (why?)
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(LinkedList<>).Assembly.Location),
        };

        var compilation = CSharpCompilation.Create(
            assemblyName: null,
            syntaxTrees: new[]
            {
                attributeSyntaxTree,
                primitiveSyntax,
                collectionSyntax,
                dictionarySyntax,
                enumSyntax,
                nestedNamespaceSyntax,
                tupleSyntax,
            },
            references: references,
            options: compilationOptions);

        Compilation = compilation;
    }
}
