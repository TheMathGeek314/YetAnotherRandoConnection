using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class YetAnotherRandoConnection: Mod, IGlobalSettings<GlobalSettings> {
        new public string GetName() => "YetAnotherRandoConnection";
        public override string GetVersion() => "1.0.0.0";

        public static GlobalSettings Settings { get; set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => Settings = s;
        public GlobalSettings OnSaveGlobal() => Settings;

        internal static YetAnotherRandoConnection instance;

        public YetAnotherRandoConnection() : base(null) {
            instance = this;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            SoulJarContainer.jarPrefab = preloadedObjects["Ruins1_31"]["Ruins Vial Empty"];
            RandoInterop.Hook();

            On.DreamPlantOrb.Awake += LogOrbs;
        }

        private void LogOrbs(On.DreamPlantOrb.orig_Awake orig, DreamPlantOrb self) {
            orig(self);
            Log($"scene\t{self.gameObject.scene.name}\tname\t{self.gameObject.name}\tcoords\t{self.gameObject.transform.position.x}\t{self.gameObject.transform.position.y}");
        }

        public override List<(string, string)> GetPreloadNames() {
            return [
                ("Ruins1_31", "Ruins Vial Empty")
            ];
        }
    }
}