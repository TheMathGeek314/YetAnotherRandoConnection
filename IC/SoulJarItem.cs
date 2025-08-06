using ItemChanger;
using ItemChanger.Items;
using ItemChanger.UIDefs;

namespace YetAnotherRandoConnection {
    public class SoulJarItem: SoulTotemItem {
        public SoulJarItem() {
            name = Consts.SoulJar;
            soul = 22;
            hitCount = 1;
            soulTotemSubtype = SoulTotemSubtype.B;
            UIDef = new MsgUIDef() {
                name = new BoxedString("Soul Refill"),
                shopDesc = new BoxedString("idk lorem ipsum yahoo.com or something"),
                sprite = new ItemChangerSprite("ShopIcons.Soul")
            };
        }

        public override string GetPreferredContainer() => Consts.SoulJar;

        /*public override void GiveImmediate(GiveInfo info) {
            if(info.Container != Consts.SoulJar) {
                base.GiveImmediate(info);
            }
        }*/
    }
}
