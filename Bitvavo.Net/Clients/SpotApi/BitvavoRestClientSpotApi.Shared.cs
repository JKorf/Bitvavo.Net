using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System.Globalization;

namespace Bitvavo.Net.Clients.SpotApi;

internal partial class BitvavoRestClientSpotApi : IBitvavoRestClientSpotApiShared
{
    private const string _exchangeName = "Bitvavo";
    private const string _topicId = "Bitvavo";

    #region Shared client

    /// <inheritdoc />
    string ISharedClient.Exchange => BitvavoExchange.ExchangeName;

    /// <inheritdoc />
    TradingMode[] ISharedClient.SupportedTradingModes => [TradingMode.Spot];

    /// <inheritdoc />
    void ISharedClient.SetDefaultExchangeParameter(string name, object value)
        => ExchangeParameters.SetStaticParameter(BitvavoExchange.ExchangeName, name, value);

    /// <inheritdoc />
    void ISharedClient.ResetDefaultExchangeParameters()
        => ExchangeParameters.ResetStaticParameters();

    /// <inheritdoc />
    SharedClientInfo ISharedClient.Discover() => SharedUtils.GetClientInfo(BitvavoExchange.Metadata, this);

    #endregion

    #region Assets client

    /// <inheritdoc />
    GetAssetOptions IAssetsRestClient.GetAssetOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    async Task<HttpResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
    {
        var validationError = ((IAssetsRestClient)this).GetAssetOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedAsset>(BitvavoExchange.ExchangeName, validationError);

        var result = await ExchangeData.GetAssetsAsync(request.Asset, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedAsset>(result);

        var asset = result.Data!.SingleOrDefault();
        if (asset == null)
            return HttpResult.Fail<SharedAsset>(BitvavoExchange.ExchangeName, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Asset not found")));

        return HttpResult.Ok(result, ToSharedAsset(asset));
    }

    /// <inheritdoc />
    GetAssetsOptions IAssetsRestClient.GetAssetsOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    async Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
    {
        var validationError = ((IAssetsRestClient)this).GetAssetsOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedAsset[]>(BitvavoExchange.ExchangeName, validationError);

        var result = await ExchangeData.GetAssetsAsync(asset: null, ct).ConfigureAwait(false);
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

    #endregion

    #region Kline client

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
    GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new(_exchangeName,  true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1440,
        needsAuthentication: false,
        intervals: _supportedKlineIntervals)
    {
        MaxTotalDataPoints = 1440,
    };

    /// <inheritdoc />
    async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? nextPageToken, CancellationToken ct)
    {
        var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

        var interval = (KlineInterval)request.Interval;
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

    #endregion

    #region Recent Trade client

    /// <inheritdoc />
    GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new(_exchangeName, 1000, false);

    /// <inheritdoc />
    async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
    {
        var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetTradeHistoryAsync(symbol, request.Limit, ct: ct).ConfigureAwait(false);
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

    #endregion

    #region Order Book client

    /// <inheritdoc />
    GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new(_exchangeName, 1, 1000, false);

    /// <inheritdoc />
    async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
    {
        var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetOrderBookAsync(symbol, request.Limit, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedOrderBook>(result);

        return HttpResult.Ok(result, new SharedOrderBook(
            result.Data!.Asks,
            result.Data!.Bids));
    }

    #endregion

    #region Spot Symbol client

    /// <inheritdoc />
    GetSpotSymbolsOptions ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

        var result = await ExchangeData.GetSymbolsAsync(ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotSymbol[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(market => new SharedSpotSymbol(
                market.BaseAsset,
                market.QuoteAsset,
                market.Symbol,
                string.Equals(market.Status, "trading", StringComparison.OrdinalIgnoreCase))
            {
                MinTradeQuantity = market.MinOrderInBaseAsset,
                MaxTradeQuantity = market.MaxOrderInBaseAsset,
                MinNotionalValue = market.MinOrderInQuoteAsset,
                PriceSignificantFigures = market.PricePrecision,
            })
            .ToArray());
    }

    /// <inheritdoc />
    async Task<ExchangeCallResult<SharedSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsForBaseAssetAsync(string baseAsset)
    {
        if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
        {
            var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
            if (!symbols.Success)
                return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
        }

        return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, EnvironmentName, null, baseAsset));
    }

    async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(SharedSymbol symbol)
    {
        if (symbol.TradingMode != TradingMode.Spot)
            throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

        if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
        {
            var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
            if (!symbols.Success)
                return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
        }

        return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbol));
    }

    async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(string symbolName)
    {
        if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
        {
            var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
            if (!symbols.Success)
                return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
        }

        return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbolName));
    }

    #endregion

    #region Spot Ticker client

    /// <inheritdoc />
    GetSpotTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new(_exchangeName, SharedTickerType.Day24H);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetTickersAsync(symbol, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotTicker>(result);

        var ticker = result.Data!.SingleOrDefault();
        if (ticker == null)
            return HttpResult.Fail<SharedSpotTicker>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Ticker not found")));

        return HttpResult.Ok(result, ToSharedSpotTicker(request.Symbol, ticker));
    }

    /// <inheritdoc />
    GetSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new(_exchangeName, SharedTickerType.Day24H);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

        var result = await ExchangeData.GetTickersAsync(symbol: null, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotTicker[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(ticker => ToSharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, ticker.Symbol), ticker))
            .ToArray());
    }

    private static SharedSpotTicker ToSharedSpotTicker(SharedSymbol? sharedSymbol, BitvavoTicker24h ticker)
    {
        var changePercentage = ticker.Open is > 0m && ticker.Last != null
            ? Math.Round((ticker.Last.Value - ticker.Open.Value) / ticker.Open.Value * 100m, 6)
            : (decimal?)null;

        return new SharedSpotTicker(
            sharedSymbol,
            ticker.Symbol,
            ticker.Last,
            ticker.High,
            ticker.Low,
            ticker.Volume ?? 0m,
            changePercentage)
        {
            QuoteVolume = ticker.VolumeQuote,
        };
    }

    #endregion

    #region Book Ticker client

    /// <inheritdoc />
    GetBookTickerOptions IBookTickerRestClient.GetBookTickerOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    async Task<HttpResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
    {
        var validationError = ((IBookTickerRestClient)this).GetBookTickerOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await ExchangeData.GetBookTickerAsync(symbol, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedBookTicker>(result);

        var book = result.Data!.SingleOrDefault();
        if (book == null)
            return HttpResult.Fail<SharedBookTicker>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Book ticker not found")));

        return HttpResult.Ok(result, new SharedBookTicker(
            request.Symbol,
            book.Symbol,
            book.Ask ?? 0m,
            book.AskSize ?? 0m,
            book.Bid ?? 0m,
            book.BidSize ?? 0m));
    }

    #endregion

    #region Balance client

    /// <inheritdoc />
    GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new(_exchangeName, [AccountTypeFilter.Spot]);

    /// <inheritdoc />
    async Task<HttpResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
    {
        var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

        var result = await Account.GetBalancesAsync(asset: null, ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedBalance[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(balance => new SharedBalance(
                TradingMode.Spot,
                balance.Asset,
                balance.Available ?? 0m,
                (balance.Available ?? 0m) + (balance.InOrder ?? 0m)))
            .ToArray());
    }

    #endregion

    #region Spot Order client

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
    string ISpotOrderRestClient.GenerateClientOrderId() => Guid.NewGuid().ToString();

    /// <inheritdoc />
    PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new(_exchangeName)
    {
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                "OperatorId",
                typeof(long),
                "Account-scoped integer identifying the originator of the order request",
                1L),
        ],
    };

    /// <inheritdoc />
    async Task<HttpResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).PlaceSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var operatorId = request.GetParamValue<long>("OperatorId");
        var result = await Trading.PlaceOrderAsync(
            request.Symbol!.GetSymbol(FormatSymbol),
            ToBitvavoOrderSide(request.Side),
            ToBitvavoOrderType(request.OrderType),
            operatorId,
            amount: request.Quantity?.QuantityInBaseAsset,
            amountQuote: request.Quantity?.QuantityInQuoteAsset,
            price: request.Price,
            timeInForce: request.TimeInForce == null ? null : ToBitvavoTimeInForce(request.TimeInForce.Value),
            postOnly: request.OrderType == SharedOrderType.LimitMaker ? true : null,
            clientOrderId: request.ClientOrderId,
            ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        return HttpResult.Ok(result, new SharedId(result.Data!.OrderId));
    }

    /// <inheritdoc />
    GetSpotOrderOptions ISpotOrderRestClient.GetSpotOrderOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetOrderAsync(symbol, orderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder>(result);

        return HttpResult.Ok(result, ToSharedSpotOrder(request.Symbol, result.Data!));
    }

    /// <inheritdoc />
    GetOpenSpotOrdersOptions ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).GetOpenSpotOrdersOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

        var symbol = request.Symbol?.GetSymbol(FormatSymbol);
        var result = await Trading.GetOpenOrdersAsync(symbol: symbol, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder[]>(result);

        return HttpResult.Ok(result, result.Data!
            .Select(order => ToSharedSpotOrder(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, order.Market)!, order))
            .ToArray());
    }

    /// <inheritdoc />
    GetSpotClosedOrdersOptions ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new(
        _exchangeName,
        supportsAscending: true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? nextPageToken, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).GetClosedSpotOrdersOptions.ValidateRequest(request, this);
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
    GetSpotOrderTradesOptions ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).GetSpotOrderTradesOptions.ValidateRequest(request, this);
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
    GetSpotUserTradesOptions ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new(
        _exchangeName,
        supportsAscending: true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? nextPageToken, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).GetSpotUserTradesOptions.ValidateRequest(request, this);
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
    CancelSpotOrderOptions ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new(_exchangeName, true)
    {
        EndpointName = "CancelOrderRequest",
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                "OperatorId",
                typeof(long),
                "Account-scoped integer identifying the originator of the cancel request",
                1L),
        ],
    };

    /// <inheritdoc />
    async Task<HttpResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var operatorId = ExchangeParameters.GetValue<long?>(request.ExchangeParameters, Exchange, "OperatorId")
            ?? throw new ArgumentException("OperatorId exchange parameter is required for Bitvavo order cancellation", nameof(request));

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.CancelOrderAsync(symbol, operatorId, orderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        return HttpResult.Ok(result, new SharedId(result.Data!.OrderId));
    }

    #endregion

    #region Spot Order Client Id client

    /// <inheritdoc />
    GetSpotOrderByClientOrderIdOptions ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    async Task<HttpResult<SharedSpotOrder>> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderClientIdRestClient)this).GetSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

        var symbol = request.Symbol!.GetSymbol(FormatSymbol);
        var result = await Trading.GetOrderAsync(symbol, clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedSpotOrder>(result);

        return HttpResult.Ok(result, ToSharedSpotOrder(request.Symbol, result.Data!));
    }

    /// <inheritdoc />
    CancelSpotOrderByClientOrderIdOptions ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdOptions { get; } = new(_exchangeName, true)
    {
        EndpointName = "CancelOrderByClientOrderIdRequest",
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                "OperatorId",
                typeof(long),
                "Account-scoped integer identifying the originator of the cancel request",
                1L),
        ],
    };

    /// <inheritdoc />
    async Task<HttpResult<SharedId>> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
    {
        var validationError = ((ISpotOrderClientIdRestClient)this).CancelSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var operatorId = ExchangeParameters.GetValue<long?>(request.ExchangeParameters, Exchange, "OperatorId")
            ?? throw new ArgumentException("OperatorId exchange parameter is required for Bitvavo order cancellation", nameof(request));

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

    #endregion

    #region Fee client

    /// <inheritdoc />
    GetFeeOptions IFeeRestClient.GetFeeOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    async Task<HttpResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
    {
        var validationError = ((IFeeRestClient)this).GetFeeOptions.ValidateRequest(request, this);
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

    #endregion

    #region Deposit client

    /// <inheritdoc />
    GetDepositAddressesOptions IDepositRestClient.GetDepositAddressesOptions { get; } = new(_exchangeName, true);

    /// <inheritdoc />
    async Task<HttpResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
    {
        var validationError = ((IDepositRestClient)this).GetDepositAddressesOptions.ValidateRequest(request, this);
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
    GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new(_exchangeName,  true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    async Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? nextPageToken, CancellationToken ct)
    {
        var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(request, this);
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

    #endregion

    #region Withdrawal client

    /// <inheritdoc />
    GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new(_exchangeName,  true,
        supportsDescending: true,
        timeFilterSupported: true,
        maxLimit: 1000);

    /// <inheritdoc />
    async Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? nextPageToken, CancellationToken ct)
    {
        var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(request, this);
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

    #endregion

    #region Withdraw client

    /// <inheritdoc />
    WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new(_exchangeName);

    /// <inheritdoc />
    async Task<HttpResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
    {
        var validationError = ((IWithdrawRestClient)this).WithdrawOptions.ValidateRequest(request, this);
        if (validationError != null)
            return HttpResult.Fail<SharedId>(Exchange, validationError);

        var result = await Funding.WithdrawAsync(
            request.Asset,
            request.Quantity,
            request.Address,
            paymentId: request.AddressTag,
            ct: ct).ConfigureAwait(false);
        if (!result.Success)
            return HttpResult.Fail<SharedId>(result);

        // Bitvavo's withdrawal endpoint returns no id â€” echo the requested asset symbol
        // so the Shared contract's non-null SharedId is satisfied.
        return HttpResult.Ok(result, new SharedId(result.Data!.Symbol));
    }

    #endregion

    #region Shared helpers

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

    #endregion
}
