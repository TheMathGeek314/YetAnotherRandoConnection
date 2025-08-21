using Modding;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ConnectionMetadataInjector;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Modules;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using RandomizerMod.RC;

namespace YetAnotherRandoConnection {
    internal static class RandoInterop {
        public static void Hook() {
            RandoMenuPage.Hook();
            RequestModifier.Hook();
            LogicAdder.Hook();

            Container.DefineContainer<SoulJarContainer>();

            DefineLocations();
            DefineItems();

            RandoController.OnExportCompleted += EditModules;

            if(ModHooks.GetMod("CondensedSpoilerLogger") is Mod)
                CondensedSpoilerLogger.AddCategory("Useful Vines", (args) => true, Consts.UsefulVines);

            if(ModHooks.GetMod("RandoSettingsManager") is Mod)
                RSMInterop.Hook();

            if(ModHooks.GetMod("FStatsMod") is Mod)
                FStatsInterop.Hook();
        }

        private static void EditModules(RandoController controller) {
            if(!YetAnotherRandoConnection.Settings.Any)
                return;
            if(YetAnotherRandoConnection.Settings.Scarecrow)
                ItemChangerMod.Modules.Remove(ItemChangerMod.Modules.GetOrAdd<GreatHopperEasterEgg>());
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
            string platCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("HivePlatCoords.json"));
            string bombCoords = assembly.GetManifestResourceNames().Single(str => str.EndsWith("EggBombCoords.json"));

            using Stream orbStream = assembly.GetManifestResourceStream(orbCoords);
            using Stream vineStream = assembly.GetManifestResourceStream(vineCoords);
            using Stream jarStream = assembly.GetManifestResourceStream(jarCoords);
            using Stream platStream = assembly.GetManifestResourceStream(platCoords);
            using Stream bombStream = assembly.GetManifestResourceStream(bombCoords);

            foreach(JsonDreamOrbCoords jsonOrb in new ParseJson(orbStream).parseFile<JsonDreamOrbCoords>())
                jsonOrb.translate();
            foreach(JsonVineCoords jsonVine in new ParseJson(vineStream).parseFile<JsonVineCoords>())
                jsonVine.translate();
            foreach(JsonJarCoords jsonJar in new ParseJson(jarStream).parseFile<JsonJarCoords>())
                jsonJar.translate();
            foreach(JsonPlatCoords jsonPlat in new ParseJson(platStream).parseFile<JsonPlatCoords>())
                jsonPlat.translate();
            foreach(JsonBombCoords jsonBomb in new ParseJson(bombStream).parseFile<JsonBombCoords>())
                jsonBomb.translate();

            foreach((string area, string scene, int count) in Consts.RootCounts) {
                for(int i = 1; i <= count; i++) {
                    string name = Consts.GetOrbNumName(area, i);
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

            ChainLocation chainLoc = new() { name = Consts.Chain, sceneName = SceneNames.Ruins1_28 };
            DefineLoc(chainLoc, SceneNames.Ruins1_28, "pin_vine", 96.39f, 15.7f);

            foreach(string name in Consts.JarNames) {
                string scene = JarCoords.placementToPosition[name].Item1;
                Vector2 coords = JarCoords.placementToPosition[name].Item2;
                ObjectLocation objLoc = new() { name = name, objectName = JarCoords.placementToName[name], sceneName = scene, forceShiny = false, flingType = FlingType.Everywhere };
                DefineLoc(objLoc, scene, "pin_soul_jar", coords.x, coords.y);
            }

            foreach(string name in Consts.HivePlatNames) {
                string scene = HivePlatCoords.placementToPosition[name].Item1;
                Vector2 coords = HivePlatCoords.placementToPosition[name].Item2;
                HivePlatformLocation hpLoc = new() { name = name, sceneName = scene };
                DefineLoc(hpLoc, scene, "pin_hive_platform", coords.x, coords.y);
            }

            foreach(string name in Consts.EggBombNames) {
                string scene = EggBombCoords.placementToPosition[name].Item1;
                Vector2 coords = EggBombCoords.placementToPosition[name].Item2;
                JellyEggBombLocation bombLoc = new() { name = name, sceneName = scene };
                DefineLoc(bombLoc, scene, "pin_egg_bomb", coords.x, coords.y);
            }

            TelescopeLocation teleLoc = new() { name = Consts.Telescope, sceneName = SceneNames.Ruins2_Watcher_Room };
            DefineLoc(teleLoc, SceneNames.Ruins2_Watcher_Room, "pin_telescope", 21f, 137f);

            ScarecrowLocation scareLoc = new() { name = Consts.Scarecrow, sceneName = SceneNames.Deepnest_East_16 };
            DefineLoc(scareLoc, SceneNames.Deepnest_East_16, "pin_scarecrow", 30, 17);
        }

        public static void DefineItems() {
            DreamOrbItem orbItem = new();
            Finder.DefineCustomItem(orbItem);

            System.Random rand = new();
            foreach(string vName in Consts.VineNames) {
                VineItem vineItem = new(vName, PullRandomVine(rand));
                Finder.DefineCustomItem(vineItem);
            }

            ChainItem chainItem = new(PullRandomVine(rand));
            Finder.DefineCustomItem(chainItem);

            SoulJarItem jarItem = new();
            Finder.DefineCustomItem(jarItem);

            EggBombItem bombItem = new();
            Finder.DefineCustomItem(bombItem);

            TelescopeItem telescopeItem = new() { name = Consts.Telescope };
            InteropTag tag5 = AddTag(telescopeItem);
            tag5.Properties["PinSprite"] = new EmbeddedSprite("pin_telescope");
            telescopeItem.UIDef = new MsgUIDef {
                name = new BoxedString(clean(Consts.Telescope)),
                shopDesc = new BoxedString("Watcher? I hardly know her!"),
                sprite = new EmbeddedSprite("pin_telescope")
            };
            Finder.DefineCustomItem(telescopeItem);

            ScarecrowItem scarecrowItem = new();
            Finder.DefineCustomItem(scarecrowItem);
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

        public static string clean(string name) {
            return name.Replace("_", " ").Replace("-", " - ");
        }
    }
}