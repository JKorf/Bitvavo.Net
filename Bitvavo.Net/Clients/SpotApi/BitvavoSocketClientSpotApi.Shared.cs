using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc />
internal sealed partial class BitvavoSocketClientSpotApi :
    IBitvavoSocketClientSpotApiShared
{
    private const string _exchangeName = "Bitvavo";

    /// <summary>
    /// <see cref="ExchangeParameters"/> key carrying the <c>string[]</c> of markets the
    /// account-channel subscription should cover (e.g. <c>["ETH-EUR", "BTC-EUR"]</c>).
    /// </summary>
    public const string MarketsExchangeParameter = "Markets";

    private static readonly TradingMode[] _spotOnlySocket = [TradingMode.Spot];

    #region Shared client

    /// <inheritdoc />
    string ISharedClient.Exchange => BitvavoExchange.ExchangeName;

    /// <inheritdoc />
    TradingMode[] ISharedClient.SupportedTradingModes => _spotOnlySocket;

    // ISharedClient.Authenticated is satisfied by the inherited SocketApiClient.Authenticated.

    /// <inheritdoc />
    void ISharedClient.SetDefaultExchangeParameter(string name, object value)
        => ExchangeParameters.SetStaticParameter(BitvavoExchange.ExchangeName, name, value);

    /// <inheritdoc />
    void ISharedClient.ResetDefaultExchangeParameters()
        => ExchangeParameters.ResetStaticParameters();

    /// <inheritdoc />
    SharedClientInfo ISharedClient.Discover() => SharedUtils.GetClientInfo(BitvavoExchange.Metadata, this);

    #endregion

    #region Kline client

    private static readonly SharedKlineInterval[] _supportedSocketKlineIntervals =
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
    SubscribeKlineOptions IKlineSocketClient.SubscribeKlineOptions { get; } = new(_exchangeName, false, _supportedSocketKlineIntervals);

    /// <inheritdoc />
    async Task<WebSocketResult<UpdateSubscription>> IKlineSocketClient.SubscribeToKlineUpdatesAsync(
        SubscribeKlineRequest request,
        Action<DataEvent<SharedKline>> handler,
        CancellationToken ct)
    {
        var validationError = ((IKlineSocketClient)this).SubscribeKlineOptions.ValidateRequest(request, this);
        if (validationError != null)
            return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

        var interval = (KlineInterval)request.Interval;
        var sharedSymbol = request.Symbol!;
        var symbol = sharedSymbol.GetSymbol(FormatSymbol);

        var result = await ExchangeData.SubscribeToKlineUpdatesAsync(
            symbol,
            interval,
            update =>
            {
                foreach (var candle in update.Data.Candle)
                {
                    handler(update.ToType(new SharedKline(
                        sharedSymbol,
                        symbol,
                        candle.OpenTime,
                        candle.ClosePrice,
                        candle.HighPrice,
                        candle.LowPrice,
                        candle.OpenPrice,
                        candle.Volume)));
                }
            },
            ct).ConfigureAwait(false);

        return result;
    }

    #endregion

    #region Trade client

    /// <inheritdoc />
    SubscribeTradeOptions ITradeSocketClient.SubscribeTradeOptions { get; } = new(_exchangeName, false);

    /// <inheritdoc />
    async Task<WebSocketResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(
        SubscribeTradeRequest request,
        Action<DataEvent<SharedTrade[]>> handler,
        CancellationToken ct)
    {
        var validationError = ((ITradeSocketClient)this).SubscribeTradeOptions.ValidateRequest(request, this);
        if (validationError != null)
            return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

        var sharedSymbol = request.Symbol!;
        var symbol = sharedSymbol.GetSymbol(FormatSymbol);

        var result = await ExchangeData.SubscribeToTradeUpdatesAsync(
            symbol,
            update => handler(update.ToType<SharedTrade[]>(
            [
                new SharedTrade(
                    sharedSymbol,
                    symbol,
                    update.Data.Amount,
                    update.Data.Price,
                    update.Data.Timestamp)
                {
                    Side = ToSharedSocketOrderSide(update.Data.Side),
                },
            ])),
            ct).ConfigureAwait(false);

        return result;
    }

    #endregion

    #region Spot Order client

    /// <inheritdoc />
    SubscribeSpotOrderOptions ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; } = new(_exchangeName, true)
    {
        EndpointName = "SubscribeSpotOrderRequest",
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                MarketsExchangeParameter,
                typeof(string[]),
                "Markets the Bitvavo account channel subscription should cover",
                new[] { "ETH-EUR" }),
        ],
    };

    /// <inheritdoc />
    async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(
        SubscribeSpotOrderRequest request,
        Action<DataEvent<SharedSpotOrder[]>> handler,
        CancellationToken ct)
    {
        var validationError = ((ISpotOrderSocketClient)this).SubscribeSpotOrderOptions.ValidateRequest(request, this);
        if (validationError != null)
            return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

        var markets = ExchangeParameters.GetValue<string[]>(request.ExchangeParameters, Exchange, MarketsExchangeParameter)
            ?? throw new ArgumentException($"The '{MarketsExchangeParameter}' exchange parameter (string[]) is required â€” Bitvavo's account channel is subscribed per market", nameof(request));

        var result = await Account.SubscribeToOrderUpdatesAsync(
            markets,
            update =>
            {
                var order = update.Data;
                var sharedSymbol = ParseSocketSharedSymbol(order.Market);
                handler(update.ToType<SharedSpotOrder[]>(
                [
                    new SharedSpotOrder(
                        sharedSymbol,
                        order.Market,
                        order.OrderId,
                        ToSharedSocketOrderType(order.OrderType),
                        ToSharedSocketOrderSide(order.Side),
                        ToSharedSocketOrderStatus(order.Status),
                        order.Created)
                    {
                        ClientOrderId = order.ClientOrderId,
                        OrderPrice = order.Price,
                        OrderQuantity = new SharedOrderQuantity(order.Amount, null),
                        QuantityFilled = new SharedOrderQuantity(order.FilledAmount, order.FilledAmountQuote),
                        TimeInForce = order.TimeInForce == null ? null : ToSharedSocketTimeInForce(order.TimeInForce.Value),
                        UpdateTime = order.Updated,
                        TriggerPrice = order.TriggerAmount,
                        IsTriggerOrder = order.TriggerAmount != null,
                    },
                ]));
            },
            ct).ConfigureAwait(false);

        return result;
    }

    #endregion

    #region User Trade client

    /// <inheritdoc />
    SubscribeUserTradeOptions IUserTradeSocketClient.SubscribeUserTradeOptions { get; } = new(_exchangeName, true)
    {
        EndpointName = "SubscribeUserTradeRequest",
        RequiredExchangeParameters =
        [
            new ParameterDescription(
                MarketsExchangeParameter,
                typeof(string[]),
                "Markets the Bitvavo account channel subscription should cover",
                new[] { "ETH-EUR" }),
        ],
    };

    /// <inheritdoc />
    async Task<WebSocketResult<UpdateSubscription>> IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(
        SubscribeUserTradeRequest request,
        Action<DataEvent<SharedUserTrade[]>> handler,
        CancellationToken ct)
    {
        var validationError = ((IUserTradeSocketClient)this).SubscribeUserTradeOptions.ValidateRequest(request, this);
        if (validationError != null)
            return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

        var markets = ExchangeParameters.GetValue<string[]>(request.ExchangeParameters, Exchange, MarketsExchangeParameter)
            ?? throw new ArgumentException($"The '{MarketsExchangeParameter}' exchange parameter (string[]) is required â€” Bitvavo's account channel is subscribed per market", nameof(request));

        // User trades are surfaced from Bitvavo's account-channel `fill` event.
        var result = await Account.SubscribeToFillUpdatesAsync(
            markets,
            update =>
            {
                var fill = update.Data;
                var sharedSymbol = ParseSocketSharedSymbol(fill.Market);
                handler(update.ToType<SharedUserTrade[]>(
                [
                    new SharedUserTrade(
                        sharedSymbol,
                        fill.Market,
                        fill.OrderId,
                        fill.FillId,
                        ToSharedSocketOrderSide(fill.Side),
                        fill.Amount,
                        fill.Price,
                        fill.Timestamp)
                    {
                        ClientOrderId = fill.ClientOrderId,
                        Fee = fill.Fee,
                        FeeAsset = fill.FeeCurrency,
                        Role = fill.Taker ? SharedRole.Taker : SharedRole.Maker,
                    },
                ]));
            },
            ct).ConfigureAwait(false);

        return result;
    }

    #endregion

    #region Shared socket helpers

    private static SharedOrderSide ToSharedSocketOrderSide(OrderSide side) => side switch
    {
        OrderSide.Buy => SharedOrderSide.Buy,
        OrderSide.Sell => SharedOrderSide.Sell,
        _ => throw new ArgumentOutOfRangeException(nameof(side), side, "Unmapped Bitvavo OrderSide"),
    };

    private static SharedOrderType ToSharedSocketOrderType(OrderType type) => type switch
    {
        OrderType.Market => SharedOrderType.Market,
        OrderType.Limit => SharedOrderType.Limit,
        OrderType.StopLoss => SharedOrderType.Other,
        OrderType.StopLossLimit => SharedOrderType.Other,
        OrderType.TakeProfit => SharedOrderType.Other,
        OrderType.TakeProfitLimit => SharedOrderType.Other,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unmapped Bitvavo OrderType"),
    };

    private static SharedOrderStatus ToSharedSocketOrderStatus(OrderStatus status) => status switch
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

    private static SharedTimeInForce ToSharedSocketTimeInForce(TimeInForce timeInForce) => timeInForce switch
    {
        TimeInForce.GoodTillCanceled => SharedTimeInForce.GoodTillCanceled,
        TimeInForce.ImmediateOrCancel => SharedTimeInForce.ImmediateOrCancel,
        TimeInForce.FillOrKill => SharedTimeInForce.FillOrKill,
        _ => throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "Unmapped Bitvavo TimeInForce"),
    };

    #endregion
}
