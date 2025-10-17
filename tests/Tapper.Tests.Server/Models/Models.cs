using System;
using System.Collections.Generic;
using MessagePack;

namespace Tapper.Tests.Server.Models;

//public class Type1
//{
//    public Guid Id { get; }

//    // nullable refarence
//    public string? Name { get; }

//    // nullable struct
//    public int? Value { get; }


//}

[MessagePackObject(true)]
[TranspilationSource]
public record Type1(Guid Id, string Name, int? Value);

[MessagePackObject(true)]
[TranspilationSource]
public class Type2
{
    public DateTime DateTime { get; }
    public Uri? Uri { get; }

    public Type2(DateTime dateTime, Uri? uri)
    {
        DateTime = dateTime;
        Uri = uri;
    }
}

//public record Type2(DateTime DateTime, Uri? Uri);

[MessagePackObject(true)]
[TranspilationSource]
public class Type3
{
    public List<Type1> CustomTypeList { get; }

    public Type3(List<Type1> customTypeList)
    {
        CustomTypeList = customTypeList;
    }
}

[TranspilationSource]
public enum MyEnum
{
    Zero = 0,
    One = 1,
    Two = 2,
    Four = 4
}

[TranspilationSource]
public record Type4(MyEnum MyEnum, float Value);


[TranspilationSource]
public record Type5(char Value);


[TranspilationSource]
public record Type6(byte[] Binary);

[TranspilationSource]
public record Type7(DateTimeOffset DateTimeOffset);
