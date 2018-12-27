using JetBrains.Annotations;

namespace Lykke.Service.BitmexAdapter.Client
{
    /// <summary>
    /// BitmexAdapter client interface.
    /// </summary>
    [PublicAPI]
    public interface IBitmexAdapterClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IBitmexAdapterApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application Api interface</summary>
        IBitmexAdapterApi Api { get; }
    }
}
