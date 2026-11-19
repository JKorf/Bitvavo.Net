using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi
    {
        public GetLedgerOptions GetLedgerOptions { get; } = new GetLedgerOptions(_exchangeName, false, true, true, 100);

        async Task<IExchangeCallResult<SharedLedgerEntry[]>> IGetLedger.GetLedgerAsync(GetLedgerRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetLedgerAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedLedgerEntry[]>> GetLedgerAsync(GetLedgerRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetLedgerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLedgerEntry[]>(Exchange, validationError);

            var limit = request.Limit ?? 100;
            var endTime = pageRequest?.EndTime ?? request.EndTime ?? DateTime.UtcNow;
            var startTime = pageRequest?.StartTime ?? request.StartTime;
            var result = await _api.Account.GetTransactionHistoryAsync(
                startTime: startTime, endTime: endTime, page: pageRequest?.Page ?? 1, pageSize: limit, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLedgerEntry[]>(result);

            var entries = result.Data.Items.SelectMany(ParseLedgerEntries)
                .Where(x => request.Asset == null || string.Equals(x.Asset, request.Asset, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Timestamp)
                .ToArray();
            var offset = pageRequest?.Offset ?? 0;
            var data = entries.Skip(offset).Take(limit).ToArray();
            var nextOffset = offset + data.Length;
            PageRequest? next = null;
            if (nextOffset < entries.Length)
                next = new PageRequest { Page = result.Data.CurrentPage, Offset = nextOffset, StartTime = startTime, EndTime = endTime };
            else if (result.Data.CurrentPage < result.Data.TotalPages)
                next = new PageRequest { Page = result.Data.CurrentPage + 1, StartTime = startTime, EndTime = endTime };

            return HttpResult.Ok(result, data, next);
        }

        private static IEnumerable<SharedLedgerEntry> ParseLedgerEntries(BitvavoTransaction transaction)
        {
            var type = transaction.Type switch
            {
                TransactionType.Buy or TransactionType.Sell or TransactionType.ConvertBuy or TransactionType.ConvertSell => SharedLedgerEntryType.Trade,
                TransactionType.Deposit => SharedLedgerEntryType.Deposit,
                TransactionType.Withdrawal => SharedLedgerEntryType.Withdrawal,
                TransactionType.Transfer or TransactionType.InternalTransfer => SharedLedgerEntryType.Transfer,
                TransactionType.Rebate => SharedLedgerEntryType.Rebate,
                _ => SharedLedgerEntryType.Unknown
            };
            var typeName = EnumConverter.GetString(transaction.Type);

            if (transaction.SentAsset is { Length: > 0 } sentAsset && transaction.SentQuantity is { } sentQuantity)
                yield return new SharedLedgerEntry(sentAsset, -sentQuantity, type, typeName, transaction.Timestamp) { RelationId = transaction.Id };

            if (transaction.ReceivedAsset is { Length: > 0 } receivedAsset && transaction.ReceivedQuantity is { } receivedQuantity)
                yield return new SharedLedgerEntry(receivedAsset, receivedQuantity, type, typeName, transaction.Timestamp) { RelationId = transaction.Id };

            if (transaction.FeeAsset is { Length: > 0 } feeAsset && transaction.FeeQuantity is { } fee && fee != 0)
                yield return new SharedLedgerEntry(feeAsset, -fee, fee < 0 ? SharedLedgerEntryType.Rebate : SharedLedgerEntryType.Fee, typeName, transaction.Timestamp)
                {
                    RelationId = transaction.Id
                };
        }
    }
}
