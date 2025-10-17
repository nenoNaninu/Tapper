namespace Tapper.Test.SourceTypes;

[TranspilationSource]
public enum Enum1
{
    None,
    Value1,
    Value2,
}


[TranspilationSource]
public enum Enum2
{
    None = 1 << 2,
    Value1 = 1 << 3,
    Value2 = 1 << 4,
}
