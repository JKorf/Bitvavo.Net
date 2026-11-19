using CryptoExchange.Net.SharedApis;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoSocketClientExchangeSharedApi :
        SharedApiBase,
        IBitvavoSocketClientExchangeSharedApi
    {
        private readonly BitvavoSocketClientExchangeApi _api;

        private const string _topicId = "BitvavoExchange";
        private const string _exchangeName = "Bitvavo";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BitvavoExchange.Metadata, this);

        public BitvavoSocketClientExchangeSharedApi(BitvavoSocketClientExchangeApi api)
            : base(
                  SharedTransport.Socket,
                  api,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities();
        }
    }
}
