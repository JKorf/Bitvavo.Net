using Bitvavo.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Bitvavo.Net.Clients.ExchangeApi.BitvavoSharedApiMappings;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoSocketClientExchangeSharedApi
    {
        public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.AddToCost;
        public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        public SharedOrderType[] SpotSupportedOrderTypes { get; } = [SharedOrderType.Limit, SharedOrderType.LimitMaker, SharedOrderType.Market];
        public SharedTimeInForce[] SpotSupportedTimeInForce { get; } =
            [SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill];

        public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
            SharedQuantityType.BaseAsset, SharedQuantityType.BaseAsset,
            SharedQuantityType.BaseAndQuoteAsset, SharedQuantityType.BaseAndQuoteAsset);

        public string GenerateClientOrderId() => Guid.NewGuid().ToString();

        #region PlaceSpotOrder

        public PlaceSpotOrderOptions PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> IPlaceSpotOrder.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
            => await PlaceSpotOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var isMarketOrder = request.OrderType == SharedOrderType.Market;
            var quantity = request.Quantity?.QuantityInBaseAsset;
            var quoteQuantity = isMarketOrder && quantity == null ? request.Quantity?.QuantityInQuoteAsset : null;
            if (quantity == null && quoteQuantity == null)
                return QueryResult.Fail<SharedId>(Exchange, ArgumentError.Missing(nameof(request.Quantity)));

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? OrderSide.Buy : OrderSide.Sell,
                isMarketOrder ? OrderType.Market : OrderType.Limit,
                clientOrderId: request.ClientOrderId,
                quantity: quantity,
                quoteQuantity: quoteQuantity,
                price: isMarketOrder ? null : request.Price,
                timeInForce: MapTimeInForce(request.TimeInForce),
                postOnly: request.OrderType == SharedOrderType.LimitMaker ? true : null,
                responseRequired: false,
                operatorId: GetOperatorId(request, _exchangeName),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion

        #region EditSpotOrder

        public EditSpotOrderOptions EditSpotOrderOptions { get; } = new EditSpotOrderOptions(_exchangeName)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> IEditSpotOrder.EditSpotOrderAsync(EditOrderRequest request, CancellationToken ct)
            => await EditSpotOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult<SharedId>> EditSpotOrderAsync(EditOrderRequest request, CancellationToken ct)
        {
            var validationError = EditSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var quantity = request.Quantity?.QuantityInBaseAsset;
            var result = await _api.Trading.UpdateOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                orderId: request.OrderId,
                quantity: quantity,
                quoteQuantity: quantity == null ? request.Quantity?.QuantityInQuoteAsset : null,
                price: request.Price,
                responseRequired: false,
                operatorId: GetOperatorId(request, _exchangeName),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion

        #region EditSpotOrderByClientOrderId

        public EditSpotOrderByClientOrderIdOptions EditSpotOrderByClientOrderIdOptions { get; } = new EditSpotOrderByClientOrderIdOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> IEditSpotOrderByClientOrderId.EditSpotOrderByClientOrderIdAsync(EditOrderRequest request, CancellationToken ct)
            => await EditSpotOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult<SharedId>> EditSpotOrderByClientOrderIdAsync(EditOrderRequest request, CancellationToken ct)
        {
            var validationError = EditSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var quantity = request.Quantity?.QuantityInBaseAsset;
            var result = await _api.Trading.UpdateOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                clientOrderId: request.OrderId,
                quantity: quantity,
                quoteQuantity: quantity == null ? request.Quantity?.QuantityInQuoteAsset : null,
                price: request.Price,
                responseRequired: false,
                operatorId: GetOperatorId(request, _exchangeName),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion

        #region CancelSpotOrder

        public CancelSpotOrderOptions CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> ICancelSpotOrder.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.CancelOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol), orderId: request.OrderId,
                operatorId: GetOperatorId(request, _exchangeName), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion

        #region CancelSpotOrderByClientOrderId

        public CancelSpotOrderByClientOrderIdOptions CancelSpotOrderByClientOrderIdOptions { get; } = new CancelSpotOrderByClientOrderIdOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> ICancelSpotOrderByClientOrderId.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult<SharedId>> CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.CancelOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId,
                operatorId: GetOperatorId(request, _exchangeName), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion

        #region CancelAllSpotOrders

        public CancelAllSpotOrdersOptions CancelAllSpotOrdersOptions { get; } = new CancelAllSpotOrdersOptions(_exchangeName)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult> ICancelAllSpotOrders.CancelAllSpotOrdersAsync(CancelAllOrdersRequest request, CancellationToken ct)
            => await CancelAllSpotOrdersAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult> CancelAllSpotOrdersAsync(CancelAllOrdersRequest request, CancellationToken ct)
        {
            var validationError = CancelAllSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail(Exchange, validationError);

            return await _api.Trading.CancelOrdersAsync(operatorId: GetOperatorId(request, _exchangeName), ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region CancelAllSpotSymbolOrders

        public CancelAllSpotSymbolOrdersOptions CancelAllSpotSymbolOrdersOptions { get; } = new CancelAllSpotSymbolOrdersOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult> ICancelAllSpotSymbolOrders.CancelAllSpotSymbolOrdersAsync(CancelAllSymbolOrdersRequest request, CancellationToken ct)
            => await CancelAllSpotSymbolOrdersAsync(request, ct).ConfigureAwait(false);

        public async Task<QueryResult<SharedId>> CancelAllSpotSymbolOrdersAsync(CancelAllSymbolOrdersRequest request, CancellationToken ct)
        {
            var validationError = CancelAllSpotSymbolOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.CancelOrdersAsync(
                request.Symbol!.GetSymbol(FormatSymbol), operatorId: GetOperatorId(request, _exchangeName), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(null));
        }

        #endregion

        #region Subscribe Spot Orders

        public SubscribeSpotOrderOptions SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, true);

        public Task<WebSocketResult<UpdateSubscription>> SubscribeToSpotOrderUpdatesAsync(
            SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError));

            return _api.Trading.SubscribeToUserDataUpdatesAsync(update =>
            {
                var order = update.Data;
                handler(update.ToType<SharedSpotOrderUpdate[]>([new SharedSpotOrderUpdate(
                    ParseSymbol(order.Symbol), order.Symbol, order.Id, ParseOrderType(order),
                    order.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(order.Status), order.CreateTime)
                {
                    ClientOrderId = order.ClientOrderId,
                    OrderPrice = order.Price,
                    OrderQuantity = new SharedOrderQuantity(order.Quantity, order.QuoteQuantity),
                    QuantityFilled = new SharedOrderQuantity(order.FilledQuantity, order.FilledQuoteQuantity),
                    AveragePrice = GetAveragePrice(order),
                    TimeInForce = ParseTimeInForce(order.TimeInForce),
                    UpdateTime = order.UpdateTime,
                    IsTriggerOrder = order.Type is OrderType.StopLoss or OrderType.StopLossLimit or OrderType.TakeProfit or OrderType.TakeProfitLimit,
                    TriggerPrice = order.TriggerPrice ?? order.RequestedTriggerPrice
                }]));
            }, null, ct);
        }

        #endregion
    }
}
