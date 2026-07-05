using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal;

internal sealed record BitvavoSocketAuthResponse
{
    [JsonPropertyName("event")]
    public string Event { get; init; } = string.Empty;

    [JsonPropertyName("authenticated")]
    public bool Authenticated { get; init; }
}
