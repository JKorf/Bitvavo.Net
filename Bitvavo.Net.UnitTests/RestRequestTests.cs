using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitvavo.Net.Clients;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateAccountCalls()
        {
            var client = new BitvavoRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new BitvavoCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitvavoRestClient>(client, "Endpoints/Spot/ExchangeData", "XXX", IsAuthenticated);
            //await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetServerTimeAsync(), "GetServerTime");
           
        }

        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestUrl?.Contains("signature") == true || result.RequestBody?.Contains("signature=") == true;
        }
    }
}
