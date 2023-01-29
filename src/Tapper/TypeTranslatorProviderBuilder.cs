using System;
using Tapper.TypeTranslators;

namespace Tapper;

public sealed class TypeTranslatorProviderBuilder
{
    private readonly EnumStyle _enumStyle;

    public TypeTranslatorProviderBuilder(EnumStyle enumStyle)
    {
        _enumStyle = enumStyle;
    }

    public ITypeTranslatorProvider Build()
    {
        var messageTypeTranslator = new DefaultMessageTypeTranslator();

        ITypeTranslator enumTypeTranslator = _enumStyle switch
        {
            EnumStyle.Value => new EnumTypeTranslator(),
            EnumStyle.Name or EnumStyle.NameCamel or EnumStyle.NamePascal => new StringEnumTypeTranslator(),
            EnumStyle.Union or EnumStyle.UnionCamel or EnumStyle.UnionPascal => new UnionEnumTypeTranslator(),
            _ => throw new InvalidOperationException()
        };

        return new DefaultTypeTranslatorProvider(messageTypeTranslator, enumTypeTranslator);
    }
}
