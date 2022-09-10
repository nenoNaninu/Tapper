using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Tapper.Tests.Referenced;
public class CompilationSingleton
{
    public static readonly Compilation CompilationWithExternalReferences;

    static CompilationSingleton()
    {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp10);

        // x: System.Core.dll (why?)
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(LinkedList<>).Assembly.Location),
        };

        var reference = Tests.CompilationSingleton.Compilation.ToMetadataReference();

        var referenceSyntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText("../../../../Tapper.Test.SourceTypes.Reference/ReferencedClasses.cs"),
            options);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithNullableContextOptions(NullableContextOptions.Enable);

        var referenceCompilation = CSharpCompilation.Create(
            assemblyName: null,
            syntaxTrees: new[] { referenceSyntax },
            references: references.Concat(new[] { reference }),
            options: compilationOptions);

        CompilationWithExternalReferences = referenceCompilation;
    }
}
