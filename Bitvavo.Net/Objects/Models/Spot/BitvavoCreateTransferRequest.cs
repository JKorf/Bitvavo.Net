using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Strongly-typed request payload for <c>POST /v2/subaccounts/transfers</c> â€” moves an
/// asset between an institutional main account and one of its subaccounts. The API key
/// must have the <c>Internal Transfer</c> permission.
/// </summary>
/// <param name="SubaccountId">
/// Identifier (UUID) of the subaccount involved in the transfer. Required.
/// </param>
/// <param name="Direction">
/// Whether assets move from main â†’ sub or sub â†’ main. Required.
/// </param>
/// <param name="Symbol">
/// Asset symbol to transfer (e.g. <c>"EUR"</c>, <c>"BTC"</c>). Required.
/// </param>
/// <param name="Amount">
/// Quantity of the asset to transfer. Required.
/// </param>
/// <param name="ClientRequestId">
/// Caller-supplied idempotency identifier (UUID). Optional.
/// </param>
public record BitvavoCreateTransferRequest(
    string SubaccountId,
    SubaccountTransferDirection Direction,
    string Symbol,
    decimal Amount,
    string? ClientRequestId = null);
