using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;
using Satchel;

namespace YetAnotherRandoConnection {
    public class HivePlatformLocation: AutoLocation {
        private static readonly Dictionary<string, HivePlatformLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookPlats();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookPlats();
        }

        private static void HookPlats() {
            On.PlayMakerFSM.OnEnable += EditSmashFsm;
        }

        private static void UnhookPlats() {
            On.PlayMakerFSM.OnEnable -= EditSmashFsm;
        }

        private static void EditSmashFsm(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            if(self.FsmName == "Smash") {
                self.GetValidState("Smash").AddCustomAction(() => {
                    string id = $"{self.gameObject.scene.name}/{self.gameObject.name}";
                    if(HivePlatCoords.nameToPlacement.TryGetValue(id, out string placement)) {
                        GiveInfo giveInfo = new() {
                            Container = Container.Unknown,
                            FlingType = FlingType.Everywhere,
                            Transform = self.gameObject.transform,
                            MessageType = MessageType.Corner
                        };
                        Ref.Settings.Placements[placement].GiveAll(giveInfo);
                    }
                });
            }
        }
    }
}
