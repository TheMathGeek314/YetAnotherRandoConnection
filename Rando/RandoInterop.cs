using Modding;
using System.Collections.Generic;
using ConnectionMetadataInjector;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using System.Numerics;
using System.Reflection;
using System.Linq;
using System.IO;

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
                tag.Properties["PinSprite"] = new EmbeddedSprite(sprite);
                tag.Properties["WorldMapLocation"] = (scene, x, y);

                Finder.DefineCustomLocation(voidLoc);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            string jsonCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("DreamOrbCoords.json"));
            Stream coordsStream = assembly.GetManifestResourceStream(jsonCoords);
            foreach(JsonDreamOrbCoords jsonOrb in new ParseOrbJson(coordsStream).parseCoords())
                jsonOrb.translate();
            foreach((string area, string scene, int count) in Consts.RootCounts) {
                for(int i = 1; i <= count; i++) {
                    string name = $"DreamOrb_{area}_{i}";
                    Modding.Logger.Log("[YARC] - trying " + name);
                    Vector2 coords = DreamOrbCoords.data[name];
                    DefineLoc(name, scene, "pin_dream_tree", coords.X, coords.Y);
                }
            }
        }

        public static void DefineItems() {
            EssenceItem orbItem = EssenceItem.MakeEssenceItem(1);
            orbItem.name = Consts.EssenceOrb;
            AddTag(orbItem);
            Finder.DefineCustomItem(orbItem);
        }

        public static InteropTag AddTag(TaggableObject obj) {
            InteropTag tag = obj.GetOrAddTag<InteropTag>();
            tag.Message = SupplementalMetadata.InteropTagMessage;
            tag.Properties["ModSource"] = YetAnotherRandoConnection.instance.GetName();
            return tag;
        }
    }
}
