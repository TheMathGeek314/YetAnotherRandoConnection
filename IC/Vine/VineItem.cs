using UnityEngine;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace YetAnotherRandoConnection {
    public class VineItem: AbstractItem {
        public VineItem(string name, BoxedString description) {
            this.name = name;
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_vine");
            UIDef = new MsgUIDef {
                name = new BoxedString(RandoInterop.clean(name)),
                shopDesc = description,
                sprite = new EmbeddedSprite("pin_vine")
            };
        }

        public override void GiveImmediate(GiveInfo info) {
            if(GameManager.instance.sceneName == VineCoords.placementToPosition[name].Item1) {
                GameObject.Find(VineCoords.placementToName[name]).GetComponentInChildren<VinePlatformCut>().Cut();
            }
        }
    }
}