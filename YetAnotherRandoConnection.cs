using ItemChanger;
using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class YetAnotherRandoConnection: Mod, IGlobalSettings<GlobalSettings> {
        new public string GetName() => "YetAnotherRandoConnection";
        public override string GetVersion() => "1.0.0.4";

        public static GlobalSettings Settings { get; set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => Settings = s;
        public GlobalSettings OnSaveGlobal() => Settings;

        internal static YetAnotherRandoConnection instance;

        public YetAnotherRandoConnection() : base(null) {
            instance = this;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            SoulJarContainer.jarPrefab = preloadedObjects["Ruins1_31"]["Ruins Vial Empty"];
            RandoInterop.Hook();
            if (ModHooks.GetMod("FStatsMod") is Mod)
                FStats_Interop.Hook();
        }

        public override List<(string, string)> GetPreloadNames() {
            return [
                ("Ruins1_31", "Ruins Vial Empty"),
            ];
        }
    }
}