using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.BitmexAdapter.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public BitmexAdapterSettings BitmexAdapterService { get; set; }
    }
}
