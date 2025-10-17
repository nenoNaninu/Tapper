namespace Tapper.Test.SourceTypes;

[TranspilationSource]
public class NestedClassParent
{
    public List<NestedClassChild>? Children { get; init; }

    [TranspilationSource]
    public class NestedClassChild
    {
        public required string Message { get; init; }
        public int Value { get; init; }
    }
}

[TranspilationSource]
public record NestedRecordParent
{
    public List<NestedRecordChild>? Children { get; init; }

    [TranspilationSource]
    public class NestedRecordChild
    {
        public required string Message { get; init; }
        public int Value { get; init; }
    }
}
