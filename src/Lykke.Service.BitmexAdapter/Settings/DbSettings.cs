using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.BitmexAdapter.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
