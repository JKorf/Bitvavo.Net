using System.Globalization;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Clients.SpotApi;

/// <summary>
/// CryptoExchange.Net Shared-API implementation for the Bitvavo Spot REST client.
/// <para>
/// This is a facade-hosted partial on <see cref="BitvavoRestClientSpotApi"/> itself
/// (mirroring <c>KrakenRestClientSpotApi.Shared.cs</c>): the Shared interfaces are
/// implemented here and delegate to the typed <see cref="BitvavoRestClientSpotApi.ExchangeData"/>,
/// <see cref="BitvavoRestClientSpotApi.Account"/>, <see cref="BitvavoRestClientSpotApi.Trading"/>,
/// and <see cref="BitvavoRestClientSpotApi.Funding"/> sub-clients. The Shared layer is the
/// exchange-agnostic surface other CryptoExchange.Net consumers program against.
/// </para>
/// <para>
/// Bitvavo is spot-only, so every endpoint reports <see cref="TradingMode.Spot"/> as the
/// single supported trading mode. Symbol translation goes through
/// <see cref="BitvavoExchange.FormatSymbol(string, string, TradingMode, DateTime?)"/>
/// (uppercase <c>BASE-QUOTE</c>).
/// </para>
/// </summary>
internal partial class BitvavoRestClientSpotApi :
    IAssetsRestClient,
    IKlineRestClient,
    IRecentTradeRestClient,
    IOrderBookRestClient,
    ISpotSymbolRestClient,
    ISpotTickerRestClient,
    IBookTickerRestClient,
    IBalanceRestClient,
    ISpotOrderRestClient,
    ISpotOrderClientIdRestClient,
    IFeeRestClient,
    IDepositRestClient,
    IWithdrawalRestClient,
    IWithdrawRestClient,
    IBitvavoRestClientSpotApiShared
{
    private const string _exchangeName = "Bitvavo";
    private static readonly TradingMode[] _spotOnly = [TradingMode.Spot];

    /// <inheritdoc />
    public new string Exchange => BitvavoExchange.ExchangeName;

    /// <inheritdoc />
    public TradingMode[] SupportedTradingModes => _spotOnly;

    // ISharedClient.Authenticated is satisfied by the inherited RestApiClient.Authenticated.

    /// <inheritdoc />
    public void SetDefaultExchangeParameter(string name, object value)
        => ExchangeParameters.SetStaticParameter(Exchange, name, value);

    /// <inheritdoc />
    public void ResetDefaultExchangeParameters()
        => ExchangeParameters.ResetStaticParameters();

    /// <inheritdoc />
    public SharedClientInfo Discover() => SharedUtils.GetClientInfo(BitvavoExchange.Metadata, this);

    // â”€â”€ IAssetsRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetAssetOptions GetAssetOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    public async Task<HttpResult<SharedAsset>> GetAssetAsync(GetAssetRequest request, CancellationToken ct = default)
    {
        var validationError = GetAssetOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedAsset>(Exchange, validationError);

        var result = await ExchangeData.GetAssetsAsync(request.Asset, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedAsset>(result);

        var asset = result.Data!.SingleOrDefault();
        if (asset == null)
            return HttpResult.Fail<SharedAsset>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Asset not found")));

        return HttpResult.Ok(result, ToSharedAsset(asset));
    }

    /// <inheritdoc />
    public GetAssetsOptions GetAssetsOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    public async Task<HttpResult<SharedAsset[]>> GetAssetsAsync(GetAssetsRequest request, CancellationToken ct = default)
    {
        var validationError = GetAssetsOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

        var result = await ExchangeData.GetAssetsAsync(symbol: null, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedAsset[]>(result);

        return HttpResult.Ok(result, result.Data!.Select(ToSharedAsset).ToArray());
    }

    private static SharedAsset ToSharedAsset(BitvavoAsset asset) => new(asset.Symbol)
    {
        FullName = asset.Name,
        Networks = asset.Networks
            .Select(network => new SharedAssetNetwork(network)
            {
                WithdrawFee = asset.WithdrawalFee,
                MinWithdrawQuantity = asset.WithdrawalMinAmount,
                WithdrawEnabled = string.Equals(asset.WithdrawalStatus, "OK", StringComparison.OrdinalIgnoreCase),
                DepositEnabled = string.Equals(asset.DepositStatus, "OK", StringComparison.OrdinalIgnoreCase),
                MinConfirmations = asset.DepositConfirmations,
            })
            .ToArray(),
    };

    // â”€â”€ IKlineRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    private static readonly SharedKlineInterval[] _supportedKlineIntervals =
    [
        SharedKlineInterval.OneMinute,
        SharedKlineInterval.FiveMinutes,
        SharedKlineInterval.FifteenMinutes,
        SharedKlineInterval.ThirtyMinutes,
        SharedKlineInterval.OneHour,
        SharedKlineInterval.TwoHours,
        SharedKlineInterval.FourHours,
        SharedKlineInterval.SixHours,
        SharedKlineInterval.EightHours,
        SharedKlineInterval.TwelveHours,
        SharedKlineInterval.OneDay,
        SharedKlineInterval.OneWeek,
        SharedKlineInterval.OneMonth,
    ];

    /// <inheritdoc />
    public GetKlinesOptions GetKlinesOptions { get; } = new(_exchangeName,  true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1440,
        needsAuthentication: false,
        intervals: _supportedKlineIntervals)
    {
        MaxTotalDataPoints = 1440,
    };

    /// <inheritdoc />
    public async Task<HttpResult<SharedKline[]>> GetKlinesAsync(GetKlinesRequest request, PageRequest? nextPageToken = null, CancellationToken ct = default)
    {
        // ValidateRequest rejects intervals outside _supportedKlineIntervals (e.g.
        // SharedKlineInterval.ThreeMinutes â€” Bitvavo has no 3m bucket) before the
        // ToBitvavoKlineInterval mapper is reached, so the mapper only sees supported values.
        var validationError = GetKlinesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

        var interval = ToBitvavoKlineInterval(request.Interval);
        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetKlinesAsync(
            symbol,
            interval,
            request.Limit,
            request.StartTime,
            request.EndTime,
            ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedKline[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(kline => new SharedKline(
                request.Symbol,
                symbol,
                kline.OpenTime,
                kline.ClosePrice,
                kline.HighPrice,
                kline.LowPrice,
                kline.OpenPrice,
                kline.Volume))
            .ToArray());
    }

    // â”€â”€ IRecentTradeRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetRecentTradesOptions GetRecentTradesOptions { get; } = new(_exchangeName, 1000, false);

    /// <inheritdoc />
    public async Task<HttpResult<SharedTrade[]>> GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct = default)
    {
        var validationError = GetRecentTradesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetPublicTradesAsync(symbol, request.Limit, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedTrade[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(trade => new SharedTrade(
                request.Symbol,
                symbol,
                trade.Amount ?? 0m,
                trade.Price ?? 0m,
                trade.Timestamp)
            {
                Side = ToSharedOrderSide(trade.Side),
            })
            .ToArray());
    }

    // â”€â”€ IOrderBookRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetOrderBookOptions GetOrderBookOptions { get; } = new(_exchangeName, 1, 1000, false);

    /// <inheritdoc />
    public async Task<HttpResult<SharedOrderBook>> GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct = default)
    {
        var validationError = GetOrderBookOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetOrderBookAsync(symbol, request.Limit, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedOrderBook>(result);

        return HttpResult.Ok(result, new SharedOrderBook(
            result.Data!.Asks.Select(entry => (ISymbolOrderBookEntry)new BitvavoSharedOrderBookEntry { Price = entry.Price, Quantity = entry.Size }).ToArray(),
            result.Data!.Bids.Select(entry => (ISymbolOrderBookEntry)new BitvavoSharedOrderBookEntry { Price = entry.Price, Quantity = entry.Size }).ToArray()));
    }

    // â”€â”€ ISpotSymbolRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct = default)
    {
        var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

        var result = await ExchangeData.GetMarketsAsync(ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotSymbol[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(market => new SharedSpotSymbol(
                market.BaseAsset,
                market.QuoteAsset,
                market.Market,
                string.Equals(market.Status, "trading", StringComparison.OrdinalIgnoreCase))
            {
                MinTradeQuantity = ParseDecimal(market.MinOrderInBaseAsset),
                MaxTradeQuantity = ParseDecimal(market.MaxOrderInBaseAsset),
                MinNotionalValue = ParseDecimal(market.MinOrderInQuoteAsset),
                PriceSignificantFigures = ParseInt(market.PricePrecision),
            })
            .ToArray());
    }

    /// <inheritdoc />
    public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
    {
        var result = await GetSpotSymbolsAsync(new GetSymbolsRequest(TradingMode.Spot)).ConfigureAwait(false);
        if (!result.Success)
            return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, result.Error!);

        var matches = result.Data!
            .Where(symbol => string.Equals(symbol.BaseAsset, baseAsset, StringComparison.OrdinalIgnoreCase))
            .Select(symbol => symbol.SharedSymbol)
            .ToArray();
        return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, matches);
    }

    /// <inheritdoc />
    public Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        => SupportsSpotSymbolAsync(symbol.GetSymbol(FormatSymbol));

    /// <inheritdoc />
    public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
    {
        var result = await GetSpotSymbolsAsync(new GetSymbolsRequest(TradingMode.Spot)).ConfigureAwait(false);
        if (!result.Success)
            return ExchangeCallResult<bool>.Fail(Exchange, result.Error!);

        var supported = result.Data!.Any(symbol => string.Equals(symbol.Name, symbolName, StringComparison.OrdinalIgnoreCase));
        return ExchangeCallResult<bool>.Ok(Exchange, supported);
    }

    // â”€â”€ ISpotTickerRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetSpotTickerOptions GetSpotTickerOptions { get; } = new(_exchangeName, SharedTickerType.Day24H);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct = default)
    {
        var validationError = GetSpotTickerOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetTicker24hAsync(symbol, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotTicker>(result);

        var ticker = result.Data!.SingleOrDefault();
        if (ticker == null)
            return HttpResult.Fail<SharedSpotTicker>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Ticker not found")));

        return HttpResult.Ok(result, ToSharedSpotTicker(request.Symbol, ticker));
    }

    /// <inheritdoc />
    public GetSpotTickersOptions GetSpotTickersOptions { get; } = new(_exchangeName, SharedTickerType.Day24H);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotTicker[]>> GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct = default)
    {
        var validationError = GetSpotTickersOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

        var result = await ExchangeData.GetTicker24hAsync(market: null, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotTicker[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(ticker => ToSharedSpotTicker(ParseSharedSymbol(ticker.Market), ticker))
            .ToArray());
    }

    private static SharedSpotTicker ToSharedSpotTicker(SharedSymbol sharedSymbol, BitvavoTicker24h ticker)
    {
        var changePercentage = ticker.Open is > 0m && ticker.Last != null
            ? Math.Round((ticker.Last.Value - ticker.Open.Value) / ticker.Open.Value * 100m, 6)
            : (decimal?)null;

        return new SharedSpotTicker(
            sharedSymbol,
            ticker.Market,
            ticker.Last,
            ticker.High,
            ticker.Low,
            ticker.Volume ?? 0m,
            changePercentage)
        {
            QuoteVolume = ticker.VolumeQuote,
        };
    }

    // â”€â”€ IBookTickerRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetBookTickerOptions GetBookTickerOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    public async Task<HttpResult<SharedBookTicker>> GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct = default)
    {
        var validationError = GetBookTickerOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetTickerBookAsync(symbol, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedBookTicker>(result);

        var book = result.Data!.SingleOrDefault();
        if (book == null)
            return HttpResult.Fail<SharedBookTicker>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Book ticker not found")));

        return HttpResult.Ok(result, new SharedBookTicker(
            request.Symbol,
            book.Market,
            book.Ask ?? 0m,
            book.AskSize ?? 0m,
            book.Bid ?? 0m,
            book.BidSize ?? 0m));
    }

    // â”€â”€ IBalanceRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetBalancesOptions GetBalancesOptions { get; } = new(_exchangeName, [AccountTypeFilter.Spot]);

    /// <inheritdoc />
    public async Task<HttpResult<SharedBalance[]>> GetBalancesAsync(GetBalancesRequest request, CancellationToken ct = default)
    {
        var validationError = GetBalancesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

        var result = await Account.GetBalancesAsync(symbol: null, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedBalance[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(balance => new SharedBalance(
                TradingMode.Spot,
                balance.Symbol,
                balance.Available ?? 0m,
                (balance.Available ?? 0m) + (balance.InOrder ?? 0m)))
            .ToArray());
    }

    // â”€â”€ ISpotOrderRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;

    /// <inheritdoc />
    public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;

    /// <inheritdoc />
    public SharedOrderType[] SpotSupportedOrderTypes { get; } = [SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker];

    /// <inheritdoc />
    public SharedTimeInForce[] SpotSupportedTimeInForce { get; } =
        [SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill];

    /// <inheritdoc />
    public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new(
        SharedQuantityType.BaseAsset,
        SharedQuantityType.BaseAsset,
        SharedQuantityType.BaseAndQuoteAsset,
        SharedQuantityType.BaseAndQuoteAsset);

    /// <inheritdoc />
    public string GenerateClientOrderId() => Guid.NewGuid().ToString();

    /// <inheritdoc />
    public PlaceSpotOrderOptions PlaceSpotOrderOptions { get; } = new(_exchangeName)
    {
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                nameof(BitvavoPlaceOrderRequest.OperatorId),
                typeof(long),
                "Account-scoped integer identifying the originator of the order request",
                1L),
        ],
    };

    /// <inheritdoc />
    public async Task<HttpResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct = default)
    {
        var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var operatorId = ExchangeParameters.GetValue<long?>(request.ExchangeParameters, Exchange, nameof(BitvavoPlaceOrderRequest.OperatorId))
            ?? throw new ArgumentException($"{nameof(BitvavoPlaceOrderRequest.OperatorId)} exchange parameter is required for Bitvavo orders", nameof(request));

        var placeRequest = new BitvavoPlaceOrderRequest(
            request.Symbol!.GetSymbol(FormatSymbol),
            ToBitvavoOrderSide(request.Side),
            ToBitvavoOrderType(request.OrderType),
            operatorId,
            Amount: request.Quantity?.QuantityInBaseAsset,
            AmountQuote: request.Quantity?.QuantityInQuoteAsset,
            Price: request.Price,
            TimeInForce: request.TimeInForce == null ? null : ToBitvavoTimeInForce(request.TimeInForce.Value),
            PostOnly: request.OrderType == SharedOrderType.LimitMaker ? true : null,
            ClientOrderId: request.ClientOrderId);

        var result = await Trading.PlaceOrderAsync(placeRequest, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        return HttpResult.Ok(result, new SharedId(result.Data!.OrderId));
    }

    /// <inheritdoc />
    public GetSpotOrderOptions GetSpotOrderOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct = default)
    {
        var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetOrderAsync(symbol, orderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder>(result);

        return HttpResult.Ok(result, ToSharedSpotOrder(request.Symbol, result.Data!));
    }

    /// <inheritdoc />
    public GetOpenSpotOrdersOptions GetOpenSpotOrdersOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotOrder[]>> GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct = default)
    {
        var validationError = GetOpenSpotOrdersOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

        var symbol = request.Symbol?.GetSymbol(FormatSymbol);
        var result = await Trading.GetOpenOrdersAsync(market: symbol, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(order => ToSharedSpotOrder(ParseSharedSymbol(order.Market), order))
            .ToArray());
    }

    /// <inheritdoc />
    public GetSpotClosedOrdersOptions GetClosedSpotOrdersOptions { get; } = new(
        _exchangeName,
        supportsAscending: true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotOrder[]>> GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? nextPageToken = null, CancellationToken ct = default)
    {
        var validationError = GetClosedSpotOrdersOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetOrderHistoryAsync(
            symbol,
            request.Limit,
            request.StartTime,
            request.EndTime,
            ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder[]>(result);

        // Filter to terminal states â€” Bitvavo's /v2/orders returns the full history,
        // the Shared "closed orders" contract is for non-open orders only.
        var closed = result.Data!
            .Where(order => ToSharedOrderStatus(order.Status) != SharedOrderStatus.Open)
            .Select(order => ToSharedSpotOrder(request.Symbol, order))
            .ToArray();
        return HttpResult.Ok(result, closed);
    }

    /// <inheritdoc />
    public GetSpotOrderTradesOptions GetSpotOrderTradesOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    public async Task<HttpResult<SharedUserTrade[]>> GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct = default)
    {
        var validationError = GetSpotOrderTradesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetUserTradesAsync(symbol, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedUserTrade[]>(result);

        var trades = result.Data!
            .Where(fill => fill.OrderId == request.OrderId)
            .Select(fill => ToSharedUserTrade(request.Symbol, symbol, fill))
            .ToArray();
        return HttpResult.Ok(result, trades);
    }

    /// <inheritdoc />
    public GetSpotUserTradesOptions GetSpotUserTradesOptions { get; } = new(
        _exchangeName,
        supportsAscending: true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    public async Task<HttpResult<SharedUserTrade[]>> GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? nextPageToken = null, CancellationToken ct = default)
    {
        var validationError = GetSpotUserTradesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetUserTradesAsync(
            symbol,
            request.Limit,
            request.StartTime,
            request.EndTime,
            ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedUserTrade[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(fill => ToSharedUserTrade(request.Symbol, symbol, fill))
            .ToArray());
    }

    /// <inheritdoc />
    public CancelSpotOrderOptions CancelSpotOrderOptions { get; } = new(_exchangeName, true)
    {
        EndpointName = "CancelOrderRequest",
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                nameof(BitvavoPlaceOrderRequest.OperatorId),
                typeof(long),
                "Account-scoped integer identifying the originator of the cancel request",
                1L),
        ],
    };

    /// <inheritdoc />
    public async Task<HttpResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct = default)
    {
        var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var operatorId = ExchangeParameters.GetValue<long?>(request.ExchangeParameters, Exchange, nameof(BitvavoPlaceOrderRequest.OperatorId))
            ?? throw new ArgumentException($"{nameof(BitvavoPlaceOrderRequest.OperatorId)} exchange parameter is required for Bitvavo order cancellation", nameof(request));

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.CancelOrderAsync(symbol, operatorId, orderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        return HttpResult.Ok(result, new SharedId(result.Data!.OrderId));
    }

    // â”€â”€ ISpotOrderClientIdRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetSpotOrderByClientOrderIdOptions GetSpotOrderByClientOrderIdOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct = default)
    {
        var validationError = GetSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetOrderAsync(symbol, clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder>(result);

        return HttpResult.Ok(result, ToSharedSpotOrder(request.Symbol, result.Data!));
    }

    /// <inheritdoc />
    public CancelSpotOrderByClientOrderIdOptions CancelSpotOrderByClientOrderIdOptions { get; } = new(_exchangeName, true)
    {
        EndpointName = "CancelOrderByClientOrderIdRequest",
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                nameof(BitvavoPlaceOrderRequest.OperatorId),
                typeof(long),
                "Account-scoped integer identifying the originator of the cancel request",
                1L),
        ],
    };

    /// <inheritdoc />
    public async Task<HttpResult<SharedId>> CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct = default)
    {
        var validationError = CancelSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var operatorId = ExchangeParameters.GetValue<long?>(request.ExchangeParameters, Exchange, nameof(BitvavoPlaceOrderRequest.OperatorId))
            ?? throw new ArgumentException($"{nameof(BitvavoPlaceOrderRequest.OperatorId)} exchange parameter is required for Bitvavo order cancellation", nameof(request));

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.CancelOrderAsync(symbol, operatorId, clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        return HttpResult.Ok(result, new SharedId(result.Data!.OrderId));
    }

    private SharedSpotOrder ToSharedSpotOrder(SharedSymbol sharedSymbol, BitvavoOrder order)
    {
        return new SharedSpotOrder(
            sharedSymbol,
            order.Market,
            order.OrderId,
            ToSharedOrderType(order.OrderType),
            ToSharedOrderSide(order.Side),
            ToSharedOrderStatus(order.Status),
            order.Created)
        {
            ClientOrderId = order.ClientOrderId,
            OrderPrice = order.Price,
            OrderQuantity = new SharedOrderQuantity(order.Amount, order.AmountQuote),
            QuantityFilled = new SharedOrderQuantity(order.FilledAmount, order.FilledAmountQuote),
            AveragePrice = order.FilledAmount is > 0m && order.FilledAmountQuote != null
                ? order.FilledAmountQuote / order.FilledAmount
                : null,
            TimeInForce = order.TimeInForce == null ? null : ToSharedTimeInForce(order.TimeInForce.Value),
            UpdateTime = order.Updated,
            Fee = order.FeePaid,
            FeeAsset = order.FeeCurrency,
            TriggerPrice = order.TriggerAmount,
            IsTriggerOrder = order.TriggerAmount != null,
        };
    }

    private static SharedUserTrade ToSharedUserTrade(SharedSymbol sharedSymbol, string symbol, BitvavoFill fill)
    {
        return new SharedUserTrade(
            sharedSymbol,
            symbol,
            fill.OrderId ?? string.Empty,
            fill.Id,
            ToSharedOrderSide(fill.Side),
            fill.Amount ?? 0m,
            fill.Price ?? 0m,
            fill.Timestamp)
        {
            ClientOrderId = fill.ClientOrderId,
            Fee = fill.Fee,
            FeeAsset = fill.FeeCurrency,
            Role = fill.Taker switch
            {
                true => SharedRole.Taker,
                false => SharedRole.Maker,
                null => null,
            },
        };
    }

    // â”€â”€ IFeeRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetFeeOptions GetFeeOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    public async Task<HttpResult<SharedFee>> GetFeesAsync(GetFeeRequest request, CancellationToken ct = default)
    {
        var validationError = GetFeeOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedFee>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Account.GetTradingFeesAsync(symbol, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedFee>(result);

        return HttpResult.Ok(result, new SharedFee(
            (result.Data!.Maker ?? 0m) * 100m,
            (result.Data!.Taker ?? 0m) * 100m));
    }

    // â”€â”€ IDepositRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetDepositAddressesOptions GetDepositAddressesOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    public async Task<HttpResult<SharedDepositAddress[]>> GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct = default)
    {
        var validationError = GetDepositAddressesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

        var result = await Funding.GetDepositAddressAsync(request.Asset, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedDepositAddress[]>(result);

        if (result.Data!.Address == null)
            return HttpResult.Ok(result, Array.Empty<SharedDepositAddress>());

        SharedDepositAddress[] addresses =
        [
            new SharedDepositAddress(request.Asset, result.Data!.Address)
            {
                Network = request.Network,
                TagOrMemo = result.Data!.PaymentReference,
            },
        ];
        return HttpResult.Ok(result, addresses);
    }

    /// <inheritdoc />
    public GetDepositsOptions GetDepositsOptions { get; } = new(_exchangeName,  true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    public async Task<HttpResult<SharedDeposit[]>> GetDepositsAsync(GetDepositsRequest request, PageRequest? nextPageToken = null, CancellationToken ct = default)
    {
        var validationError = GetDepositsOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

        var result = await Funding.GetDepositHistoryAsync(
            request.Asset,
            request.Limit,
            request.StartTime,
            request.EndTime,
            ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedDeposit[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(deposit =>
            {
                var completed = string.Equals(deposit.Status, "completed", StringComparison.OrdinalIgnoreCase);
                return new SharedDeposit(
                    deposit.Symbol,
                    deposit.Amount ?? 0m,
                    completed,
                    deposit.Timestamp,
                    ToSharedTransferStatus(deposit.Status))
                {
                    TransactionId = deposit.TxId,
                    Tag = deposit.PaymentReference,
                };
            })
            .ToArray());
    }

    // â”€â”€ IWithdrawalRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public GetWithdrawalsOptions GetWithdrawalsOptions { get; } = new(_exchangeName,  true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? nextPageToken = null, CancellationToken ct = default)
    {
        var validationError = GetWithdrawalsOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

        var result = await Funding.GetWithdrawalHistoryAsync(
            request.Asset,
            request.Limit,
            request.StartTime,
            request.EndTime,
            ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedWithdrawal[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(withdrawal => new SharedWithdrawal(
                withdrawal.Symbol,
                withdrawal.Address ?? string.Empty,
                withdrawal.Amount ?? 0m,
                string.Equals(withdrawal.Status, "completed", StringComparison.OrdinalIgnoreCase),
                withdrawal.Timestamp,
                ToSharedTransferStatus(withdrawal.Status))
            {
                TransactionId = withdrawal.TxId,
                Tag = withdrawal.PaymentReference,
                Fee = withdrawal.Fee,
            })
            .ToArray());
    }

    // â”€â”€ IWithdrawRestClient â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <inheritdoc />
    public WithdrawOptions WithdrawOptions { get; } = new(_exchangeName);

    /// <inheritdoc />
    public async Task<HttpResult<SharedId>> WithdrawAsync(WithdrawRequest request, CancellationToken ct = default)
    {
        var validationError = WithdrawOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var withdrawRequest = new BitvavoWithdrawRequest(
            request.Asset,
            request.Quantity,
            request.Address,
            PaymentId: request.AddressTag);

        var result = await Funding.WithdrawAsync(withdrawRequest, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        // Bitvavo's withdrawal endpoint returns no id â€” echo the requested asset symbol
        // so the Shared contract's non-null SharedId is satisfied.
        return HttpResult.Ok(result, new SharedId(result.Data!.Symbol));
    }

    // â”€â”€ Shared helpers â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    /// <summary>
    /// Parse a Bitvavo wire symbol (<c>BASE-QUOTE</c>) back into a <see cref="SharedSymbol"/>.
    /// Used when the typed DTO carries only the joined market string (ticker / order list
    /// responses) and the Shared model needs the split base / quote.
    /// </summary>
    private static SharedSymbol ParseSharedSymbol(string market)
    {
        var dash = market.IndexOf('-');
        return dash > 0
            ? new SharedSymbol(TradingMode.Spot, market[..dash], market[(dash + 1)..], market)
            : new SharedSymbol(TradingMode.Spot, market, string.Empty, market);
    }

    /// <summary>
    /// Map a CryptoExchange.Net <see cref="SharedKlineInterval"/> onto Bitvavo's
    /// <see cref="KlineInterval"/>. The two enums do not share underlying integer values
    /// (<see cref="SharedKlineInterval"/> is interval-seconds, <see cref="KlineInterval"/>
    /// is ordinal) so an explicit map is mandatory â€” a direct cast would silently corrupt.
    /// <see cref="SharedKlineInterval.ThreeMinutes"/> has no Bitvavo equivalent; callers are
    /// guarded by <see cref="GetKlinesOptions"/> validation before this mapper runs.
    /// </summary>
    private static KlineInterval ToBitvavoKlineInterval(SharedKlineInterval interval) => interval switch
    {
        SharedKlineInterval.OneMinute => KlineInterval.OneMinute,
        SharedKlineInterval.FiveMinutes => KlineInterval.FiveMinutes,
        SharedKlineInterval.FifteenMinutes => KlineInterval.FifteenMinutes,
        SharedKlineInterval.ThirtyMinutes => KlineInterval.ThirtyMinutes,
        SharedKlineInterval.OneHour => KlineInterval.OneHour,
        SharedKlineInterval.TwoHours => KlineInterval.TwoHours,
        SharedKlineInterval.FourHours => KlineInterval.FourHours,
        SharedKlineInterval.SixHours => KlineInterval.SixHours,
        SharedKlineInterval.EightHours => KlineInterval.EightHours,
        SharedKlineInterval.TwelveHours => KlineInterval.TwelveHours,
        SharedKlineInterval.OneDay => KlineInterval.OneDay,
        SharedKlineInterval.OneWeek => KlineInterval.OneWeek,
        SharedKlineInterval.OneMonth => KlineInterval.OneMonth,
        _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, "Interval not supported by Bitvavo"),
    };

    private static decimal? ParseDecimal(string value)
        => decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;

    private static int? ParseInt(string value)
        => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;

    private static SharedOrderSide ToSharedOrderSide(OrderSide side) => side switch
    {
        OrderSide.Buy => SharedOrderSide.Buy,
        OrderSide.Sell => SharedOrderSide.Sell,
        _ => throw new ArgumentOutOfRangeException(nameof(side), side, "Unmapped Bitvavo OrderSide"),
    };

    private static SharedOrderSide? ToSharedOrderSide(OrderSide? side) => side == null ? null : ToSharedOrderSide(side.Value);

    private static OrderSide ToBitvavoOrderSide(SharedOrderSide side) => side switch
    {
        SharedOrderSide.Buy => OrderSide.Buy,
        SharedOrderSide.Sell => OrderSide.Sell,
        _ => throw new ArgumentOutOfRangeException(nameof(side), side, "Unmapped SharedOrderSide"),
    };

    private static SharedOrderType ToSharedOrderType(OrderType type) => type switch
    {
        OrderType.Market => SharedOrderType.Market,
        OrderType.Limit => SharedOrderType.Limit,
        OrderType.StopLoss => SharedOrderType.Other,
        OrderType.StopLossLimit => SharedOrderType.Other,
        OrderType.TakeProfit => SharedOrderType.Other,
        OrderType.TakeProfitLimit => SharedOrderType.Other,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unmapped Bitvavo OrderType"),
    };

    private static OrderType ToBitvavoOrderType(SharedOrderType type) => type switch
    {
        SharedOrderType.Market => OrderType.Market,
        SharedOrderType.Limit => OrderType.Limit,
        SharedOrderType.LimitMaker => OrderType.Limit,
        SharedOrderType.Other => throw new ArgumentException("SharedOrderType.Other cannot be placed on Bitvavo â€” use a concrete order type", nameof(type)),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unmapped SharedOrderType"),
    };

    private static SharedOrderStatus ToSharedOrderStatus(OrderStatus status) => status switch
    {
        OrderStatus.New => SharedOrderStatus.Open,
        OrderStatus.AwaitingTrigger => SharedOrderStatus.Open,
        OrderStatus.PartiallyFilled => SharedOrderStatus.Open,
        OrderStatus.Filled => SharedOrderStatus.Filled,
        OrderStatus.Canceled => SharedOrderStatus.Canceled,
        OrderStatus.CanceledAuction => SharedOrderStatus.Canceled,
        OrderStatus.CanceledSelfTradePrevention => SharedOrderStatus.Canceled,
        OrderStatus.CanceledIoc => SharedOrderStatus.Canceled,
        OrderStatus.CanceledFok => SharedOrderStatus.Canceled,
        OrderStatus.CanceledMarketProtection => SharedOrderStatus.Canceled,
        OrderStatus.CanceledPostOnly => SharedOrderStatus.Canceled,
        OrderStatus.Expired => SharedOrderStatus.Canceled,
        OrderStatus.Rejected => SharedOrderStatus.Canceled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unmapped Bitvavo OrderStatus"),
    };

    private static SharedTimeInForce ToSharedTimeInForce(TimeInForce timeInForce) => timeInForce switch
    {
        TimeInForce.GoodTillCanceled => SharedTimeInForce.GoodTillCanceled,
        TimeInForce.ImmediateOrCancel => SharedTimeInForce.ImmediateOrCancel,
        TimeInForce.FillOrKill => SharedTimeInForce.FillOrKill,
        _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "Unmapped Bitvavo TimeInForce"),
    };

    private static TimeInForce ToBitvavoTimeInForce(SharedTimeInForce timeInForce) => timeInForce switch
    {
        SharedTimeInForce.GoodTillCanceled => TimeInForce.GoodTillCanceled,
        SharedTimeInForce.ImmediateOrCancel => TimeInForce.ImmediateOrCancel,
        SharedTimeInForce.FillOrKill => TimeInForce.FillOrKill,
        _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "Unmapped SharedTimeInForce"),
    };

    private static SharedTransferStatus ToSharedTransferStatus(string status) => status.ToLowerInvariant() switch
    {
        "completed" => SharedTransferStatus.Completed,
        "awaiting_processing" => SharedTransferStatus.InProgress,
        "awaiting_email_confirmation" => SharedTransferStatus.InProgress,
        "awaiting_bitvavo_inspection" => SharedTransferStatus.InProgress,
        "in_mempool" => SharedTransferStatus.InProgress,
        "processing" => SharedTransferStatus.InProgress,
        "canceled" => SharedTransferStatus.Failed,
        "rejected" => SharedTransferStatus.Failed,
        _ => SharedTransferStatus.Unknown,
    };
}
