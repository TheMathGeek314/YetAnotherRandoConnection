using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;

namespace YetAnotherRandoConnection {
    internal static class RSMInterop {
        public static void Hook() {
            RandoSettingsManagerMod.Instance.RegisterConnection(new YarcSettingsProxy());
        }
    }

    internal class YarcSettingsProxy: RandoSettingsProxy<GlobalSettings, string> {
        public override string ModKey => YetAnotherRandoConnection.instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; } = new EqualityVersioningPolicy<string>(YetAnotherRandoConnection.instance.GetVersion());

        public override void ReceiveSettings(GlobalSettings settings) {
            settings ??= new();
            RandoMenuPage.Instance.ResetMenu(settings);
        }

        public override bool TryProvideSettings(out GlobalSettings settings) {
            settings = YetAnotherRandoConnection.Settings;
            return settings.Any;
        }
    }
}