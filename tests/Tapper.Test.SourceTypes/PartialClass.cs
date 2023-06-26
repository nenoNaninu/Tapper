namespace Tapper.Test.SourceTypes;

[TranspilationSource]
public partial class PartialClass
{
    public int Value1 { get; set; }
}

public partial class PartialClass
{
    public int Value2 { get; set; }
}
