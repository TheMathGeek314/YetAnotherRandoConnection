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
            RandoInterop.Hook();
            CustomCheckEdits.Hook();
            On.DreamPlantOrb.Awake += orbLogger;
        }

        private void orbLogger(On.DreamPlantOrb.orig_Awake orig, DreamPlantOrb self) {
            orig(self);
            string area = Consts.OrbAreas[self.gameObject.scene.name];
            float x = self.gameObject.transform.position.x;
            float y = self.gameObject.transform.position.y;
            string name = self.gameObject.name;
            Log($"{name}\t{area}\t{x}\t{y}");
        }
    }
}