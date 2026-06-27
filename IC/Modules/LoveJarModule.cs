using UnityEngine;
using ItemChanger;
using ItemChanger.Modules;

namespace YetAnotherRandoConnection {
    public class LoveJarModule: Module {
        public override void Initialize() {
            On.GameManager.OnNextLevelReady += sceneLoad;
        }

        public override void Unload() {
            On.GameManager.OnNextLevelReady -= sceneLoad;
        }

        private void sceneLoad(On.GameManager.orig_OnNextLevelReady orig, GameManager self) {
            orig(self);
            if(self.sceneName == SceneNames.Ruins2_11) {
                foreach(string name in new string[] { "death0001", "death0004", "death0008", "goomba_death0006" }) {
                    GameObject.Find(name).SetActive(false);
                }
            }
        }
    }
}
