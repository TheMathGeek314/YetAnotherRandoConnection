using Modding;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
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
                    DefineLoc(name, scene, "pin_dream_orb", coords.x, coords.y);
                }
            }
            foreach(string name in Consts.VineNames) {
                string scene = VineCoords.placementToPosition[name].Item1;
                Vector2 coords = VineCoords.placementToPosition[name].Item2;
                DefineLoc(name, scene, "pin_vine", coords.x, coords.y);
            }
            DefineLoc("Chain-Storerooms", SceneNames.Ruins1_28, "pin_vine", 96.39f, 15.7f);
        }

        public static void DefineItems() {
            EssenceItem orbItem = EssenceItem.MakeEssenceItem(1);
            orbItem.name = Consts.EssenceOrb;
            AddTag(orbItem);
            Finder.DefineCustomItem(orbItem);

            System.Random rand = new();
            foreach(string vName in Consts.VineNames) {
                VoidItem vineItem = new() { name = vName };
                InteropTag tag = AddTag(vineItem);
                tag.Properties["PinSprite"] = new EmbeddedSprite("pin_vine");
                vineItem.UIDef = new MsgUIDef {
                    name = new BoxedString(vName.Replace("_", " ").Replace("-", " - ")),
                    shopDesc = PullRandomVine(rand),
                    sprite = new EmbeddedSprite("pin_vine")
                };
                vineItem.OnGive += remoteVineCut;
                Finder.DefineCustomItem(vineItem);
            }
        }

        public static InteropTag AddTag(TaggableObject obj) {
            InteropTag tag = obj.GetOrAddTag<InteropTag>();
            tag.Message = SupplementalMetadata.InteropTagMessage;
            tag.Properties["ModSource"] = YetAnotherRandoConnection.instance.GetName();
            return tag;
        }

        private static BoxedString PullRandomVine(System.Random rand) {
            string description = Consts.VineDescriptions[rand.Next(Consts.VineDescriptions.Count)];
            return new BoxedString(description);
        }

        private static void remoteVineCut(ReadOnlyGiveEventArgs args) {
            string name = args.Item.name;
            if(GameManager.instance.sceneName == VineCoords.placementToPosition[name].Item1) {
                GameObject.Find(VineCoords.placementToName[name]).GetComponentInChildren<VinePlatformCut>().Cut();
            }
        }
    }
}
