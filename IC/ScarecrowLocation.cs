using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;
using Satchel;

namespace YetAnotherRandoConnection {
    public class ScarecrowLocation: AutoLocation {
        protected override void OnLoad() {
            On.PlayMakerFSM.OnEnable += EditScarecrow;
        }

        protected override void OnUnload() {
            On.PlayMakerFSM.OnEnable -= EditScarecrow;
        }

        private void EditScarecrow(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            if(self.gameObject.name == "Giant Hopper Summon" && self.FsmName == "Summon") {
                FsmState summonState = self.GetValidState("Summon");
                summonState.RemoveAction(0);
                summonState.InsertCustomAction(() => {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = self.gameObject.transform
                    };
                    Ref.Settings.Placements[Consts.Scarecrow].GiveAll(gi);
                }, 0);
            }
        }
    }
}