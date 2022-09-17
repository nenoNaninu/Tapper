using System;
using Microsoft.CodeAnalysis;

namespace Tapper.TypeMappers;

internal class DateTimeTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public DateTimeTypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.DateTime")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (SymbolEqualityComparer.Default.Equals(typeSymbol, Assign))
        {
            if (options.SerializerOption == SerializerOption.MessagePack)
            {
                return "Date";
            }
            else
            {
                return "(Date | string)";
            }
        }

        throw new InvalidOperationException($"DateTimeTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

internal class DateTimeOffsetTypeMapper : ITypeMapper
{
    public ITypeSymbol Assign { get; }

    public DateTimeOffsetTypeMapper(Compilation compilation)
    {
        Assign = compilation.GetTypeByMetadataName("System.DateTimeOffset")!;
    }

    public string MapTo(ITypeSymbol typeSymbol, ITranspilationOptions options)
    {
        if (SymbolEqualityComparer.Default.Equals(typeSymbol, Assign))
        {
            if (options.SerializerOption == SerializerOption.MessagePack)
            {
                return "[Date, number]";
            }
            else
            {
                return "(Date | string)";
            }
        }

        throw new InvalidOperationException($"DateTimeOffsetTypeMapper is not support {typeSymbol.ToDisplayString()}.");
    }
}

