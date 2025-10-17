namespace Tapper.Test.SourceTypes.Space1
{
    [TranspilationSource]
    public class CustomType
    {
        public int Value;
        public Guid Name;
    }

    namespace Sub
    {
        [TranspilationSource]
        public class CustomType2
        {
            public int Value { get; set; }
            public Uri? Name { get; set; }
        }

        [TranspilationSource]
        public class CustomType3
        {
            public int Value { get; set; }
            public Uri? Name { get; set; }
        }
    }
}

namespace Tapper.Test.SourceTypes.Space2
{
    [TranspilationSource]
    public class CustomType4
    {
        public float Value;
        public DateTime Name;
    }
}

namespace Tapper.Test.SourceTypes.Space3
{
    using Tapper.Test.SourceTypes.Space1;
    using Tapper.Test.SourceTypes.Space1.Sub;
    using Tapper.Test.SourceTypes.Space2;

    [TranspilationSource]
    public class NastingNamespaceType
    {
        public CustomType? Value { get; set; }
        public CustomType2 Name { get; set; } = default!;
        public CustomType3? Name2 { get; set; }
        public List<CustomType4> List { get; set; } = new();
    }
}
