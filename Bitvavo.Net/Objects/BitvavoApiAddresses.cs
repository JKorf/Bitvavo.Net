namespace Bitvavo.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class BitvavoApiAddresses
    {
        /// <summary>
        /// The address used by the BitvavoRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the BitvavoSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the Bitvavo API
        /// </summary>
        public static BitvavoApiAddresses Default = new BitvavoApiAddresses
        {
            RestClientAddress = "https://api.bitvavo.com",
            SocketClientAddress = "wss://ws.bitvavo.com"
        };
    }
}
