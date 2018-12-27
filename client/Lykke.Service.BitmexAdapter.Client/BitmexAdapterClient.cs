using Lykke.HttpClientGenerator;

namespace Lykke.Service.BitmexAdapter.Client
{
    /// <summary>
    /// BitmexAdapter API aggregating interface.
    /// </summary>
    public class BitmexAdapterClient : IBitmexAdapterClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to BitmexAdapter Api.</summary>
        public IBitmexAdapterApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public BitmexAdapterClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IBitmexAdapterApi>();
        }
    }
}
