using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.MessageHandlers
{
    internal class BitvavoRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = BitvavoExchange._serializerContext;

        public BitvavoRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            var (error, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (error != null)
                return error;

            using var response = document!;
            var root = response.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return new ServerError(ErrorInfo.Unknown);

            var message = root.TryGetProperty("error", out var messageValue) && messageValue.ValueKind == JsonValueKind.String
                ? messageValue.GetString()
                : null;
            if (!root.TryGetProperty("errorCode", out var codeValue) || codeValue.ValueKind != JsonValueKind.Number || !codeValue.TryGetInt32(out var code))
                return new ServerError(ErrorInfo.Unknown with { Message = message });

            var errorCode = code.ToString(CultureInfo.InvariantCulture);
            return new ServerError(errorCode, _errorMapping.GetErrorInfo(errorCode, message));
        }

        public override async ValueTask<ServerRateLimitError> ParseErrorRateLimitResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            var error = await ParseErrorResponse(httpStatusCode, responseHeaders, responseStream).ConfigureAwait(false);
            DateTime? retryAfter;
            if (responseHeaders.TryGetValues("bitvavo-ratelimit-resetat", out var resetValues)
                && long.TryParse(resetValues.FirstOrDefault(), NumberStyles.None, CultureInfo.InvariantCulture, out var reset)
                && reset <= DateTimeOffset.MaxValue.ToUnixTimeMilliseconds())
                retryAfter = DateTimeOffset.FromUnixTimeMilliseconds(reset).UtcDateTime;
            else
                retryAfter = (await base.ParseErrorRateLimitResponse(httpStatusCode, responseHeaders, responseStream).ConfigureAwait(false)).RetryAfter?.ToUniversalTime();

            var rateLimitError = new ServerRateLimitError
            {
                ErrorCode = error.ErrorCode,
                RetryAfter = retryAfter
            };
            if (error is ServerError)
                rateLimitError.Message = error.Message;
            if (error.ErrorType == ErrorType.RateLimitRequest)
            {
                rateLimitError.ErrorDescription = error.ErrorDescription;
                rateLimitError.IsTransient = error.IsTransient;
            }

            return rateLimitError;
        }
    }
}
