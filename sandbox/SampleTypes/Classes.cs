//#pragma warning disable IDE0051

//using System;
//using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Tapper;
//using SampleTypes.Structs;

namespace SampleTypes;

//[TranspilationSource]
//public class EmptyClass
//{
//}

//[TranspilationSource]
//public class ClassIncludeMethod
//{
//    public int Hoge { get; set; }
//    public string? StringProperty { get; set; }

//    void Method()
//    {
//        Console.WriteLine("Method Method");
//    }
//}

//[TranspilationSource]
//public class ClassIncludeArrayField
//{
//    public int[]? ValuesField;
//}

//[TranspilationSource]
//public class ClassIncludeArrayField2
//{
//    public Array? ValuesField;
//}

[TranspilationSource]
public class SampleType
{
    public List<int>? List { get; }
    public int Value { get; }
    public Guid Id { get; }
    public DateTime DateTime { get; }
}


//[TranspilationSource]
//public class ClassIncludeNestedList
//{
//    public List<List<int>> ListListProperty { get; } = new();
//}

//[TranspilationSource]
//public class ClassIncludeDictionary
//{
//    public Dictionary<string, int> DictionaryProperty { get; } = new();
//}


//[TranspilationSource]
//public class ClassIncludeDictionary2
//{
//    public Dictionary<string, ClassIncludeDictionary> DictionaryProperty { get; } = new();
//}

//[TranspilationSource]
//public class ClassIncludeNotNamedType
//{
//    public Dictionary<string, (int, ClassIncludeDictionary)> DictionaryProperty { get; } = new();
//}

//[TranspilationSource]
//public unsafe class ClassIncludePointerType2
//{
//    public int Elegate;
//}


//[TranspilationSource]
//public class ClassIncludeDictionary3
//{
//    public Dictionary<string, DateGuidStruct2> DictionaryProperty { get; } = new();
//}


//[TranspilationSource]
//public class ClassIncludeDictionary4
//{
//    public Dictionary<string, Dictionary<string, DateGuidStruct2>> DictionaryProperty { get; } = new();
//}



//[TranspilationSource]
//public class ClassIncludeIDictionary
//{
//    public IDictionary<string, int>? DictionaryProperty { get; set; }
//}


//[TranspilationSource]
//public class ClassIncludeIDictionary2
//{
//    public IDictionary<string, ClassIncludeDictionary>? DictionaryProperty { get; set; }
//}

//[TranspilationSource]
//public class ClassIncludeIDictionary3
//{
//    public IDictionary<string, DateGuidStruct2>? DictionaryProperty { get; set; }
//}


//[TranspilationSource]
//public class ClassIncludeIDictionary4
//{
//    public IDictionary<string, IDictionary<string, DateGuidStruct2>>? DictionaryProperty { get; set; }
//}


//[TranspilationSource]
//public enum Enum1
//{
//    None,
//    One,
//    Two,
//    Three,
//}

//[TranspilationSource]
//public enum Enum2
//{
//    None,
//    One = 1 << 1,
//    Two = 1 << 2,
//    Three = 1 << 3,
//}


//[TranspilationSource]
//public class IncludeStaticFields
//{
//    public static int StaticValue { get; set; }
//    public static string? String;

//    public int Field;
//    public int Property { get; set; }
//}


//[TranspilationSource]
//public class ClassIncludeCollectionFieldArraySegmentint
//{
//    public ArraySegment<int>? FieldOfArraySegmentint;
//}


//[TranspilationSource]
//public class ClassIncludeCollectionFieldArraySegmentint
//{
//    public int? FieldOfArraySegmentint;
//}

//[TranspilationSource]
//public class ClassIncludeCollectionFieldLinkedListstring
//{
//    public LinkedList<string>? FieldOfLinkedListstring;
//}

//[TranspilationSource]
//public class TupleClassIncludeNullable
//{
//    public (int?, Guid, DateTime) TapluField;
//}

//[TranspilationSource]
//public class SimpleTupleClass
//{
//    public (int, string) TapluField;
//}

//[TranspilationSource]
//public class TupleClassIncludeNullable
//{
//    public (int?, Guid, DateTime) TapluField;
//}

//[TranspilationSource]
//public class TupleClassNullableField
//{
//    public (int?, Guid, DateTime)? TapluField;
//}

//[TranspilationSource]
//public class CustomType
//{
//    public int Field { get; set; }
//    public int Property { get; set; }
//}

//[TranspilationSource]
//public class TupleClassIncludeCustomType
//{
//    public (int?, CustomType, DateTime)? TapluField;
//    public (int, CustomType?, DateTime?) TapluPropery { get; set; }
//}

//[TranspilationSource]
//class Hoge<T>
//{

//}

[TranspilationSource]
class Type1
{

}

//[TranspilationSource]
//class Type2
//{
//    public Type1? Type1 { get; set; }
//}

//[TranspilationSource]
//class NotSupport
//{
//    public Func<int, int>? GetPointer;
//}


//[TranspilationSource]
//class Generics<T>
//{
//    public T Value { get; set; }
//}

[TranspilationSource]
public record Type111(Guid Id, string? Name, int? Value);
