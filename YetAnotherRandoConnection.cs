using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class YetAnotherRandoConnection: Mod, IGlobalSettings<GlobalSettings> {
        new public string GetName() => "YetAnotherRandoConnection";
        public override string GetVersion() => "1.1.0.2";

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
            ScarecrowItem.hopperPrefab1 = preloadedObjects["Deepnest_East_16"]["Giant Hopper Summon/Giant Hopper"];
            ScarecrowItem.hopperPrefab2 = preloadedObjects["Deepnest_East_16"]["Giant Hopper Summon/Giant Hopper (1)"];
            RandoInterop.Hook();
        }

        public override List<(string, string)> GetPreloadNames() {
            return [
                ("Ruins1_31", "Ruins Vial Empty"),
                ("Deepnest_East_16", "Giant Hopper Summon/Giant Hopper"),
                ("Deepnest_East_16", "Giant Hopper Summon/Giant Hopper (1)")
            ];
        }
    }
}