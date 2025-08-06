using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace YetAnotherRandoConnection {
    public class DreamOrbLocation: AutoLocation {
        private static readonly Dictionary<string, DreamOrbLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookOrbs();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookOrbs();
        }

        private static void HookOrbs() {
            IL.DreamPlantOrb.OnTriggerEnter2D += OrbTrigger;
        }

        private static void UnhookOrbs() {
            IL.DreamPlantOrb.OnTriggerEnter2D -= OrbTrigger;
        }

        private static void OrbTrigger(ILContext il) {
            //remove essence if roots are disabled??
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("DREAM ORB COLLECT"),
                            i => i.MatchCall<EventRegister>("SendEvent"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<DreamPlantOrb>>(j => {
                if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                    string name = j.gameObject.name;
                    int num = -1;
                    if(name == "Dream Plant Orb") {
                        num = 1;
                    }
                    else {
                        Match match = Regex.Match(name, @"\((\d+)\)");
                        if(match.Success) {
                            num = int.Parse(match.Groups[1].Value) + 1;
                        }
                    }
                    if(num == -1)
                        return;
                    string placementName = $"DreamOrb_{Consts.OrbAreas[j.gameObject.scene.name]}_{num}";
                    AbstractPlacement ap = Ref.Settings.Placements[placementName];
                    GiveInfo gi = new() {
                        FlingType = FlingType.DirectDeposit,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner
                    };
                    ap.GiveAll(gi);
                }
            });
        }
    }
}
