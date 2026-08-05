using ConnectionSettingsRando;

namespace YetAnotherRandoConnection
{
    internal static class CSRInterop
    {
        public static void Hook()
        {
            CSR.Register(
            YetAnotherRandoConnection.instance.GetName(),
            () => YetAnotherRandoConnection.Settings,
            s => SettingsRandomizer.CopyTo(s, YetAnotherRandoConnection.Settings));
        }
    }
}