using Modding;  
using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class YetAnotherRandoConnection: Mod, IGlobalSettings<GlobalSettings> {
        new public string GetName() => "YetAnotherRandoConnection";
        public override string GetVersion() => "1.3.0.0";

        public static GlobalSettings Settings { get; set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => Settings = s;
        public GlobalSettings OnSaveGlobal() => Settings;

        internal static YetAnotherRandoConnection instance;

        public YetAnotherRandoConnection() : base(null) {
            instance = this;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            SoulJarContainer.soulJarPrefab = preloadedObjects["Ruins1_31"]["Ruins Vial Empty"];
            ScarecrowItem.hopperPrefab1 = preloadedObjects["Deepnest_East_16"]["Giant Hopper Summon/Giant Hopper"];
            ScarecrowItem.hopperPrefab2 = preloadedObjects["Deepnest_East_16"]["Giant Hopper Summon/Giant Hopper (1)"];
            LoveJarParent.emptyJarPrefab = preloadedObjects["Ruins2_11"]["Break Jar (8)"];
            LoveJarCrawlidContainer.contentsPrefab = preloadedObjects["Ruins2_11"]["goomba_death0006"];
            LoveJarHuskContainer.contentsPrefab = preloadedObjects["Ruins2_11_b"]["Break Jar (1)/Corpse Zombie Basic One"];
            LoveJarLeapingContainer.contentsPrefab = preloadedObjects["Ruins2_11"]["death0008"];
            LoveJarObbleContainer.contentsPrefab = preloadedObjects["Ruins2_11"]["death0001"];
            LoveJarSentryContainer.contentsPrefab = preloadedObjects["Ruins2_11"]["death0004"];
            LoveJarVengeflyContainer.contentsPrefab = preloadedObjects["Ruins2_11"]["Break Jar (3)/Buzzer Dummy"];

            RandoInterop.Hook();
        }

        public override List<(string, string)> GetPreloadNames() {
            return [
                ("Ruins1_31", "Ruins Vial Empty"),
                ("Deepnest_East_16", "Giant Hopper Summon/Giant Hopper"),
                ("Deepnest_East_16", "Giant Hopper Summon/Giant Hopper (1)"),
                ("Ruins2_11", "Break Jar (8)"),
                ("Ruins2_11", "goomba_death0006"),
                ("Ruins2_11", "death0008"),
                ("Ruins2_11", "death0001"),
                ("Ruins2_11", "death0004"),
                ("Ruins2_11", "Break Jar (3)/Buzzer Dummy"),
                ("Ruins2_11_b", "Break Jar (1)/Corpse Zombie Basic One")
            ];
        }
    }
}