using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Tapper.Analyzer;
using Xunit;

namespace Tapper.Tests.Analyzer;

public class AnalyzerTest
{
    static async Task VerifyAsync(string testCode, string id, int startLine, int startColumn, int endLine, int endColumn)
    {
        await new CSharpAnalyzerTest<TapperAnalyzer, XUnitVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Default,
            ExpectedDiagnostics = { new DiagnosticResult(id, DiagnosticSeverity.Warning)
                .WithSpan(startLine, startColumn, endLine, endColumn) },
            TestCode = testCode,
            TestState =
            {
                AdditionalReferences = { typeof(TranspilationSourceAttribute).Assembly.Location }
            }
        }.RunAsync();
    }

    [Fact]
    public async Task Test_TAPP001()
    {
        var code = @"using Tapper;

class Type1
{
}

[TranspilationSource]
class Type2
{
    public Type1? Type1 { get; set; }
}";

        await VerifyAsync(code, "TAPP001", 10, 19, 10, 24);
    }

    [Fact]
    public async Task Test_TAPP002()
    {
        var code = @"using Tapper;

[TranspilationSource]
unsafe class NotNamedType
{
    public delegate*<int, int> FuncPointer;
}";

        await VerifyAsync(code, "TAPP002", 6, 32, 6, 43);
    }

    [Fact]
    public async Task Test_TAPP004()
    {
        var code = @"using Tapper;
using System;

[TranspilationSource]
class NotSupport
{
    public Func<int, int>? GetPointer;
}
";

        await VerifyAsync(code, "TAPP004", 7, 28, 7, 38);
    }

    [Fact]
    public async Task Test_GenericType()
    {
        var code = """
using Tapper;
using System;

public class GenericClass1<T>
{
    public string StringProperty { get; set; }
    public T GenericProperty { get; set; }
}

[TranspilationSource]
public class NestedGenericClass<T1, T2>
{
    public string StringProperty { get; set; }
    public T1 GenericProperty { get; set; }
    public GenericClass1<T1> GenericClass1Property { get; set; }
}
""";

        await VerifyAsync(code, "TAPP001", 15, 30, 15, 51);
    }
}
