using System;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ItemChanger;
using ItemChanger.Internal;

namespace YetAnotherRandoConnection {
    public class CustomCheckEdits {
        public static void Hook() {
            IL.DreamPlantOrb.OnTriggerEnter2D += OrbTrigger;
        }

        private static void OrbTrigger(ILContext il) {
            //how do I counteract the +1 essence if ItemChanger hasn't already
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("DREAM ORB COLLECT"),
                            i => i.MatchCall<EventRegister>("SendEvent"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<DreamPlantOrb>>(j => { 
                if(YetAnotherRandoConnection.Settings.Orbs) {
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
