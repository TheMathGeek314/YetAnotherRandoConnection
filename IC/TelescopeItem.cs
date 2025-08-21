using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using Modding;

namespace YetAnotherRandoConnection {
    public class TelescopeItem: AbstractItem {
        public TelescopeItem() {
            name = Consts.Telescope;
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_telescope");
            UIDef = new MsgUIDef {
                name = new BoxedString(name.Replace("_", " ")),
                shopDesc = new BoxedString("Watcher? I hardly know her!"),
                sprite = new EmbeddedSprite("pin_telescope")
            };
        }

        public override void GiveImmediate(GiveInfo info) {
            if (ModHooks.GetMod("FStatsMod") is Mod)
                YARCStats.AddEntry(name.Replace("_", " "));
        }
    }
}
