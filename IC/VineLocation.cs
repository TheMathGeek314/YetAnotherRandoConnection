using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MonoMod.Cil;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;
using Satchel;

namespace YetAnotherRandoConnection {
    public class VineLocation: AutoLocation {
        private static readonly Dictionary<string, VineLocation> SubscribedLocations = new();
        static FieldInfo _vineCutActivated;
        static FieldInfo _vineCutAudioSource;

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
            IL.VinePlatformCut.OnTriggerEnter2D -= VineTrigger;
            On.VinePlatformCut.Awake -= VineAwake;
            On.VinePlatform.Land -= VinePlatformLand;
            On.VinePlatform.Awake -= VinePlatformAwake;
        }

        private static void VineTrigger(ILContext il) {
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
                string key = getIdName(j, true);
                AbstractPlacement ap = Ref.Settings.Placements[VineCoords.nameToPlacement[key]];
                GiveInfo gi = new() {
                    FlingType = FlingType.StraightUp,
                    Container = Container.Unknown,
                    MessageType = MessageType.Corner,
                    Transform = j.transform
                };
                ap.GiveAll(gi);
            });
        }

        private static void VineAwake(On.VinePlatformCut.orig_Awake orig, VinePlatformCut self) {
            orig(self);
            string id = getIdName(self, true);
            string placement = VineCoords.nameToPlacement[id];
            if(RandomizerMod.RandomizerMod.RS.TrackerData.pm.Get(placement) > 0) {
                self.Cut();
            }
            else if(Ref.Settings.Placements[placement].AllObtained()) {
                _vineCutActivated.SetValue(self, true);
                self.sprites.SetActive(false);
            }
        }

        private static void VinePlatformLand(On.VinePlatform.orig_Land orig, VinePlatform self) {
            orig(self);
            string id = VineCoords.nameToPlacement[getIdName(self)];
            if(!Ref.Settings.Placements[id].AllObtained()) {
                VinePlatformCut vine = self.gameObject.GetComponentInChildren<VinePlatformCut>(true);
                GameObject parent = vine.transform.parent.gameObject;
                parent.SetActive(true);
                Vector2 uncutPosition = VineCoords.placementToPosition[id].Item2;
                _vineCutActivated.SetValue(vine, false);
                vine.transform.position = uncutPosition;
                vine.body.isKinematic = true;
                vine.body.velocity = Vector3.zero;
                vine.sprites.SetActive(true);
            }
        }

        private static void VinePlatformAwake(On.VinePlatform.orig_Awake orig, VinePlatform self) {
            orig(self);
            platformAfterOneFrame(self);
        }

        private static async void platformAfterOneFrame(VinePlatform self) {
            await Task.Yield();
            string id = VineCoords.nameToPlacement[getIdName(self)];
            if(self.activatedSprite.activeSelf) {
                GameObject platSprite = self.platformSprite;
                VinePlatformCut vine = platSprite.GetComponentInChildren<VinePlatformCut>();
                if(!Ref.Settings.Placements[id].AllObtained()) {
                    platSprite.SetActive(true);
                    platSprite.FindGameObjectInChildren("Sprites").SetActive(true);
                    platSprite.GetComponent<SpriteRenderer>().enabled = false;
                    _vineCutActivated.SetValue(vine, false);
                }
                Vector3 platSpritePosition = platSprite.transform.position;
                Vector3 activePosition = self.activatedSprite.transform.position;
                self.transform.position = activePosition;
                self.activatedSprite.transform.position = activePosition;
                platSprite.transform.position = platSpritePosition;
                vine.body.isKinematic = true;
                vine.body.velocity = Vector2.zero;
                await Task.Yield();
                self.collider.enabled = true;
            }
        }

        public static string getIdName(MonoBehaviour self, bool useGrandparent = false) {
            return $"{self.gameObject.scene.name}/{(useGrandparent ? self.transform.parent.parent.name : self.gameObject.name)}";
        }
    }
}