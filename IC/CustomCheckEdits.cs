using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;

namespace YetAnotherRandoConnection {
    public class CustomCheckEdits {
        static FieldInfo _vineCutActivated;
        static FieldInfo _vineCutAudioSource;

        public static void Hook() {
            IL.DreamPlantOrb.OnTriggerEnter2D += OrbTrigger;
            IL.VinePlatformCut.OnTriggerEnter2D += VineTrigger;

            _vineCutActivated = typeof(VinePlatformCut).GetField("activated", BindingFlags.Instance | BindingFlags.NonPublic);
            _vineCutAudioSource = typeof(VinePlatformCut).GetField("audioSource", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private static void OrbTrigger(ILContext il) {
            //how do I counteract the +1 essence if ItemChanger hasn't already
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

        private static void VineTrigger(ILContext il) {
            if(YetAnotherRandoConnection.Settings.Vines) {
                ILCursor cursor = new ILCursor(il).Goto(0);
                cursor.GotoNext(i => i.MatchCallvirt<VinePlatformCut>("Cut"));
                cursor.Remove();
                cursor.EmitDelegate<Action<VinePlatformCut>>(j => {
                    //imitate cutting
                    _vineCutActivated.SetValue(j, true);
                    if(j.body)
                        j.body.isKinematic = false;
                    if(j.cutParticles)
                        j.cutParticles.SetActive(true);
                    AudioSource audioSource = _vineCutAudioSource.GetValue(j) as AudioSource;
                    if(audioSource && j.cutSound)
                        audioSource.PlayOneShot(j.cutSound);
                    j.Disable(false);

                    //grant rando check
                    string key = $"{j.gameObject.scene.name}/{j.transform.parent.parent.name}";
                    Modding.Logger.Log("Cut vine " + key);
                    AbstractPlacement ap = Ref.Settings.Placements[VineCoords.nameToPlacement[key]];
                    GiveInfo gi = new() {
                        FlingType = FlingType.DirectDeposit,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner
                    };
                    ap.GiveAll(gi);
                });
            }
        }
    }
}
