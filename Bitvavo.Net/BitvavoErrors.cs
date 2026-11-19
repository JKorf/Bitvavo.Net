using CryptoExchange.Net.Objects.Errors;

namespace Bitvavo.Net
{
    internal static class BitvavoErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping(
            [
            new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid JSON", "102"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Path parameters are not allowed", "200"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Body parameters are not allowed", "201"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Parameter is unsupported for this order type", "202"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Missing or incompatible parameters", "203"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid parameter", "204", "205"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Conflicting parameters", "206", "236"),
            new ErrorInfo(ErrorType.InvalidQuantity, false, "Quantity exceeds the market maximum", "210"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Price exceeds the allowed maximum", "211"),
            new ErrorInfo(ErrorType.InvalidQuantity, false, "Quantity is below the allowed minimum", "212", "217", "406"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Price is below the allowed minimum", "213"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Price has too many decimal places", "215"),
            new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "216", "408"),
            new ErrorInfo(ErrorType.UnknownSymbol, false, "Market has been delisted", "219"),
            new ErrorInfo(ErrorType.DuplicateClientOrderId, false, "Client order ID already exists for this market", "220"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Paused markets require a good-till-canceled order", "231"),
            new ErrorInfo(ErrorType.MissingParameter, false, "Order update requires at least one change", "232"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Market orders cannot be updated", "234"),
            new ErrorInfo(ErrorType.RateLimitOrder, false, "Open order limit reached for this market", "235"),
            new ErrorInfo(ErrorType.InvalidStopParameters, false, "Missing stop order parameters", "237", "238"),
            new ErrorInfo(ErrorType.RejectedOrderConfiguration, false, "Order update cannot change the quantity denomination", "239"),
            new ErrorInfo(ErrorType.UnknownOrder, false, "Order does not exist or is inactive", "240"),

            new ErrorInfo(ErrorType.Unauthorized, false, "Authentication is required", "300"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key length", "301"),
            new ErrorInfo(ErrorType.InvalidTimestamp, false, "Request timestamp must be in milliseconds", "302"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Access window is outside the allowed range", "303"),
            new ErrorInfo(ErrorType.InvalidTimestamp, false, "Request arrived outside the access window", "304"),
            new ErrorInfo(ErrorType.Unauthorized, false, "API key is inactive or unconfirmed", "305", "306"),
            new ErrorInfo(ErrorType.Unauthorized, false, "IP address is not allowed for the API key", "307"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Invalid request signature", "308", "309"),
            new ErrorInfo(ErrorType.Unauthorized, false, "API key lacks the required permission", "310", "311", "312", "511", "512", "513"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Invalid session", "313"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Invalid VPC ID", "314"),
            new ErrorInfo(ErrorType.Unauthorized, false, "WebSocket connection only allows public asset information", "316"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Account is locked or inactive", "317", "322"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Account verification is required", "318"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Feature is restricted in this region", "319"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Operation is forbidden", "320"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Subaccount is unavailable to this API key", "510"),
            new ErrorInfo(ErrorType.InvalidOperation, false, "Request is unavailable for subaccount management", "514"),

            new ErrorInfo(ErrorType.InvalidOperation, false, "Deposits are unavailable for this asset", "401"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Identity or phone verification is required", "402", "403"),
            new ErrorInfo(ErrorType.IncorrectState, false, "Withdrawal cooldown is active", "405", "414"),
            new ErrorInfo(ErrorType.InvalidOperation, false, "Internal transfers are unavailable for this asset", "407"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Bank account verification is required", "409"),
            new ErrorInfo(ErrorType.InvalidOperation, false, "Withdrawals are unavailable for this asset", "410"),
            new ErrorInfo(ErrorType.InvalidOperation, false, "Transfer to the same account is forbidden", "411"),
            new ErrorInfo(ErrorType.InvalidOperation, false, "Deposit or withdrawal requirements were not met", "412"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Withdrawal address is not allowed", "413", "439"),
            new ErrorInfo(ErrorType.RiskError, false, "Withdrawal exceeds the account limit", "434"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Withdrawal address ownership must be verified", "435"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Crypto withdrawals are blocked for this account", "436", "437"),
            new ErrorInfo(ErrorType.RiskError, false, "An outstanding loan prevents this withdrawal", "438"),

            new ErrorInfo(ErrorType.InvalidOperation, false, "Unknown WebSocket action", "415"),
            new ErrorInfo(ErrorType.InvalidPrice, false, "Price is not a multiple of the tick size", "422"),
            new ErrorInfo(ErrorType.UnavailableSymbol, true, "Market is temporarily unavailable for trading", "423", "424", "425", "426", "431"),
            new ErrorInfo(ErrorType.InvalidParameter, false, "Parameter exceeds the allowed decimal precision", "429"),

            new ErrorInfo(ErrorType.RateLimitRequest, true, "Request rate limit exceeded", "105"),
            new ErrorInfo(ErrorType.RateLimitRequest, true, "WebSocket message rate limit exceeded", "112"),
            new ErrorInfo(ErrorType.SystemError, true, "Bitvavo service is unavailable", "101", "111", "419"),
            new ErrorInfo(ErrorType.SystemError, true, "Internal server error", "400", "404"),
            new ErrorInfo(ErrorType.SystemError, true, "Bitvavo is overloaded", "107"),
            new ErrorInfo(ErrorType.SystemError, true, "Bitvavo could not process the request", "108"),
            new ErrorInfo(ErrorType.Timeout, false, "Request timed out and its outcome is unknown", "109"),
            new ErrorInfo(ErrorType.Timeout, false, "Connection timed out waiting for market data", "430"),
            ]
            );
    }
}
