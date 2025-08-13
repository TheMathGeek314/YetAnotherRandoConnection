using UnityEngine;
using ItemChanger;

namespace YetAnotherRandoConnection {
    public class ChainItem: VineItem {
        public ChainItem(BoxedString description) : base(Consts.Chain, description) { }

        public override void GiveImmediate(GiveInfo info) {
            if(GameManager.instance.sceneName == SceneNames.Ruins1_28) {
                GameObject.Find("Chain Platform").GetComponent<PlayMakerFSM>().SendEvent("REMOTECUT");
            }
        }
    }
}
