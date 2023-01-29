using Microsoft.CodeAnalysis;

namespace Tapper;

public class SpecialSymbols
{
    public INamedTypeSymbol? JsonPropertyNameAttribute { get; }
    public INamedTypeSymbol? JsonIgnoreAttribute { get; }
    public INamedTypeSymbol? MessagePackKeyAttribute { get; }
    public INamedTypeSymbol? MessagePackIgnoreMemberAttribute { get; }

    public SpecialSymbols(Compilation compilation)
    {
        JsonPropertyNameAttribute = compilation.GetTypeByMetadataName("System.Text.Json.Serialization.JsonPropertyNameAttribute");
        JsonIgnoreAttribute = compilation.GetTypeByMetadataName("System.Text.Json.Serialization.JsonIgnoreAttribute");

        MessagePackKeyAttribute = compilation.GetTypeByMetadataName("MessagePack.KeyAttribute");
        MessagePackIgnoreMemberAttribute = compilation.GetTypeByMetadataName("MessagePack.IgnoreMemberAttribute");
    }
}
