using System.Net.Http;
using Bitvavo.Net.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using Shouldly;
using Xunit;

namespace Bitvavo.Net.Tests.Clients.SpotApi;

/// <summary>
/// Unit tests for <see cref="RequestDefinitionCacheExtensions"/> â€” the Bitvavo-internal
/// "InUri-DELETE" cache helper. The 5-arg overload added in P1-bis threads the per-host
/// <see cref="BitvavoExchange.RateLimiter.Bitvavo"/> and a per-endpoint weight through
/// <c>GetOrCreate</c> so InUri DELETEs are rate-limit-gated like every other endpoint.
/// </summary>
public class RequestDefinitionCacheExtensionsTests
{
    [Fact]
    public void GetOrCreateInUri_Gated_SetsGateWeightAndInUriPosition()
    {
        var cache = new RequestDefinitionCache();

        var def = cache.GetOrCreate(
            HttpMethod.Delete,
            "https://api.bitvavo.com",
            "v2/order",
            BitvavoExchange.RateLimiter.Bitvavo,
            weight: 1,
            authenticated: true,
            parameterPosition: HttpMethodParameterPosition.InUri);

        def.RateLimitGate.ShouldBeSameAs(BitvavoExchange.RateLimiter.Bitvavo);
        def.Weight.ShouldBe(1);
        def.ParameterPosition.ShouldBe(HttpMethodParameterPosition.InUri);
        def.Authenticated.ShouldBeTrue();
        def.Method.ShouldBe(HttpMethod.Delete);
        // RequestDefinitionCache normalises the path to a leading-slash form.
        def.Path.ShouldEndWith("v2/order");
    }

    [Fact]
    public void GetOrCreateInUri_Gated_IsIdempotent_ReturnsSameInstance()
    {
        var cache = new RequestDefinitionCache();

        var first = cache.GetOrCreate(
            HttpMethod.Delete,
            "https://api.bitvavo.com",
            "v2/orders",
            BitvavoExchange.RateLimiter.Bitvavo,
            weight: 100,
            authenticated: true,
            parameterPosition: HttpMethodParameterPosition.InUri);
        var second = cache.GetOrCreate(
            HttpMethod.Delete,
            "https://api.bitvavo.com",
            "v2/orders",
            BitvavoExchange.RateLimiter.Bitvavo,
            weight: 100,
            authenticated: true,
            parameterPosition: HttpMethodParameterPosition.InUri);

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void GetOrCreateInUri_Gated_CarriesEndpointWeight()
    {
        var cache = new RequestDefinitionCache();

        var def = cache.GetOrCreate(
            HttpMethod.Delete,
            "https://api.bitvavo.com",
            "v2/orders",
            BitvavoExchange.RateLimiter.Bitvavo,
            weight: 100,
            authenticated: true,
            parameterPosition: HttpMethodParameterPosition.InUri);

        def.Weight.ShouldBe(100);
    }
}
