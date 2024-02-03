using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

        var inheritanceSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/InheritanceClasses.cs"),
            options);

        var attributeAnnotatedSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/AttributeAnnotatedClasses.cs"),
            options);

        var messagePackAttributesSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/MessagePackAttributes.cs"),
            options);

        var partialClassSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/PartialClass.cs"),
            options);

        var nestedTypeSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/NestedType.cs"),
            options);

        var genericClassSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes/GenericClasses.cs"),
            options);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithNullableContextOptions(NullableContextOptions.Enable);

        // x: System.Core.dll (why?)
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(LinkedList<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(JsonPropertyNameAttribute).Assembly.Location),
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
                inheritanceSyntax,
                attributeAnnotatedSyntax,
                messagePackAttributesSyntax,
                partialClassSyntax,
                nestedTypeSyntax,
                genericClassSyntax,
            },
            references: references,
            options: compilationOptions);

        Compilation = compilation;
    }
}
