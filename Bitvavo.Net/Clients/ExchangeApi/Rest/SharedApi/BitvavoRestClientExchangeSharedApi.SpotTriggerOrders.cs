using Bitvavo.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Threading;
using System.Threading.Tasks;
using static Bitvavo.Net.Utils.BitvavoSharedApiMappings;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi
    {
        #region PlaceSpotTriggerOrder

        public PlaceSpotTriggerOrderOptions PlaceSpotTriggerOrderOptions { get; } = new PlaceSpotTriggerOrderOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> IPlaceSpotTriggerOrder.PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
            => await PlaceSpotTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> PlaceSpotTriggerOrderAsync(PlaceSpotTriggerOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var isBuy = request.OrderSide == SharedOrderSide.Buy;
            var stopLoss = isBuy == (request.PriceDirection == SharedTriggerPriceDirection.PriceAbove);
            var isLimit = request.OrderPrice.HasValue;
            var type = stopLoss
                ? isLimit ? OrderType.StopLossLimit : OrderType.StopLoss
                : isLimit ? OrderType.TakeProfitLimit : OrderType.TakeProfit;

            var quantity = request.Quantity.QuantityInBaseAsset;
            var quoteQuantity = !isLimit && quantity == null ? request.Quantity.QuantityInQuoteAsset : null;
            if (quantity == null && quoteQuantity == null)
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Missing(nameof(request.Quantity)));

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol), isBuy ? OrderSide.Buy : OrderSide.Sell, type,
                clientOrderId: request.ClientOrderId,
                quantity: quantity,
                quoteQuantity: quoteQuantity,
                price: request.OrderPrice,
                triggerPrice: request.TriggerPrice,
                triggerType: TriggerType.Price,
                triggerReference: TriggerReference.LastTrade,
                timeInForce: MapTimeInForce(request.TimeInForce),
                responseRequired: false,
                operatorId: GetOperatorId(request, _exchangeName),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion

        #region GetSpotTriggerOrder

        public GetSpotTriggerOrderOptions GetSpotTriggerOrderOptions { get; } = new GetSpotTriggerOrderOptions(_exchangeName, true);

        async Task<IExchangeCallResult<SharedSpotTriggerOrder>> IGetSpotTriggerOrder.GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetSpotTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedSpotTriggerOrder>> GetSpotTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTriggerOrder>(Exchange, validationError);

            var result = await _api.Trading.GetOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), request.OrderId, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTriggerOrder>(result);

            var order = result.Data;
            var triggerPrice = order.TriggerPrice ?? order.RequestedTriggerPrice;
            if (!triggerPrice.HasValue)
                return HttpResult.Fail<SharedSpotTriggerOrder>(result, new DeserializeError("Bitvavo did not return a trigger price"));

            var status = order.Status switch
            {
                OrderStatus.AwaitingTrigger => SharedTriggerOrderStatus.Active,
                OrderStatus.New or OrderStatus.PartiallyFilled => SharedTriggerOrderStatus.Triggered,
                OrderStatus.Filled => SharedTriggerOrderStatus.Filled,
                OrderStatus.Expired or OrderStatus.Canceled => SharedTriggerOrderStatus.CanceledOrRejected,
                _ => SharedTriggerOrderStatus.Unknown
            };

            return HttpResult.Ok(result, new SharedSpotTriggerOrder(
                request.Symbol, order.Symbol, order.Id, ParseOrderType(order),
                order.Side == OrderSide.Buy ? SharedTriggerOrderDirection.Enter : SharedTriggerOrderDirection.Exit,
                status, triggerPrice.Value, order.CreateTime)
            {
                PlacedOrderId = status is SharedTriggerOrderStatus.Triggered or SharedTriggerOrderStatus.Filled ? order.Id : null,
                ClientOrderId = order.ClientOrderId,
                OrderPrice = order.Price,
                AveragePrice = GetAveragePrice(order),
                OrderQuantity = new SharedOrderQuantity(order.Quantity, order.QuoteQuantity),
                QuantityFilled = new SharedOrderQuantity(order.FilledQuantity, order.FilledQuoteQuantity),
                TimeInForce = ParseTimeInForce(order.TimeInForce),
                Fee = order.FeeQuantity,
                FeeAsset = order.FeeAsset,
                UpdateTime = order.UpdateTime
            });
        }

        #endregion

        #region CancelSpotTriggerOrder

        public CancelSpotTriggerOrderOptions CancelSpotTriggerOrderOptions { get; } = new CancelSpotTriggerOrderOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [OperatorIdRule]
        };

        async Task<IExchangeCallResult<SharedId>> ICancelSpotTriggerOrder.CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> CancelSpotTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.CancelOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol), request.OrderId, operatorId: GetOperatorId(request, _exchangeName), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.Id));
        }

        #endregion
    }
}
