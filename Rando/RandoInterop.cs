using Modding;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ConnectionMetadataInjector;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using Satchel;

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
                CondensedSpoilerLogger.AddCategory("Useful Vines", (args) => true, Consts.UsefulVines);
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
        }

        public static void DefineItems() {
            EssenceItem orbItem = EssenceItem.MakeEssenceItem(1);
            orbItem.name = Consts.EssenceOrb;
            InteropTag tag = AddTag(orbItem);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_dream_orb");
            Finder.DefineCustomItem(orbItem);

            System.Random rand = new();
            foreach(string vName in Consts.VineNames) {
                VoidItem vineItem = new() { name = vName };
                InteropTag tag1 = AddTag(vineItem);
                tag1.Properties["PinSprite"] = new EmbeddedSprite("pin_vine");
                vineItem.UIDef = new MsgUIDef {
                    name = new BoxedString(clean(vName)),
                    shopDesc = PullRandomVine(rand),
                    sprite = new EmbeddedSprite("pin_vine")
                };
                vineItem.OnGive += remoteVineCut;
                Finder.DefineCustomItem(vineItem);
            }

            VoidItem chainItem = new() { name = Consts.Chain };
            InteropTag tag2 = AddTag(chainItem);
            tag2.Properties["PinSprite"] = new EmbeddedSprite("pin_vine");
            chainItem.UIDef = new MsgUIDef {
                name = new BoxedString(clean(Consts.Chain)),
                shopDesc = PullRandomVine(rand),
                sprite = new EmbeddedSprite("pin_vine")
            };
            chainItem.OnGive += remoteChainCut;
            Finder.DefineCustomItem(chainItem);

            SoulJarItem jarItem = new() { name = Consts.SoulJar };
            InteropTag tag3 = AddTag(jarItem);
            tag3.Properties["PinSprite"] = new EmbeddedSprite("pin_soul_jar");
            jarItem.UIDef = new MsgUIDef {
                name = new BoxedString("Soul Refill"),
                shopDesc = new BoxedString("Will that be CACHE or CREDIT?"),
                sprite = new ItemChangerSprite("ShopIcons.Soul")
            };
            Finder.DefineCustomItem(jarItem);

            VoidItem bombItem = new() { name = Consts.EggBomb };
            InteropTag tag4 = AddTag(bombItem);
            tag4.Properties["PinSprite"] = new EmbeddedSprite("pin_egg_bomb");
            bombItem.AddTag<PersistentItemTag>().Persistence = Persistence.Persistent;
            bombItem.UIDef = new MsgUIDef {
                name = new BoxedString(clean(Consts.EggBomb)),
                shopDesc = new BoxedString("Plucked straight from an ooma nest, they make the spiciest of omelettes."),
                sprite = new EmbeddedSprite("pin_egg_bomb")
            };
            bombItem.OnGive += sploded;
            Finder.DefineCustomItem(bombItem);

            VoidItem telescopeItem = new() { name = Consts.Telescope };
            InteropTag tag5 = AddTag(telescopeItem);
            tag5.Properties["PinSprite"] = new EmbeddedSprite("pin_telescope");
            telescopeItem.UIDef = new MsgUIDef {
                name = new BoxedString(clean(Consts.Telescope)),
                shopDesc = new BoxedString("Watcher? I hardly know her!"),
                sprite = new EmbeddedSprite("pin_telescope")
            };
            Finder.DefineCustomItem(telescopeItem);
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

        private static void remoteChainCut(ReadOnlyGiveEventArgs args) {
            if(GameManager.instance.sceneName == SceneNames.Ruins1_28) {
                GameObject.Find("Chain Platform").GetComponent<PlayMakerFSM>().SendEvent("REMOTECUT");
            }
        }

        private static void sploded(ReadOnlyGiveEventArgs args) {
            GameObject explosion = GameManager.instance.gameObject.FindGameObjectInChildren("Gas Explosion Recycle M(Clone)");
            Vector3 position;
            if(args != null && args.Transform != null) {
                position = args.Transform.position;
            }
            else {
                if(EggBombCoords.placementToName.TryGetValue(args.Placement.Name, out string pos)) {
                    //mostly for vanilla egg locations
                    GameObject source = GameObject.Find(pos);
                    if(source != null) {
                        position = source.transform.position;
                    }
                    else {
                        position = HeroController.instance.transform.position;
                    }
                }
                else {
                    position = HeroController.instance.transform.position;
                }
            }
            GameObject.Instantiate(explosion, position, Quaternion.identity).SetActive(true);
        }

        private static string clean(string name) {
            return name.Replace("_", " ").Replace("-", " - ");
        }
    }
}
