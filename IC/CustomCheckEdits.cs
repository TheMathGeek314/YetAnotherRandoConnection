using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YetAnotherRandoConnection {
    public class CustomCheckEdits {
        static FieldInfo _vineCutActivated;
        static FieldInfo _vineCutAudioSource;
        private static Dictionary<string, Vector3> groundedPlatforms = new();
        private static List<string> fallingPlatforms = new();

        public static void Hook() {
            IL.DreamPlantOrb.OnTriggerEnter2D += OrbTrigger;
            IL.VinePlatformCut.OnTriggerEnter2D += VineTrigger;
            On.VinePlatformCut.Awake += VineAwake;
            On.VinePlatform.Land += VinePlatformLand;
            On.VinePlatform.Awake += VinePlatformAwake;

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
                        Modding.Logger.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
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
                    if(j.cutParticles)
                        j.cutParticles.SetActive(true);
                    AudioSource audioSource = _vineCutAudioSource.GetValue(j) as AudioSource;
                    if(audioSource && j.cutSound)
                        audioSource.PlayOneShot(j.cutSound);
                    j.Disable(false);

                    //grant rando check
                    string key = $"{j.gameObject.scene.name}/{j.transform.parent.parent.name}";
                    AbstractPlacement ap = Ref.Settings.Placements[VineCoords.nameToPlacement[key]];
                    GiveInfo gi = new() {
                        FlingType = FlingType.StraightUp,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner
                    };
                    ap.GiveAll(gi);
                });
            }
        }

        private static void VineAwake(On.VinePlatformCut.orig_Awake orig, VinePlatformCut self) {
            orig(self);
            if(YetAnotherRandoConnection.Settings.Vines) {
                string id = $"{self.gameObject.scene.name}/{self.transform.parent.parent.name}";
                string placement = VineCoords.nameToPlacement[id];
                if(RandomizerMod.RandomizerMod.RS.TrackerData.pm.Get(placement) > 0) {
                    self.Cut();
                    fallingPlatforms.Add(id);
                }
                if(Ref.Settings.Placements[placement].AllObtained()) {
                    _vineCutActivated.SetValue(self, true);
                    self.sprites.SetActive(false);
                }
            }
        }

        //called if obtained in same room or on first entry
        private static void VinePlatformLand(On.VinePlatform.orig_Land orig, VinePlatform self) {
            orig(self);
            if(YetAnotherRandoConnection.Settings.Vines) {
                string id = VineCoords.nameToPlacement[$"{self.gameObject.scene.name}/{self.gameObject.name}"];
                if(!groundedPlatforms.ContainsKey(id))
                    groundedPlatforms.Add(id, self.transform.position);
                else
                    groundedPlatforms[id] = self.transform.position;
                uncutVine(id, self);
                fallingPlatforms.Remove(id);
            }
        }

        //should be called on second entry
        private static void VinePlatformAwake(On.VinePlatform.orig_Awake orig, VinePlatform self) {
            orig(self);
            if(YetAnotherRandoConnection.Settings.Vines) {
                string id = VineCoords.nameToPlacement[$"{self.gameObject.scene.name}/{self.gameObject.name}"];
                if(!fallingPlatforms.Contains(id) && groundedPlatforms.ContainsKey(id)) {
                    uncutVine(id, self);
                }
            }
        }

        private static async void uncutVine(string id, VinePlatform self) {
            for(int i = 0; i < 1; i++)
                await Task.Yield();
            if(!Ref.Settings.Placements[id].AllObtained()) {
                VinePlatformCut vine = self.gameObject.GetComponentInChildren<VinePlatformCut>(true);
                GameObject parent = vine.transform.parent.gameObject;
                vine.transform.parent.gameObject.SetActive(true);
                vine.transform.parent.gameObject.transform.position = groundedPlatforms[id];
                _vineCutActivated.SetValue(vine, false);
                Vector2 coords = VineCoords.placementToPosition[id].Item2;
                vine.transform.position = new Vector3(coords.x, coords.y, vine.transform.position.z);
                vine.body.isKinematic = true;
                vine.sprites.SetActive(true);
            }
        }
    }
}
