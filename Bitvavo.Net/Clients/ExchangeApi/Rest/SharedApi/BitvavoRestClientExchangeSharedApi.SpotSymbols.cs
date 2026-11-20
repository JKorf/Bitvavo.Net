using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using Bitvavo.Net.Utils;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi
    {
        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);
        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false);

        async Task<IExchangeCallResult<SharedSpotSymbol[]>> IGetSpotSymbols.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
            => await GetSpotSymbolsAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            var symbols = result.Data.Select(ToSharedSymbolInfo).ToArray();
            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, symbols);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(symbols, request));
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            var error = await EnsureSymbolsCachedAsync().ConfigureAwait(false);
            if (error != null)
                return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, error);

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                return ExchangeCallResult<bool>.Fail(Exchange, ArgumentError.Invalid(nameof(symbol), "Only spot symbols are supported"));

            var error = await EnsureSymbolsCachedAsync().ConfigureAwait(false);
            if (error != null)
                return ExchangeCallResult<bool>.Fail(Exchange, error);

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            var error = await EnsureSymbolsCachedAsync().ConfigureAwait(false);
            if (error != null)
                return ExchangeCallResult<bool>.Fail(Exchange, error);

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }

        private async Task<Error?> EnsureSymbolsCachedAsync()
        {
            if (ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
                return null;

            var result = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
            return result.Error;
        }

        private SharedSymbol? ParseSymbol(string symbol)
            => BitvavoSharedApiMappings.ParseSymbol(_topicId, _api.EnvironmentName, symbol);

        private static SharedSpotSymbol ToSharedSymbolInfo(BitvavoSymbol symbol)
            => new SharedSpotSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.Symbol, symbol.Status == SymbolStatus.Trading)
            {
                MinTradeQuantity = symbol.MinOrderQuantity,
                MaxTradeQuantity = symbol.MaxOrderQuantity,
                MinNotionalValue = symbol.MinOrderQuoteQuantity,
                PriceStep = symbol.TickSize,
                QuantityDecimals = symbol.QuantityDecimals,
                DisplayName = symbol.Symbol,
                BaseAssetType = symbol.BaseAsset == "EUR" ? SharedAssetType.Fiat : SharedAssetType.Crypto,
                QuoteAssetType = symbol.QuoteAsset == "EUR" ? SharedAssetType.Fiat : SharedAssetType.Crypto,
                BaseAssetSubType = LibraryHelpers.IsStableCoin(symbol.BaseAsset) ? SharedAssetSubType.StableCoin : null,
                QuoteAssetSubType = LibraryHelpers.IsStableCoin(symbol.QuoteAsset) ? SharedAssetSubType.StableCoin : null
            };
    }
}
