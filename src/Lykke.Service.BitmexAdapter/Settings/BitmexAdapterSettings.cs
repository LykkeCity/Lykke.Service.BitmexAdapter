using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.BitmexAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BitmexAdapterSettings
    {
        public DbSettings Db { get; set; }
    }
}
