using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Bitvavo.Net.Objects.Internal;

internal sealed class BitvavoSubscribeQuery : Query<BitvavoSocketRequest>
{
    public BitvavoSubscribeQuery(BitvavoSocketRequest request)
        : base(request, authenticated: false, weight: 1)
    {
        ExpectsResponse = false;
#warning TODO check sub response validation
        MessageRouter = MessageRouter.CreateVoid<object>($"bitvavo-subscribe-{Id}");
    }
}
