using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace YetAnotherRandoConnection {
    public class RequestModifier {
        public static void Hook() {
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyOrbDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyVineDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyJarDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyHivePlatDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyEggBombDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyTelescopeDef);
            RequestBuilder.OnUpdate.Subscribe(-499, SetupItems);
            RequestBuilder.OnUpdate.Subscribe(1200, RemoveRoots);
        }

        private static void AddAndEditLocation(RequestBuilder rb, string name, FlingType flingType, bool progressionPenalty, string scene) {
            rb.AddLocationByName(name);
            rb.EditLocationRequest(name, info => {
                info.customPlacementFetch = (factory, placement) => {
                    AbstractLocation absLoc = Finder.GetLocation(name);
                    absLoc.flingType = flingType;
                    AbstractPlacement ap = absLoc.Wrap();
                    factory.AddPlacement(ap);
                    return ap;
                };
                info.getLocationDef = () => new() {
                    Name = name,
                    FlexibleCount = false,
                    AdditionalProgressionPenalty = progressionPenalty,
                    SceneName = scene
                };
            });
        }

        public static void ApplyOrbDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                foreach((string area, string scene, int count) in Consts.RootCounts) {
                    for(int num = 1; num <= count; num++) {
                        string name = Consts.GetOrbNumName(area, num);
                        AddAndEditLocation(rb, name, FlingType.DirectDeposit, true, scene);
                    }
                }
            }
        }

        public static void ApplyVineDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Vines) {
                foreach(string vineName in Consts.VineNames) {
                    AddAndEditLocation(rb, vineName, FlingType.DirectDeposit, false, VineCoords.placementToPosition[vineName].Item1);
                }
                AddAndEditLocation(rb, Consts.Chain, FlingType.DirectDeposit, false, SceneNames.Ruins1_28);
            }
        }

        public static void ApplyJarDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.SoulJars) {
                foreach(string jarName in Consts.JarNames) {
                    AddAndEditLocation(rb, jarName, FlingType.Everywhere, false, JarCoords.placementToPosition[jarName].Item1);
                }
            }
        }

        public static void ApplyHivePlatDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.HivePlatforms) {
                foreach(string platName in Consts.HivePlatNames) {
                    AddAndEditLocation(rb, platName, FlingType.Everywhere, false, HivePlatCoords.placementToPosition[platName].Item1);
                }
            }
        }

        public static void ApplyEggBombDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.JellyEggBombs) {
                foreach(string bombName in Consts.EggBombNames) {
                    AddAndEditLocation(rb, bombName, FlingType.DirectDeposit, false, EggBombCoords.placementToPosition[bombName].Item1);
                }
            }
        }

        public static void ApplyTelescopeDef(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Telescope) {
                AddAndEditLocation(rb, Consts.Telescope, FlingType.DirectDeposit, false, SceneNames.Ruins2_Watcher_Room);
            }
        }

        private static void SetupItems(RequestBuilder rb) {
            if(!YetAnotherRandoConnection.Settings.Any)
                return;

            rb.EditItemRequest(Consts.EssenceOrb, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.EssenceOrb,
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                rb.AddItemByName(Consts.EssenceOrb, 482);
            }
            // If not randomized, we won't add these to vanilla since base rando also handles trees.

            foreach (string vine in Consts.VineNames)
            {
                rb.EditItemRequest(vine, info =>
                {
                    info.getItemDef = () => new ItemDef()
                    {
                        Name = vine,
                        MajorItem = false,
                        PriceCap = 50
                    };
                });
                if (YetAnotherRandoConnection.Settings.Vines)
                {
                    rb.AddItemByName(vine);
                }
                else
                {
                    // Since vines have logical effects, rando needs to place them in request if not randomized.
                    rb.AddToVanilla(vine, vine);
                }
            }
            rb.EditItemRequest(Consts.Chain, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.Chain,
                    MajorItem = false,
                    PriceCap = 50
                };
            });
            if (YetAnotherRandoConnection.Settings.Vines) {
                rb.AddItemByName(Consts.Chain);
            }

            rb.EditItemRequest(Consts.SoulJar, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = "Soul Refill",
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.SoulJars) {
                rb.AddItemByName(Consts.SoulJar, Consts.JarNames.Count);
            }

            rb.EditItemRequest(Consts.EggBomb, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.EggBomb,
                    MajorItem = false,
                    PriceCap = 100
                };
            });
            if(YetAnotherRandoConnection.Settings.JellyEggBombs) {
                rb.AddItemByName(Consts.EggBomb, Consts.EggBombNames.Count);
            }

            rb.EditItemRequest(Consts.Telescope, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.Telescope,
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.Telescope) {
                rb.AddItemByName(Consts.Telescope);
            }
        }

        private static void RemoveRoots(RequestBuilder rb) {
            if (YetAnotherRandoConnection.Settings.DreamOrbs)
            {
                rb.RemoveItemsWhere(item => item.StartsWith("Whispering_Root-"));
            }
        }
    }
}
