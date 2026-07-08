using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal;

internal class BitvavoSocketRequest
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("channels")]
    public BitvavoSocketChannelBase[] Channels { get; set; } = System.Array.Empty<BitvavoSocketChannelBase>();
}


internal class BitvavoSocketChannelBase
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

internal class BitvavoSocketChannel : BitvavoSocketChannelBase
{

    [JsonPropertyName("interval"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Interval { get; set; }

    [JsonPropertyName("markets")]
    public string[] Symbols { get; set; } = System.Array.Empty<string>();
}

internal class BitvavoAllMarketRequest : BitvavoSocketChannelBase
{

    [JsonPropertyName("markets")]
    public string Markets { get; set; } = "*";
}
