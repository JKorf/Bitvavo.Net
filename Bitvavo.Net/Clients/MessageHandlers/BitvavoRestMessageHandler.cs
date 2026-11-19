using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
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

            int? code = document!.RootElement.TryGetProperty("errorCode", out var codeProp) && codeProp.TryGetInt32(out var errorCode) ? errorCode : null;
            string? msg = document.RootElement.TryGetProperty("error", out var msgProp) ? msgProp.GetString() : null;
            if (msg == null)
                return new ServerError(ErrorInfo.Unknown);

            if (code == null)
                return new ServerError(new ErrorInfo(ErrorType.Unknown, false, msg));

            var errorInfo = _errorMapping.GetErrorInfo(code.ToString()!, msg);
            return new ServerError(code.Value.ToString(), errorInfo);
        }
    }
}
