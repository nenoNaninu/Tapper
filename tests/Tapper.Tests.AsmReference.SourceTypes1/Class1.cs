using System;

namespace Tapper.Tests.AsmReference.SourceTypes1;

[TranspilationSource]
public class Class1
{
    public DateTime DateTime { get; init; }
    public byte[] Data { get; init; } = Array.Empty<byte>();
}
