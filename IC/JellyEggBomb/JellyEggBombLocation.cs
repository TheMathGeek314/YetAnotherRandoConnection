using System;
using System.Collections.Generic;
using MonoMod.Cil;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;
using Satchel;

namespace YetAnotherRandoConnection {
    public class JellyEggBombLocation: AutoLocation {
        private static readonly Dictionary<string, JellyEggBombLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookBombs();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookBombs();
        }

        private static void HookBombs() {
            IL.JellyEgg.Burst += Splode;
        }

        private static void UnhookBombs() {
            IL.JellyEgg.Burst -= Splode;
        }

        private static void Splode(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdfld<JellyEgg>("explosionObject"));
            cursor.RemoveRange(9);
            cursor.EmitDelegate<Action<JellyEgg>>(j => {
                string key = $"{j.gameObject.scene.name}/{j.gameObject.name}";
                GiveInfo giveInfo = new() {
                    FlingType = FlingType.DirectDeposit,
                    Container = Container.Unknown,
                    MessageType = MessageType.Corner,
                    Transform = j.transform
                };
                if(EggBombCoords.nameToPlacement.TryGetValue(key, out string placement)) {
                    Ref.Settings.Placements[placement].GiveAll(giveInfo);
                }
                else {
                    GameObject explosion = GameManager.instance.gameObject.FindGameObjectInChildren("Gas Explosion Recycle M(Clone)");
                    GameObject.Instantiate(explosion, j.transform.position, Quaternion.identity).SetActive(true);
                }
            });
        }
    }
}