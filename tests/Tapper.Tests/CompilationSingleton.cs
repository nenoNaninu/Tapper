using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Tapper.TypeMappers;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Tapper.Tests;
public class CompilationSingleton
{
    public static readonly Compilation Compilation;

    private static Compilation? CompilationWithExternalReferences;
    private static readonly SyntaxTree AttributeSyntaxTree;

    static CompilationSingleton()
    {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp10);

        AttributeSyntaxTree = CSharpSyntaxTree.ParseText(
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
                AttributeSyntaxTree,
                primitiveSyntax,
                collectionSyntax,
                dictionarySyntax,
                enumSyntax,
                nestedNamespaceSyntax,
                tupleSyntax,
                inheritanceSyntax,
            },
            references: references,
            options: compilationOptions);

        Compilation = compilation;
    }

    public static async Task<Compilation> GetCompilationWithExternalReferences()
    {
        if (CompilationWithExternalReferences is not null)
        {
            return CompilationWithExternalReferences;
        }

        var logger = new ConsoleLogger(LoggerVerbosity.Quiet);
        using var workspace = MSBuildWorkspace.Create();
        workspace.LoadMetadataForReferencedProjects = true;
        var projectPath = "../../../../Tapper.Test.SourceTypes.Reference/Tapper.Test.SourceTypes.Reference.csproj";

        var msBuildProject = await workspace.OpenProjectAsync(projectPath, logger, null);

        var compilation = await msBuildProject.GetCompilationAsync();

        if (compilation is null)
        {
            throw new InvalidOperationException("Failed to get compilation.");
        }

        compilation = compilation.AddSyntaxTrees(AttributeSyntaxTree);

        return CompilationWithExternalReferences = compilation;

    }
}
