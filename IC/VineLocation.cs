using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MonoMod.Cil;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace YetAnotherRandoConnection {
    public class VineLocation: AutoLocation {
        private static readonly Dictionary<string, VineLocation> SubscribedLocations = new();
        static FieldInfo _vineCutActivated;
        static FieldInfo _vineCutAudioSource;
        private static Dictionary<string, Vector3> groundedPlatforms = new();
        private static List<string> fallingPlatforms = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookVines();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookVines();
        }

        private static void HookVines() {
            _vineCutActivated = typeof(VinePlatformCut).GetField("activated", BindingFlags.Instance | BindingFlags.NonPublic);
            _vineCutAudioSource = typeof(VinePlatformCut).GetField("audioSource", BindingFlags.Instance | BindingFlags.NonPublic);

            IL.VinePlatformCut.OnTriggerEnter2D += VineTrigger;
            On.VinePlatformCut.Awake += VineAwake;
            On.VinePlatform.Land += VinePlatformLand;
            On.VinePlatform.Awake += VinePlatformAwake;
        }

        private static void UnhookVines() {
            IL.VinePlatformCut.OnTriggerEnter2D -= VineTrigger; ;
            On.VinePlatformCut.Awake -= VineAwake;
            On.VinePlatform.Land -= VinePlatformLand;
            On.VinePlatform.Awake -= VinePlatformAwake;
        }

        private static void VineTrigger(ILContext il) {
            //if(YetAnotherRandoConnection.Settings.Vines) {
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
            //}
        }

        private static void VineAwake(On.VinePlatformCut.orig_Awake orig, VinePlatformCut self) {
            orig(self);
            //if(YetAnotherRandoConnection.Settings.Vines) {
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
            //}
        }

        //called if obtained in same room or on first entry
        private static void VinePlatformLand(On.VinePlatform.orig_Land orig, VinePlatform self) {
            orig(self);
            //if(YetAnotherRandoConnection.Settings.Vines) {
                string id = VineCoords.nameToPlacement[$"{self.gameObject.scene.name}/{self.gameObject.name}"];
                if(!groundedPlatforms.ContainsKey(id))
                    groundedPlatforms.Add(id, self.transform.position);
                else
                    groundedPlatforms[id] = self.transform.position;
                uncutVine(id, self);
                fallingPlatforms.Remove(id);
            //}
        }

        //should be called on second entry
        private static void VinePlatformAwake(On.VinePlatform.orig_Awake orig, VinePlatform self) {
            orig(self);
            //if(YetAnotherRandoConnection.Settings.Vines) {
                string id = VineCoords.nameToPlacement[$"{self.gameObject.scene.name}/{self.gameObject.name}"];
                if(!fallingPlatforms.Contains(id) && groundedPlatforms.ContainsKey(id)) {
                    uncutVine(id, self);
                }
            //}
        }

        private static async void uncutVine(string id, VinePlatform self) {
            for(int i = 0; i < 1; i++)
                await Task.Yield();
            if(!Ref.Settings.Placements[id].AllObtained()) {
                VinePlatformCut vine = self.gameObject.GetComponentInChildren<VinePlatformCut>(true);
                GameObject parent = vine.transform.parent.gameObject;
                parent.SetActive(true);
                parent.transform.position = groundedPlatforms[id];
                _vineCutActivated.SetValue(vine, false);
                Vector2 coords = VineCoords.placementToPosition[id].Item2;
                vine.transform.position = new Vector3(coords.x, coords.y, vine.transform.position.z);
                vine.body.isKinematic = true;
                vine.sprites.SetActive(true);
            }
        }
    }
}
