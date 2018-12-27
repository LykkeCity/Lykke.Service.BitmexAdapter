using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.BitmexAdapter.Client 
{
    /// <summary>
    /// BitmexAdapter client settings.
    /// </summary>
    public class BitmexAdapterServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
