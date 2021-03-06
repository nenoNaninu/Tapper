// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY Tapper
// </auto-generated>
#nullable enable
using System;
using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

public class SystemArrayTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public SystemArrayTypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Array")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (SymbolEqualityComparer.Default.Equals(typeSymbol, Assign))
        {
            return "any[]";
        }

        throw new InvalidOperationException($"SystemArrayTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class ArraySegment1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public ArraySegment1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.ArraySegment`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"ArraySegment1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class List1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public List1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.List`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"List1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class LinkedList1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public LinkedList1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.LinkedList`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"LinkedList1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class Queue1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public Queue1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.Queue`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"Queue1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class Stack1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public Stack1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.Stack`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"Stack1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class HashSet1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public HashSet1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.HashSet`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"HashSet1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class IEnumerable1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public IEnumerable1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"IEnumerable1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class IReadOnlyCollection1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public IReadOnlyCollection1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyCollection`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"IReadOnlyCollection1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class IReadOnlyList1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public IReadOnlyList1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.IReadOnlyList`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"IReadOnlyList1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class ICollection1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public ICollection1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.ICollection`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"ICollection1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class IList1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public IList1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.IList`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"IList1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public class ISet1TypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public ISet1TypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.Collections.Generic.ISet`1")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol
            && namedTypeSymbol.IsGenericType
            && SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, Assign))
        {
            var typeArgument = namedTypeSymbol.TypeArguments[0];
            var mapper = options.TypeMapperProvider.GetTypeMapper(typeArgument);
            return $"{mapper.MapTo(typeArgument, options)}[]";
        }

        throw new InvalidOperationException($"ISet1TypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

public static class CollectionTypeTypeMappers
{
    public static ITypeMapper[] Create(Compilation compilation)
    {
        var mappers = new ITypeMapper[13];

        mappers[0] = new SystemArrayTypeMapper(compilation);
        mappers[1] = new ArraySegment1TypeMapper(compilation);
        mappers[2] = new List1TypeMapper(compilation);
        mappers[3] = new LinkedList1TypeMapper(compilation);
        mappers[4] = new Queue1TypeMapper(compilation);
        mappers[5] = new Stack1TypeMapper(compilation);
        mappers[6] = new HashSet1TypeMapper(compilation);
        mappers[7] = new IEnumerable1TypeMapper(compilation);
        mappers[8] = new IReadOnlyCollection1TypeMapper(compilation);
        mappers[9] = new IReadOnlyList1TypeMapper(compilation);
        mappers[10] = new ICollection1TypeMapper(compilation);
        mappers[11] = new IList1TypeMapper(compilation);
        mappers[12] = new ISet1TypeMapper(compilation);

        return mappers;
    }
}
