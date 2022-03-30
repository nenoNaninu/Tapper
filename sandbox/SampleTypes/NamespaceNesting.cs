//#pragma warning disable IDE0051

//using System.Collections.Generic;
//using SampleTypes.Structs;
//using Tapper;

//namespace SampleTypes.Hoge;

//[TranspilationSource]
//public class NamespaceNesting
//{
//    int Value { get; }
//    DateGuidStruct DateGuidStruct { get; }
//}


//[TranspilationSource]
//public class NamespaceNesting2
//{
//    int Value { get; }
//    List<DateGuidStruct>? DateGuidStruct { get; }
//}


//[TranspilationSource]
//public class NamespaceNesting3
//{
//    int Value { get; }
//    List<List<DateGuidStruct2>>? DateGuidStruct { get; }
//}


using System;
using Tapper;
using System.Collections.Generic;

namespace Space1
{
    [TranspilationSource]
    public class CustomType1
    {
        public int Value;
        public Guid Name;
    }

    namespace Sub
    {
        [TranspilationSource]
        public enum MyEnum
        {
            Zero = 0,
            One = 1,
            Two = 1 << 1,
            Four = 1 << 2,
            Eight = 1 << 3,
        }
    }
}

namespace Space2
{
    [TranspilationSource]
    public record CustomType3(float Value, DateTime ReleaseDate);
}

namespace Space3
{
    using Space1;
    using Space1.Sub;
    using Space2;

    [TranspilationSource]
    public class NastingNamespaceType
    {
        public CustomType1? Value { get; set; }
        public MyEnum MyEnumValue { get; set; }
        public List<CustomType3> List { get; set; } = new();
    }
}
