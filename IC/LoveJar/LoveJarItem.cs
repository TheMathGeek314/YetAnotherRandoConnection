using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace YetAnotherRandoConnection {
    public class LoveJarItem: VoidItem {
        public LoveJarItem(LoveJarContents contents) {
            name = RandoInterop.GetLoveJarName(contents);
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_love_jar");
            AddTag<PersistentItemTag>().Persistence = Persistence.Persistent;
            UIDef = new MsgUIDef {
                name = new BoxedString(RandoInterop.clean(name)),
                shopDesc = new BoxedString("A necessity for any who wish to start their own collection"),
                sprite = new EmbeddedSprite("pin_love_jar")
            };
        }

        public override bool GiveEarly(string containerType) {
            return containerType.StartsWith(Consts.LoveJar);
        }

        public override string GetPreferredContainer() => name;
    }

    public enum LoveJarContents {
        Empty,
        Crawlid,
        Husk,
        LeapingHusk,
        Obble,
        Sentry,
        Vengefly
    }
}