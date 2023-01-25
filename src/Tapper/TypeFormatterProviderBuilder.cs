using System;
using Tapper.TypeFormatters;

namespace Tapper;

public static class TypeFormatterProviderBuilder
{
    public static ITypeFormatterProvider Build(EnumStyle enumStyle)
    {
        var messageTypeFormatter = new DefaultMessageTypeFormatter();

        ITypeFormatter enumTypeFormatter = enumStyle switch
        {
            EnumStyle.Value => new EnumTypeFormatter(),
            EnumStyle.Name or EnumStyle.NameCamel or EnumStyle.NamePascal => new StringEnumTypeFormatter(),
            EnumStyle.Union or EnumStyle.UnionCamel or EnumStyle.UnionPascal => new UnionEnumTypeFormatter(),
            _ => throw new InvalidOperationException()
        };

        return new DefaultTypeFormatterProvider(messageTypeFormatter, enumTypeFormatter);
    }
}
