using Tapper;

namespace Tapper.Test.SourceTypes
{

    [TranspilationSource]
    public class InheritanceClass0
    {
        public int Value0 { get; set; }
    }

    [TranspilationSource]
    public class InheritanceClass1 : InheritanceClass0
    {
        public string InheritanceString1 { get; set; } = "";
    }

    [TranspilationSource]
    public class InheritanceClass2 : InheritanceClass0
    {
        public string? InheritanceString2 { get; set; } = null;
    }
}

namespace Space1
{
    [TranspilationSource]
    public class CustomType1
    {
        public float Value;
        public DateTime DateTime;
    }
}

namespace Space2
{
    using Space1;

    [TranspilationSource]
    public class CustomType2 : CustomType1
    {
        public float Value2;
        public DateTime DateTime2;
    }
}
