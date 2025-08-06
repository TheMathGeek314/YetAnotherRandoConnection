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
using ItemChanger.Locations;

namespace YetAnotherRandoConnection {
    internal static class RandoInterop {
        public static void Hook() {
            RandoMenuPage.Hook();
            RequestModifier.Hook();
            LogicAdder.Hook();

            Container.DefineContainer<SoulJarContainer>();

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
            static void DefineLoc(AbstractLocation loc, string scene, string sprite, float x, float y) {
                InteropTag tag = AddTag(loc);
                tag.Properties["PinSprite"] = new EmbeddedSprite(sprite);
                tag.Properties["WorldMapLocation"] = (scene, x, y);
                Finder.DefineCustomLocation(loc);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();

            string orbCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("DreamOrbCoords.json"));
            string vineCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("VineCoords.json"));
            string jarCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("JarCoords.json"));

            using Stream orbStream = assembly.GetManifestResourceStream(orbCoords);
            using Stream vineStream = assembly.GetManifestResourceStream(vineCoords);
            using Stream jarStream = assembly.GetManifestResourceStream(jarCoords);

            foreach(JsonDreamOrbCoords jsonOrb in new ParseJson(orbStream).parseFile<JsonDreamOrbCoords>())
                jsonOrb.translate();
            foreach(JsonVineCoords jsonVine in new ParseJson(vineStream).parseFile<JsonVineCoords>())
                jsonVine.translate();
            foreach(JsonJarCoords jsonJar in new ParseJson(jarStream).parseFile<JsonJarCoords>())
                jsonJar.translate();

            foreach((string area, string scene, int count) in Consts.RootCounts) {
                for(int i = 1; i <= count; i++) {
                    string name = Consts.GetOrbNumName(area, i);
                    Modding.Logger.Log($"[YARC] - ({area}, {i}) => DefineLoc({name})");
                    Vector2 coords = DreamOrbCoords.data[name];
                    DreamOrbLocation orbLoc = new() { name = name, sceneName = scene };
                    DefineLoc(orbLoc, scene, "pin_dream_orb", coords.x, coords.y);
                }
            }
            foreach(string name in Consts.VineNames) {
                string scene = VineCoords.placementToPosition[name].Item1;
                Vector2 coords = VineCoords.placementToPosition[name].Item2;
                VineLocation vineLoc = new() { name = name, sceneName = scene };
                DefineLoc(vineLoc, scene, "pin_vine", coords.x, coords.y);
            }
            //SomethingLocation chainLoc = new() { name = Consts.Chain, sceneName = SceneNames.Ruins1_28 };
            //DefineLoc(chainLoc, SceneNames.Ruins1_28, "pin_vine", 96.39f, 15.7f);

            foreach(string name in Consts.JarNames) {
                string scene = JarCoords.placementToPosition[name].Item1;
                Vector2 coords = JarCoords.placementToPosition[name].Item2;
                ObjectLocation objLoc = new() { name = name, objectName = JarCoords.placementToName[name], sceneName = scene, forceShiny = false, flingType = FlingType.Everywhere };
                DefineLoc(objLoc, scene, "pin_soul_jar", coords.x, coords.y);
            }
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
                    name = new BoxedString(clean(vName)),
                    shopDesc = PullRandomVine(rand),
                    sprite = new EmbeddedSprite("pin_vine")
                };
                vineItem.OnGive += remoteVineCut;
                Finder.DefineCustomItem(vineItem);
            }
            //define chain item

            SoulJarItem jarItem = new() { name = Consts.SoulJar };
            InteropTag tag2 = AddTag(jarItem);
            tag2.Properties["PinSprite"] = new EmbeddedSprite("pin_soul_jar");
            jarItem.UIDef = new MsgUIDef {
                name = new BoxedString(clean(Consts.SoulJar)),
                shopDesc = new BoxedString("idk soul juice I guess"),
                sprite = new EmbeddedSprite("pin_soul_jar")
            };
            Finder.DefineCustomItem(jarItem);
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

        private static string clean(string name) {
            return name.Replace("_", " ").Replace("-", " - ");
        }
    }
}
