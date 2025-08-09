using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Internal;
using ItemChanger.Locations;
using Satchel;

namespace YetAnotherRandoConnection {
    public class ChainLocation: AutoLocation {
        protected override void OnLoad() {
            On.PlayMakerFSM.OnEnable += EditChainFsm;
        }

        protected override void OnUnload() {
            On.PlayMakerFSM.OnEnable -= EditChainFsm;
        }

        private void EditChainFsm(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            if(self.gameObject.name == "Chain Platform" && self.FsmName == "Rope Plat") {
                //on awake, check if platform obtained
                FsmState init = self.GetValidState("Init");
                init.RemoveFirstActionOfType<BoolTest>();
                init.InsertCustomAction(() => {
                    if(obtainedChain) {
                        self.SendEvent("ACTIVATE");
                    }
                }, 6);

                //on pre-activate, don't remove chain
                FsmState activate = self.GetValidState("Activate");
                activate.Actions[5].Enabled = false;
                activate.InsertCustomAction(() => {
                    self.FsmVariables.GetFsmGameObject("Vine").Value.SetActive(!checkedChain);
                }, 6);

                //on awake, remove chain if checked
                init.InsertCustomAction(() => {
                    self.FsmVariables.GetFsmGameObject("Vine").Value.SetActive(!checkedChain);
                }, 7);

                //listen for cut after activation
                FsmUtil.AddTransition(activate, "FINISHED", "Idle");

                //grant check when cut
                FsmState grant = FsmUtil.AddState(self, "Grant");
                grant.AddCustomAction(() => {
                    GiveInfo gi = new() {
                        FlingType = FlingType.DirectDeposit,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner
                    };
                    Ref.Settings.Placements[Consts.Chain].GiveAll(gi);
                    self.FsmVariables.GetFsmGameObject("Vine").Value.SetActive(false);
                });
                FsmState idle = self.GetValidState("Idle");
                idle.ChangeTransition("CUT", "Grant");

                //listen for remote cut
                FsmEvent remoteCut = new FsmEvent("REMOTECUT");
                FsmState remoteState = FsmUtil.AddState(self, "Remote Cut");
                remoteState.AddCustomAction(() => {
                    self.gameObject.FindGameObjectInChildren("Vine").transform.SetParent(null);
                });
                FsmUtil.AddTransition(grant, "REMOTECUT", "Remote Cut");
                FsmUtil.AddTransition(idle, "REMOTECUT", "Remote Cut");
                FsmUtil.AddTransition(remoteState, "FINISHED", "PlayerData?");

                //gotta be able to cut the chain after remotely cutting the chain
                FsmState kinemetise = self.GetValidState("Kinemetise");
                FsmUtil.AddTransition(kinemetise, "BACKTOIDLE", "Idle");
                kinemetise.AddCustomAction(() => { self.SendEvent("BACKTOIDLE"); });
            }
        }

        private bool obtainedChain => RandomizerMod.RandomizerMod.RS.TrackerData.pm.Get(Consts.Chain) > 0;
        private bool checkedChain => Ref.Settings.Placements[Consts.Chain].AllObtained();
    }
}
