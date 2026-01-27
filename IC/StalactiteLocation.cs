using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace YetAnotherRandoConnection {
    public class StalactiteLocation: AutoLocation {
        private static readonly Dictionary<string, StalactiteLocation> SubscribedLocations = new();
        private static FieldInfo _fallen = typeof(StalactiteControl).GetField("fallen", BindingFlags.NonPublic | BindingFlags.Instance);
        
        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookStalactites();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookStalactites();
        }

        private static void HookStalactites() {
            On.StalactiteControl.OnTriggerEnter2D += triggerCheck;
        }

        private static void UnhookStalactites() {
            On.StalactiteControl.OnTriggerEnter2D -= triggerCheck;
        }

        private static void triggerCheck(On.StalactiteControl.orig_OnTriggerEnter2D orig, StalactiteControl self, Collider2D collision) {
            if(((bool)_fallen.GetValue(self)) && collision.gameObject.layer == 8) {
                string placement;
                if(self.gameObject.scene.name == "Tutorial_01" && self.gameObject.name == "Stalactite Hazard") {
                    string parent = self.gameObject.transform.parent.name;
                    placement = $"Stalactite-King's_Pass_{(parent == "_Scenery" ? "Spike_Pit_1" : "Focus_Tablet")}";
                }
                else {
                    placement = StalactiteCoords.nameToPlacement[(self.gameObject.scene.name, self.gameObject.name)];
                }
                GiveInfo giveInfo = new() {
                    FlingType = FlingType.Everywhere,
                    Container = Container.Unknown,
                    MessageType = MessageType.Corner,
                    Transform = self.gameObject.transform
                };
                Ref.Settings.Placements[placement].GiveAll(giveInfo);
            }
            orig(self, collision);
        }
    }
}
