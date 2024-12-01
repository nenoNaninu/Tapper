# Tapper

[![NuGet](https://img.shields.io/nuget/v/Tapper.Attributes.svg)](https://www.nuget.org/packages/Tapper.Attributes)
[![build-and-test](https://github.com/nenoNaninu/Tapper/actions/workflows/build-and-test.yaml/badge.svg?branch=main)](https://github.com/nenoNaninu/Tapper/actions/workflows/build-and-test.yaml)

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
  - [Enum Style](#enum-style)
  - [Serializer](#serializer)
    - [MessagePack](#messagepack)
  - [Serializer Attributes Support](#serializer-attributes-support)
    - [JSON](#json)
    - [MessagePack](#messagepack-1)
  - [Transpile the Types Contained in Referenced Assemblies](#transpile-the-types-contained-in-referenced-assemblies)
- [Analyzer](#analyzer)
- [Unity Support](#unity-support)
- [Related Work](#related-work)

## Packages

- [Tapper.Attributes](https://www.nuget.org/packages/Tapper.Attributes/)
- [Tapper.Analyzer](https://www.nuget.org/packages/Tapper.Analyzer/)
- [Tapper.Generator](https://www.nuget.org/packages/Tapper.Generator/) (.NET Tool)

### Install Using .NET Tool

Use `Tapper.Generator`(CLI Tool) to generate TypeScript type from C# type. 
`Tapper.Generator` can be easily installed using .NET Global Tools.
You can use the installed tools with the command `tapper`.

```bash
# install
# Tapper CLI (dotnet tool) requires .NET 7 or .NET 8, but your app TFM can use .NET 6, etc.
$ dotnet tool install --global Tapper.Generator
$ tapper help

# update
$ dotnet tool update --global Tapper.Generator
```

## Getting Started

First, add the following packages to your project.
Tapper.Analyzer is optional, but recommended.

```bash
$ dotnet add package Tapper.Attributes
$ dotnet add package Tapper.Analyzer (optional, but recommended.)
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

```bash
$ tapper --project path/to/XXX.csproj --output outdir
```

TypeScript source code is generated in the directory specified by `--output`.
In this example, TypeScript source code named `outdir/SampleNamespace.ts` is generated.
The contents of the generated code is as follows.

```ts
/* eslint-disable */

/** Transpiled from SampleNamespace.SampleType */
export type SampleType = {
  /** Transpiled from System.Collections.Generic.List<int>? */
  list?: number[];
  /** Transpiled from int */
  value: number;
  /** Transpiled from System.Guid */
  id: string;
  /** Transpiled from System.DateTime */
  dateTime: (Date | string);
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
| DateTime | `(Date \| string)` or `Date` | Json: `(Date \| string)`,  MessagePack: `Date`. |
| DateTimeOffset | `(Date \| string)` or `[Date, number]` | Json: `(Date \| string)`,  MessagePack: `[Date, number]`. note [#41](https://github.com/nenoNaninu/Tapper/pull/41) |
| TimeSpan | `string` or `number` | Json: `string`,  MessagePack: `number`. |
| System.Nullable\<T\>| (T \| undefined) |
| byte[] | string or Uint8Array | JSON: `string` (base64), MessagePack `Uint8Array`. |
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
| Dictionary<TKey, TValue> |  Partial<Record<TKey, TValue>> |
| IDictionary<TKey, TValue> |  Partial<Record<TKey, TValue>> |
| IReadOnlyDictionary<TKey, TValue> |  Partial<Record<TKey, TValue>> |
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
using System.Text.Json.Serialization;
using Tapper;

namespace Space1
{
    [TranspilationSource]
    public class CustomType1
    {
        public int Value;
        public Guid Id;
        [JsonIgnore]
        public string Foo;
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
        [JsonPropertyName("list")]
        public List<CustomType3> MyList { get; set; } = new();
    }
}
```

The following TypeScript code is generated.

- Space1.ts

```ts
/** Transpiled from Space1.CustomType1 */
export type CustomType1 = {
  /** Transpiled from int */
  value: number;
  /** Transpiled from System.Guid */
  id: string;
}
```

- Space1.Sub.ts

```ts
/** Transpiled from Space1.Sub.MyEnum */
export enum MyEnum {
  Zero = 0,
  One = 1,
  Two = 2,
  Four = 4,
}
```

- Space2.ts

```ts
/** Transpiled from Space2.CustomType3 */
export type CustomType3 = {
  /** Transpiled from float */
  value: number;
  /** Transpiled from System.DateTime */
  name: (Date | string);
}
```

- Space3.ts 

```ts
import { CustomType1 } from './Space1';
import { MyEnum } from './Space1.Sub';
import { CustomType3 } from './Space2';

/** Transpiled from Space3.NastingNamespaceType */
export type NastingNamespaceType = {
  /** Transpiled from Space1.CustomType1? */
  value?: CustomType1;
  /** Transpiled from Space1.Sub.MyEnum */
  myEnumValue: MyEnum;
  /** Transpiled from System.Collections.Generic.List<Space2.CustomType3> */
  list: CustomType3[];
}
```

## Options

### Naming Style

You can select `camelCase`, `PascalCase`, or `none` for the property name of the generated TypeScript type.
For `none`, the property name in C# is used.
The default is the standard naming style for TypeScript.

```bash
$ tapper --project path/to/Xxx.csproj --output outdir --naming-style camelCase
```

### Enum Style
There are options for enum transpiling.
You can select `Value` (default), `Name`, `NameCamel`, `NamePascal`, `Union`, `UnionCamel`, or `UnionPascal`.
If you use this option, be careful with the serializer options. 
For example, `System.Text.Json` serializes an enum as a integer by default (not string).
To serialize an enum as a string, you must pass `JsonStringEnumConverter` as an option to `JsonSerializer`.

```bash
$ tapper --project path/to/Xxx.csproj --output outdir --enum value
$ tapper --project path/to/Xxx.csproj --output outdir --enum name
$ tapper --project path/to/Xxx.csproj --output outdir --enum union
```

```cs
// C# source
[TranspilationSource]
public enum MyEnum
{
    Zero = 0,
    One = 1,
    Two = 1 << 1,
    Four = 1 << 2,
}
```

```ts
// Generated TypeScript

// --enum value (default)
export enum MyEnum {
  Zero = 0,
  One = 1,
  Two = 2,
  Four = 4,
}

// --enum name
export enum MyEnum {
  Zero = "Zero",
  One = "One",
  Two = "Two",
  Four = "Four",
}

// --enum union
export type MyEnum = "Zero" | "One" | "Two" | "Four";

// --enum unionCamel
export type MyEnum = "zero" | "one" | "two" | "four";
```

### Serializer

The TypeScript code generated by Tapper is supposed to be serialized/deserialized with `json` or` MessagePack`.
And the appropriate type is slightly different depending on the serializer.
You can specify which one to use by passing the `--serializer` option.
The default is `json`.

```bash
$ tapper --project path/to/Xxx.csproj --output outdir --serializer MessagePack --naming-style none
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

```bash
$ tapper --project path/to/Xxx.csproj --output outdir --serializer MessagePack --naming-style none
```

```cs
[MessagePackObject(true)] // <- use this!
public class SampleType
{
    public Guid Id { get; set; }
    public int Value { get; set; }
}
```

### Serializer Attributes Support

Tapper reflects JSON and MessagePack serializer attributes in the output TypeScript code.

Support attributes: 
- `System.Text.Json.Serialization`
    - `[JsonPropertyName("string")]`
    - `[JsonIgnore]`
- `MessagePack`
    - `[Key("string")]`
    - `[IgnoreMember]`

#### JSON
```cs
// input C# code
// --serializer json

namespace Readme;

[TranspilationSource]
public class Type1
{
    [JsonIgnore]
    public required int Value { get; init; }
    public required string Name { get; init; }
}

[TranspilationSource]
public class Type2
{
    [JsonPropertyName("Foo")]
    public required int Value { get; init; }
    public required string Name { get; init; }
}
```

```ts
// output TypeScript code

/** Transpiled from Readme.Type1 */
export type Type1 = {
    /** Transpiled from string */
    name: string;
}

/** Transpiled from Readme.Type2 */
export type Type2 = {
    /** Transpiled from int */
    Foo: number;
    /** Transpiled from string */
    name: string;
}
```


#### MessagePack

```cs
// input C# code
// --serializer MessagePack --naming-style none

namespace Readme;

[TranspilationSource]
public class Type3
{
    [IgnoreMember]
    public required Value { get; init; }
    public required string Name { get; init; }
}

[TranspilationSource]
public class Type4
{
    [Key("Bar")]
    public required int Value { get; init; }
    public required string Name { get; init; }
}
```

```ts
// output TypeScript code

/** Transpiled from Readme.Type3 */
export type Type3 = {
    /** Transpiled from string */
    Name: string;
}

/** Transpiled from Readme.Type4 */
export type Type4 = {
    /** Transpiled from int */
    Bar: number;
    /** Transpiled from string */
    Name: string;
}
```


### Transpile the Types Contained in Referenced Assemblies

By default, only types defined in the project specified by the `--project` option are targeted for transpiling.
By passing the `--asm true` option, types contained in project/package reference assemblies will also be targeted for transpiling.

```bash
$ tapper --project path/to/Xxx.csproj --output outdir --asm true
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

```bash
$ tapper --project path/to/Assembly-CSharp.csproj --output outdir
```

## Related Work

- [nenoNaninu/TypedSignalR.Client.TypeScript](https://github.com/nenoNaninu/TypedSignalR.Client.TypeScript)
  - TypeScript source generator to provide strongly typed SignalR clients by analyzing C# type definitions.
