using Space1;
using Tapper;

namespace Tapper.Test.SourceTypes
{

    [TranspilationSource]
    public class GenericClass1<T>
    {
        public required string StringProperty { get; set; }
        public required T GenericProperty { get; set; }
    }

    [TranspilationSource]
    public class NestedGenericClass<T1, T2>
    {
        public required string StringProperty { get; set; }
        public required T1 GenericProperty { get; set; }
        public required GenericClass1<T1> GenericClass1Property { get; set; }
        public required GenericClass2<T1, T2> GenericClass2Property { get; set; }
    }

    [TranspilationSource]
    public class DeeplyNestedGenericClass<A, B, C>
    {
        public required string StringProperty { get; set; }
        public required A GenericPropertyA { get; set; }
        public required B GenericPropertyB { get; set; }
        public required GenericClass1<A> GenericClass1Property { get; set; }
        public required GenericClass2<B, C> GenericClass2Property { get; set; }
        public required NestedGenericClass<string, B> NestedGenericClassProperty { get; set; }
    }

    [TranspilationSource]
    public class InheritedGenericClass2<T1, T2> : GenericClass1<T1>
    {
        public required T2 GenericPropertyT2 { get; set; }
    }


    [TranspilationSource]
    public class InheritedConcreteGenericClass : GenericClass2<bool, int>
    {
    }
}
namespace Space1
{

    [TranspilationSource]
    public class GenericClass2<T1, T2>
    {
        public required string StringProperty { get; set; }
        public required T1 GenericProperty1 { get; set; }
        public required T2 GenericProperty2 { get; set; }
    }
}
