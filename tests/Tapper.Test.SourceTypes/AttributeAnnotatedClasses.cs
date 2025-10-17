using System.Text.Json.Serialization;
using MessagePack;

namespace Tapper.Test.SourceTypes;

[TranspilationSource]
public class AttributeAnnotatedClass1
{
    [JsonIgnore]
    public int Value { get; }
    public required string Name { get; init; }
}

[TranspilationSource]
public class AttributeAnnotatedClass2
{
    [JsonPropertyName("Foo")]
    public int Value { get; }
    public required string Name { get; init; }
}

[TranspilationSource]
public class AttributeAnnotatedClass3
{
    [IgnoreMember]
    public int Value { get; }
    public required string Name { get; init; }
}

[TranspilationSource]
public class AttributeAnnotatedClass4
{
    [Key("Bar")]
    public int Value { get; }
    public required string Name { get; init; }
}
