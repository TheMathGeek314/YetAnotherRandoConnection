using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace YetAnotherRandoConnection {
    public class TelescopeItem: AbstractItem {
        public TelescopeItem() {
            name = Consts.Telescope;
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_telescope");
            UIDef = new MsgUIDef {
                name = new BoxedString(RandoInterop.clean(name)),
                shopDesc = new BoxedString("Watcher? I hardly know her!"),
                sprite = new EmbeddedSprite("pin_telescope")
            };
        }

        public override void GiveImmediate(GiveInfo info) {
            YARCModule.Instance.RecordItem(RandoInterop.clean(name));
        }
    }
}