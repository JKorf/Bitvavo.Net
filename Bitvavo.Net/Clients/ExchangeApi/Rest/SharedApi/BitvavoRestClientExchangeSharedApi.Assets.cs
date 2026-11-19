using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi
    {
        public GetAssetOptions GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, false);
        public GetAllAssetsOptions GetAllAssetsOptions { get; } = new GetAllAssetsOptions(_exchangeName, false);

        async Task<IExchangeCallResult<SharedAsset>> IGetAsset.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
            => await GetAssetAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedAsset>> GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var result = await _api.ExchangeData.GetAssetAsync(request.Asset, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedAsset>(result);

            return HttpResult.Ok(result, ParseAsset(result.Data));
        }

        async Task<IExchangeCallResult<SharedAsset[]>> IGetAllAssets.GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => await GetAllAssetsAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedAsset[]>> GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = GetAllAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetAssetsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedAsset[]>(result);

            return HttpResult.Ok(result, result.Data.Select(ParseAsset).ToArray());
        }

        private static SharedAsset ParseAsset(BitvavoAsset asset)
        {
            // Bitvavo supplies fees, quantities, and confirmations at the asset level.
            var singleNetwork = asset.Networks.Length == 1;
            return new SharedAsset(asset.Asset)
            {
                FullName = asset.Name,
                Networks = asset.Networks.Select(network => new SharedAssetNetwork(network)
                {
                    DepositEnabled = singleNetwork || asset.DepositStatus != AssetTransferStatus.Ok
                        ? asset.DepositStatus == AssetTransferStatus.Ok : null,
                    WithdrawEnabled = singleNetwork || asset.WithdrawalStatus != AssetTransferStatus.Ok
                        ? asset.WithdrawalStatus == AssetTransferStatus.Ok : null,
                    MinConfirmations = singleNetwork ? asset.DepositConfirmations : null,
                    MinWithdrawQuantity = singleNetwork ? asset.MinWithdrawalQuantity : null,
                    WithdrawFee = singleNetwork ? asset.WithdrawalFee : null
                }).ToArray()
            };
        }
    }
}
