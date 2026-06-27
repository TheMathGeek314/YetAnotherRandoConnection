using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace YetAnotherRandoConnection {
    public class DreamOrbItem: EssenceItem {
        public DreamOrbItem() {
            name = Consts.EssenceOrb;
            amount = 1;
            UIDef = new MsgUIDef {
                name = new FormattedLanguageString("ESSENCE", amount),
                shopDesc = new LanguageString("UI", "ITEMCHANGER_DESC_ESSENCE"),
                sprite = new ItemChangerSprite("ShopIcons.Essence"),
            };
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_dream_orb");
        }

        public override void GiveImmediate(GiveInfo info) {
            base.GiveImmediate(info);
            YARCModule.Instance.Orbs++;
        }
    }
}