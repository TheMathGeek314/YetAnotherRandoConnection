using Modding;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using ConnectionMetadataInjector;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace YetAnotherRandoConnection {
    internal static class RandoInterop {
        public static void Hook() {
            RandoMenuPage.Hook();
            RequestModifier.Hook();
            LogicAdder.Hook();

            //Container.DefineContainer<>();

            DefineLocations();
            DefineItems();

            if(ModHooks.GetMod("CondensedSpoilerLogger") is Mod) {
                //todo
            }

            if(ModHooks.GetMod("RandoSettingsManager") is Mod) {
                RSMInterop.Hook();
            }
        }

        public static void DefineLocations() {
            static void DefineLoc(string name, string scene, string sprite, float x, float y) {
                VoidLocation voidLoc = new() { name = name, sceneName = scene };
                InteropTag tag = AddTag(voidLoc);
                if(sprite != "pin_todo")
                    tag.Properties["PinSprite"] = new EmbeddedSprite(sprite);
                tag.Properties["WorldMapLocation"] = (scene, x, y);

                Finder.DefineCustomLocation(voidLoc);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();

            string jsonCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("DreamOrbCoords.json"));
            string vineCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("VineCoords.json"));

            using Stream coordsStream = assembly.GetManifestResourceStream(jsonCoords);
            using Stream vineStream = assembly.GetManifestResourceStream(vineCoords);

            foreach(JsonDreamOrbCoords jsonOrb in new ParseJson(coordsStream).parseFile<JsonDreamOrbCoords>())
                jsonOrb.translate();
            foreach(JsonVineCoords jsonVine in new ParseJson(vineStream).parseFile<JsonVineCoords>())
                jsonVine.translate();

            foreach((string area, string scene, int count) in Consts.RootCounts) {
                for(int i = 1; i <= count; i++) {
                    string name = $"DreamOrb_{area}_{i}";
                    Vector2 coords = DreamOrbCoords.data[name];
                    DefineLoc(name, scene, "pin_dream_tree", coords.X, coords.Y);
                }
            }
            foreach(string name in Consts.VineNames) {
                string scene = VineCoords.placementToPosition[name].Item1;
                Vector2 coords = VineCoords.placementToPosition[name].Item2;
                DefineLoc(name, scene, "pin_todo", coords.X, coords.Y);
            }
            DefineLoc("Chain-Storerooms", SceneNames.Ruins1_28, "pin_todo", 96.39f, 15.7f);
        }

        public static void DefineItems() {
            EssenceItem orbItem = EssenceItem.MakeEssenceItem(1);
            orbItem.name = Consts.EssenceOrb;
            AddTag(orbItem);
            Finder.DefineCustomItem(orbItem);

            foreach(string vName in Consts.VineNames) {
                VoidItem vineItem = new() { name = vName };
                InteropTag tag = AddTag(vineItem);
                //tag.Properties["PinSprite"] = new EmbeddedSprite("pin_todo");
                vineItem.UIDef = new MsgUIDef {
                    name = new BoxedString(vName),
                    shopDesc = new BoxedString("Do it for the vine!"),
                    sprite = new EmbeddedSprite("pin_dream_tree")//todo
                };
                Finder.DefineCustomItem(vineItem);
            }
        }

        public static InteropTag AddTag(TaggableObject obj) {
            InteropTag tag = obj.GetOrAddTag<InteropTag>();
            tag.Message = SupplementalMetadata.InteropTagMessage;
            tag.Properties["ModSource"] = YetAnotherRandoConnection.instance.GetName();
            return tag;
        }
    }
}
