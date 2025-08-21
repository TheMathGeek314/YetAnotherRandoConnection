using UnityEngine;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using Satchel;
using Modding;

namespace YetAnotherRandoConnection {
    public class EggBombItem: AbstractItem {
        public EggBombItem() {
            name = Consts.EggBomb;
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_egg_bomb");
            AddTag<PersistentItemTag>().Persistence = Persistence.Persistent;
            UIDef = new MsgUIDef {
                name = new BoxedString(RandoInterop.clean(Consts.EggBomb)),
                shopDesc = new BoxedString("Plucked straight from an ooma nest, they make the spiciest of omelettes."),
                sprite = new EmbeddedSprite("pin_egg_bomb")
            };
        }

        public override void GiveImmediate(GiveInfo info)
        {
            GameObject explosion = GameManager.instance.gameObject.FindGameObjectInChildren("Gas Explosion Recycle M(Clone)");
            Vector3 position;
            if (info != null && info.Transform != null)
            {
                position = info.Transform.position;
            }
            else
            {
                position = HeroController.instance.transform.position;
            }
            GameObject.Instantiate(explosion, position, Quaternion.identity).SetActive(true);
            if (ModHooks.GetMod("FStatsMod") is Mod)
                YARCStats.Bombs++;
        }
    }
}
