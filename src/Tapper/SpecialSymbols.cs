using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Tapper;

public class SpecialSymbols
{
    public ImmutableArray<INamedTypeSymbol> JsonPropertyNameAttributes { get; }
    public ImmutableArray<INamedTypeSymbol> JsonIgnoreAttributes { get; }
    public ImmutableArray<INamedTypeSymbol> MessagePackKeyAttributes { get; }
    public ImmutableArray<INamedTypeSymbol> MessagePackIgnoreMemberAttributes { get; }

    public SpecialSymbols(Compilation compilation)
    {
        JsonPropertyNameAttributes = compilation.GetTypesByMetadataName("System.Text.Json.Serialization.JsonPropertyNameAttribute");
        JsonIgnoreAttributes = compilation.GetTypesByMetadataName("System.Text.Json.Serialization.JsonIgnoreAttribute");

        MessagePackKeyAttributes = compilation.GetTypesByMetadataName("MessagePack.KeyAttribute");
        MessagePackIgnoreMemberAttributes = compilation.GetTypesByMetadataName("MessagePack.IgnoreMemberAttribute");
    }
}
