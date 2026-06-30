using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Bitvavo.Net.Clients.MessageHandlers;

/// <summary>
/// REST message handler for Bitvavo's spot API. Bitvavo error envelope shape:
/// <c>{ "errorCode": int, "error": "msg" }</c>. Mirrors KrakenRestSpotMessageHandler but
/// adapted to Bitvavo's flat error structure.
/// </summary>
internal sealed class BitvavoRestSpotMessageHandler : JsonRestMessageHandler
{
    private readonly ErrorMapping _errorMapping;

    public override JsonSerializerOptions Options { get; }
        = SerializerOptions.WithConverters(BitvavoExchange._serializerContext);

    public BitvavoRestSpotMessageHandler(ErrorMapping errorMapping)
    {
        _errorMapping = errorMapping;
    }

    /// <inheritdoc />
    public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
    {
        var (parseError, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
        if (parseError != null) return parseError;

        // Defensive: only Bitvavo error envelopes are JSON objects. Some endpoints (e.g. /candles)
        // return arrays, and TryGetProperty on a non-object element throws InvalidOperationException.
        if (document!.RootElement.ValueKind != JsonValueKind.Object)
            return new ServerError(ErrorInfo.Unknown);

        var code = document.RootElement.TryGetProperty("errorCode", out var c) ? c.GetInt32().ToString() : null;
        var msg  = document.RootElement.TryGetProperty("error",     out var m) ? m.GetString()        : null;

        return code == null 
            ? new ServerError(ErrorInfo.Unknown) 
            : new ServerError(code, _errorMapping.GetErrorInfo(code, msg));
    }
}
