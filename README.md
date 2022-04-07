# Tapper

Tapper is a library/CLI tool to transpile C# type (class, struct, record, enum) into TypeScript type (type, enum).
Using this tool can reduce serialization bugs (type mismatch, typos, etc.) and make TypeScript code easily follow changes in C# code.

![teaser](https://user-images.githubusercontent.com/27144255/161368393-a85552c8-ba18-4376-a84e-28f411be16f7.png)

## Table of Contents
- [Packages](#packages)
  - [Install Using .NET Tool](#install-using-net-tool)
- [Getting Started](#getting-started)
- [Transpilation Rules](#transpilation-rules)
  - [Built-in Supported Types](#built-in-supported-types)
  - [C# Namespace](#c-namespace)
  - [Nesting Types](#nesting-types)
- [Options](#options)
  - [Naming Style](#naming-style)
  - [Serializer](#serializer)
    - [MessagePack](#messagepack)
- [Analyzer](#analyzer)
- [Unity Support](#unity-support)

## Packages

- [Tapper.Attributes](https://www.nuget.org/packages/Tapper.Attributes/)
- [Tapper.Analyzer](https://www.nuget.org/packages/Tapper.Analyzer/)
- [Tapper.Generator](https://www.nuget.org/packages/Tapper.Generator/) (.NET Tool)

### Install Using .NET Tool

Use `Tapper.Generator`(CLI Tool) to generate TypeScript type from C# type. 
`Tapper.Generator` can be easily installed using .NET Global Tools.
You can use the installed tools with the command `tapper`.

```
dotnet tool install --global Tapper.Generator
tapper help
```

## Getting Started

First, add the following packages to your project.
Tapper.Analyzer is optional, but recommended.

```
dotnet add package Tapper.Attributes
dotnet add package Tapper.Analyzer (optional, but recommended.)
```

Next, apply the `[TranspilationSource]` Attribute to the C# type definition.

```cs
using Tapper;

namespace SampleNamespace;

[TranspilationSource] // <- Add attribute!
public class SampleType
{
    public List<int>? List { get; }
    public int Value { get; }
    public Guid Id { get; }
    public DateTime DateTime { get; }
}
```

Then execute the command as follows.

```
tapper --project path/to/XXX.csproj --output outdir
```

TypeScript source code is generated in the directory specified by `--output`.
In this example, TypeScript source code named `outdir/SampleNamespace.ts` is generated.
The contents of the generated code is as follows.

```ts
/* eslint-disable */

/** Transpied from SampleNamespace.SampleType */
export type SampleType = {
  /** Transpied from System.Collections.Generic.List<int>? */
  List?: number[];
  /** Transpied from int */
  Value: number;
  /** Transpied from System.Guid */
  Id: string;
  /** Transpied from System.DateTime */
  DateTime: (Date | string);
}
```

## Transpilation Rules

Tapper transpile C# types (class, struct, record, enum) to TypeScript  types (type, enum).
When transpiling class, struct, and record, only `public` fields and properties are transpiled.

### Built-in Supported Types

|  C#  |  TypeScript  | Description |
| ---- | ---- | ---- |
| bool |  boolean  |
| byte |  number  |
| sbyte |  number  |
| char |  string or number  | JSON: `string`, MessagePack `number`. |
| decimal |  number |
| double |  number |
| float |  number |
| int |  number |
| uint |  number |
| long |  number |
| ulong |  number |
| short |  number |
| ushort |  number |
| object |  any  |
| string |  string  |
| Uri |  string  |
| Guid |  string  | Compatible with TypeScript's `crypto.randomUUID()`. | 
| DateTime | (Date \| string) or Date | Json: `(Date \| string)`,  MessagePack: `Date`. |
| System.Nullable\<T\>| (T \| undefined) |
| T[] | T[] | 
| System.Array | any[] | ❌ System.Text.Json |
| ArraySegment\<T\> | T[] | ❌ System.Text.Json |
| List\<T\> | T[] |
| LinkedList\<T\> | T[] |
| Queue\<T\> | T[] |
| Stack\<T\> | T[] |
| HashSet\<T\> | T[] |
| IEnumerable\<T\> | T[] |
| IReadOnlyCollection\<T\> | T[] |
| ICollection\<T\> | T[] |
| IList\<T\> | T[] |
| ISet\<T\> | T[] |
| Dictionary<TKey, TValue> |  { [key: TKey]: TValue } |
| IDictionary<TKey, TValue> |  { [key: TKey]: TValue } |
| IReadOnlyDictionary<TKey, TValue> |  { [key: TKey]: TValue } |
| Tuple | [T1, T2, ...] | ❌ System.Text.Json |



### C# Namespace

C# namespace is mapped to the filename of the generated TypeScript code.

```cs
namespace SampleNamespace;

[TranspilationSource]
record Xxx();
```

For example, given the above C# code, TypeScript code with the file name `SampleNamespace.ts` is generated.

### Nesting Types

It doesn't matter if the user-defined types are nested.
For example, consider the following C# code.
Apply `[TranspilationSource]` Attribute to all types to be transpiled.
If you add an analyzer package, you can avoid forgetting to apply `[TranspilationSource]`.

```cs
#nullable enable

namespace Space1
{
    [TranspilationSource]
    public class CustomType1
    {
        public int Value;
        public Guid Id;
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
```

The following TypeScript code is generated.

- Space1.ts

```ts
/** Transpied from Space1.CustomType1 */
export type CustomType1 = {
  /** Transpied from int */
  Value: number;
  /** Transpied from System.Guid */
  Id: string;
}
```

- Space1.Sub.ts

```ts
/** Transpied from Space1.Sub.MyEnum */
export enum MyEnum {
  Zero = 0,
  One = 1,
  Two = 2,
  Four = 4,
}
```

- Space2.ts

```ts
/** Transpied from Space2.CustomType3 */
export type CustomType3 = {
  /** Transpied from float */
  Value: number;
  /** Transpied from System.DateTime */
  Name: (Date | string);
}
```

- Space3.ts 

```ts
import { CustomType1 } from './Space1';
import { MyEnum } from './Space1.Sub';
import { CustomType3 } from './Space2';

/** Transpied from Space3.NastingNamespaceType */
export type NastingNamespaceType = {
  /** Transpied from Space1.CustomType1? */
  Value?: CustomType1;
  /** Transpied from Space1.Sub.MyEnum */
  MyEnumValue: MyEnum;
  /** Transpied from System.Collections.Generic.List<Space2.CustomType3> */
  List: CustomType3[];
}
```

## Options

### Naming Style

You can select `camelCase`, `PascalCase`, or `none` for the property name of the generated TypeScript type.
For `none`, the property name in C# is used.
The default is `none`.

```
tapper --project path/to/Xxx.csproj --output outdir --naming-style camelCase
```

### Serializer

The TypeScript code generated by Tapper is supposed to be serialized/deserialized with `json` or` MessagePack`.
And the appropriate type is slightly different depending on the serializer.
You can specify which one to use by passing the `--serializer` option.
The default is `json`.

```
tapper --project path/to/Xxx.csproj --output outdir --serializer MessagePack
```

Also, it is supposed that the following serializers are used.

- Json
    - C# : System.Text.Json
    - TypeScript : JSON.stringify()

- MessagePack
    - C# : [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp)
    - TypeScript : [msgpack-javascript](https://github.com/msgpack/msgpack-javascript)

#### MessagePack

If you use [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) for the serializer, be careful how you apply the `[MessagePackObject]` Attribute. 
It is recommended to use `[MessagePackObject(true)]`. 
Also, in that case, set `--naming-style` to `none`.

```cs
[MessagePackObject(true)]
public class SampleType
{
    public Guid Id { get; set; }
    public int Value { get; set; }
}
```

## Analyzer

Tapper has some rules. You can easily follow those rules by adding `Tapper.Analyzer`.

- If the fields and property types contained in the type to which `[TranspilationSource]` applies are user-defined types, you must also apply `[TranspilationSource]` to those types.
- You cannot apply `[TranspilationSource]` to Generic type.

![](https://user-images.githubusercontent.com/27144255/161054234-9e9000a7-7958-48ea-bc04-a4255aa49e3e.png)

## Unity Support

For Unity projects, first, copy and paste the  [TranspilationSourceAttribute.cs](/src/Tapper.Attributes/TranspilationSourceAttribute.cs) into your project. 
Then apply `[TranspilationSource]` to types you want to transpile.

Next, a file named `Assembly-CSharp.csproj` is generated by Unity.
It is in the same hierarchy as Assets.
Use this project file as an argument to `--project`.

```
tapper --project path/to/Assembly-CSharp.csproj --output outdir
```
