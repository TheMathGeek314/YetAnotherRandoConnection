using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;
using ItemChanger.Util;

namespace YetAnotherRandoConnection {
    public class DreamOrbLocation: AutoLocation {
        private static readonly Dictionary<string, DreamOrbLocation> SubscribedLocations = new();

        private static FieldInfo _pickedUp = typeof(DreamPlantOrb).GetField("pickedUp", BindingFlags.NonPublic | BindingFlags.Instance);

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
            IL.DreamPlantOrb.OnTriggerEnter2D += OrbGrant;
            On.DreamPlantOrb.Show += OrbShow;
            On.DreamPlant.Awake += PlantAwake;
            IL.DreamPlantOrb.OnTriggerEnter2D += RemoveEssence;
        }

        private static void UnhookOrbs() {
            IL.DreamPlantOrb.OnTriggerEnter2D -= OrbGrant;
            On.DreamPlantOrb.Show -= OrbShow;
            On.DreamPlant.Awake -= PlantAwake;
            IL.DreamPlantOrb.OnTriggerEnter2D -= RemoveEssence;
        }

        private static void OrbGrant(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("DREAM ORB COLLECT"),
                            i => i.MatchCall<EventRegister>("SendEvent"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<DreamPlantOrb>>(j => {
                if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                    string placementName = orbNameToPlacement(j.gameObject.name, j.gameObject.scene.name);
                    if(string.IsNullOrEmpty(placementName))
                        return;
                    AbstractPlacement ap = Ref.Settings.Placements[placementName];
                    GiveInfo gi = new() {
                        FlingType = FlingType.DirectDeposit,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = j.transform
                    };
                    ap.GiveAll(gi);
                }
            });
        }

        private static void OrbShow(On.DreamPlantOrb.orig_Show orig, DreamPlantOrb self) {
            string placementName = orbNameToPlacement(self.gameObject.name, self.gameObject.scene.name);
            if(!string.IsNullOrEmpty(placementName)) {
                if(!Ref.Settings.Placements[placementName].AllObtained()) {
                    _pickedUp.SetValue(self, false);
                }
            }
            orig(self);
        }

        private static IEnumerator OrbStart(On.DreamPlant.orig_CheckOrbs orig, DreamPlant self) {
            foreach((string area, string room, int count) in Consts.RootCounts) {
                if(self.gameObject.scene.name != room)
                    continue;
                for(int i = 1; i <= count; i++) {
                    string name = Consts.GetOrbNumName(area, i);
                    if(!Ref.Settings.Placements[name].AllObtained()) {
                        self.SetCompleted(false);
                        self.SetActivated(false);
                        break;
                    }
                }
            }
            yield return orig(self);
        }

        private static void PlantAwake(On.DreamPlant.orig_Awake orig, DreamPlant self) {
            self.SetSpriteFlash(self.GetComponent<SpriteFlash>());
            self.SetAudioSource(self.GetComponent<AudioSource>());
            self.SetAnim(self.GetComponent<tk2dSpriteAnimator>());
        }

        private static void RemoveEssence(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            if(cursor.TryGotoNext(i => i.MatchCall<GameManager>("get_instance"),
                                  i => i.MatchLdstr(nameof(PlayerData.dreamOrbs)),
                                  i => i.MatchCallvirt<GameManager>(nameof(GameManager.IncrementPlayerDataInt)))) {
                cursor.RemoveRange(3);
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate<Action<DreamPlantOrb>>(orb => {
                    if(!SubscribedLocations.ContainsKey(orb.gameObject.scene.name)) {
                        GameManager.instance.IncrementPlayerDataInt(nameof(PlayerData.dreamOrbs));
                    }
                });
            }
        }

        private static string orbNameToPlacement(string objectName, string sceneName) {
            int num = -1;
            if(objectName == "Dream Plant Orb")
                num = 1;
            else {
                Match match = Regex.Match(objectName, @"\((\d+)\)");
                if(match.Success) {
                    num = int.Parse(match.Groups[1].Value) + 1;
                }
            }
            if(num == -1)
                return null;
            return $"DreamOrb_{Consts.OrbAreas[sceneName]}_{num}";
        }
    }
}