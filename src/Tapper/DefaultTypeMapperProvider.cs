using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tapper.TypeMappers;

namespace Tapper;

public class DefaultTypeMapperProvider : ITypeMapperProvider
{
    private readonly ArrayTypeMapper _arrayTypeMapper;
    private readonly TupleTypeMapper _tupleTypeMapper;

    private readonly IDictionary<ITypeSymbol, ITypeMapper> _mappers;

    public DefaultTypeMapperProvider(Compilation compilation)
    {
        _arrayTypeMapper = new ArrayTypeMapper(compilation);
        _tupleTypeMapper = new TupleTypeMapper();

        var dateTimeTypeMapper = new DateTimeTypeMapper(compilation);
        var nullableStructTypeMapper = new NullableStructTypeMapper(compilation);

        var primitiveTypeMappers = PrimitiveTypeMappers.Create(compilation);
        var collectionTypeTypeMappers = CollectionTypeTypeMappers.Create(compilation);
        var dictionalyTypeMappers = DictionaryTypeMappers.Create(compilation);

        var sourceTypeMapper = compilation.GetSourceTypes()
            .Select(static x => new SourceTypeMapper(x));

        var typeMappers = sourceTypeMapper.Concat(primitiveTypeMappers)
            .Concat(collectionTypeTypeMappers)
            .Concat(dictionalyTypeMappers)
            .Concat(new ITypeMapper[] { dateTimeTypeMapper, nullableStructTypeMapper });

        _mappers = typeMappers.ToDictionary<ITypeMapper, ITypeSymbol>(x => x.Assign, SymbolEqualityComparer.Default);
    }

    public ITypeMapper GetTypeMapper(ITypeSymbol type)
    {
        if (type.IsTupleType)
        {
            return _tupleTypeMapper;
        }

        if (type is IArrayTypeSymbol)
        {
            return _arrayTypeMapper;
        }

        var sourceType = type is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType
            ? namedTypeSymbol.ConstructedFrom
            : type;

        if (_mappers.TryGetValue(sourceType, out var typeMapper))
        {
            return typeMapper;
        }

        throw new InvalidOperationException($"{type.ToDisplayString()} is not supported.");
    }

    public void AddTypeMapper(ITypeMapper typeMapper)
    {
        _mappers[typeMapper.Assign] = typeMapper;
    }
}
