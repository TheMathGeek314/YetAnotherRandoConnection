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
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyScarecrowDef);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyStalactiteDef);
            RequestBuilder.OnUpdate.Subscribe(-499, SetupItems);
            RequestBuilder.OnUpdate.Subscribe(1200, RemoveRoots);
            RequestBuilder.OnUpdate.Subscribe(-499.5f, DefinePools);
        }

        private static void AddAndEditLocation(RequestBuilder rb, string name, FlingType flingType, bool progressionPenalty, string scene) {
            rb.AddLocationByName(name);
            rb.EditLocationRequest(name, info => {
                info.customPlacementFetch = (factory, placement) => {
                    if(factory.TryFetchPlacement(name, out AbstractPlacement ap1))
                        return ap1;
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

        public static void ApplyScarecrowDef(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Scarecrow) {
                AddAndEditLocation(rb, Consts.Scarecrow, FlingType.Everywhere, false, SceneNames.Deepnest_East_16);
            }
        }

        public static void ApplyStalactiteDef(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Stalactites) {
                foreach(string spikeName in Consts.StalactiteNames) {
                    AddAndEditLocation(rb, spikeName, FlingType.Everywhere, false, StalactiteCoords.placementToPosition[spikeName].Item1);
                }
            }
        }

        private static void SetupItems(RequestBuilder rb) {
            if(!YetAnotherRandoConnection.Settings.Any)
                return;

            rb.EditItemRequest(Consts.EssenceOrb, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.EssenceOrb,
                    Pool = PoolNames.Root,
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                rb.AddItemByName(Consts.EssenceOrb, 482);
                if(!rb.gs.PoolSettings.WhisperingRoots) {
                    foreach(string root in new string[] {
                        "Crossroads", "Greenpath", "Leg_Eater", "Mantis_Village",
                        "Deepnest", "Queens_Gardens", "Kingdoms_Edge", "Waterways",
                        "City", "Resting_Grounds", "Spirits_Glade", "Crystal_Peak",
                        "Howling_Cliffs", "Ancestral_Mound", "Hive"
                    }) {
                        rb.RemoveFromVanilla($"Whispering_Root-{root}");
                    }
                }
            }
            // If not randomized, we won't add these to vanilla since base rando also handles trees.

            foreach(string vine in Consts.VineNames) {
                rb.EditItemRequest(vine, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = vine,
                        Pool = "Vines",
                        MajorItem = false,
                        PriceCap = 50
                    };
                });
                if(YetAnotherRandoConnection.Settings.Vines) {
                    rb.AddItemByName(vine);
                }
                else {
                    // Since vines have logical effects, rando needs to place them in request if not randomized.
                    rb.AddToVanilla(vine, vine);
                }
            }
            rb.EditItemRequest(Consts.Chain, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.Chain,
                    Pool = "Vines",
                    MajorItem = false,
                    PriceCap = 50
                };
            });
            if(YetAnotherRandoConnection.Settings.Vines) {
                rb.AddItemByName(Consts.Chain);
            }

            rb.EditItemRequest(Consts.SoulJar, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = "Soul Refill",
                    Pool = PoolNames.Soul,
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
                    Pool = "EggBombs",
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
                    Pool = "Telescope",
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.Telescope) {
                rb.AddItemByName(Consts.Telescope);
            }

            rb.EditItemRequest(Consts.Scarecrow, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.Scarecrow,
                    Pool = "Scarecrow",
                    MajorItem = false,
                    PriceCap = 800
                };
            });
            if(YetAnotherRandoConnection.Settings.Scarecrow) {
                rb.AddItemByName(Consts.Scarecrow);
            }
        }

        private static void RemoveRoots(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                rb.RemoveItemsWhere(item => item.StartsWith("Whispering_Root-"));
                rb.RemoveLocationsWhere(loc => loc.StartsWith("Whispering_Root-"));
            }
        }

        private static void DefinePools(RequestBuilder rb) {
            GlobalSettings ys = YetAnotherRandoConnection.Settings;
            if(!ys.Any)
                return;
            if(rb.gs.SplitGroupSettings.RandomizeOnStart) {
                if(ys.VineGroup >= 0 && ys.VineGroup <= 2) {
                    ys.VineGroup = rb.rng.Next(3);
                }
                if(ys.HivePlatformGroup >= 0 && ys.HivePlatformGroup <= 2) {
                    ys.HivePlatformGroup = rb.rng.Next(3);
                }
                if(ys.EggBombGroup >= 0 && ys.EggBombGroup <= 2) {
                    ys.EggBombGroup = rb.rng.Next(3);
                }
                if(ys.TelescopeGroup >= 0 && ys.TelescopeGroup <= 2) {
                    ys.TelescopeGroup = rb.rng.Next(3);
                }
                if(ys.ScarecrowGroup >= 0 && ys.ScarecrowGroup <= 2) {
                    ys.ScarecrowGroup = rb.rng.Next(3);
                }
                if(ys.ScarecrowGroup >= 0 && ys.StalactiteGroup <= 2) {
                    ys.StalactiteGroup = rb.rng.Next(3);
                }
            }

            ItemGroupBuilder[] myGroups = [null, null, null, null, null, null];
            int[] groupSettings = [ys.VineGroup, ys.HivePlatformGroup, ys.EggBombGroup, ys.TelescopeGroup, ys.ScarecrowGroup, ys.StalactiteGroup];
            for(int i = 0; i < groupSettings.Length; i++) {
                if(groupSettings[i] > 0) {
                    string label = RBConsts.SplitGroupPrefix + groupSettings[i];
                    foreach(ItemGroupBuilder igb in rb.EnumerateItemGroups()) {
                        if(igb.label == label) {
                            myGroups[i] = igb;
                            break;
                        }
                    }
                    myGroups[i] ??= rb.MainItemStage.AddItemGroup(label);
                }
            }

            rb.OnGetGroupFor.Subscribe(0.01f, ResolveYarcGroup);
            bool ResolveYarcGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb) {
                if(type == RequestBuilder.ElementType.Item) {
                    if(item == Consts.EssenceOrb) {
                        gb = rb.GetGroupFor(ItemNames.Whispering_Root_Crossroads);
                        return true;
                    }
                    if(Consts.VineNames.Contains(item) || item == Consts.Chain) {
                        gb = myGroups[0];
                        return true;
                    }
                    if(item == Consts.SoulJar) {
                        gb = rb.GetGroupFor(ItemNames.Soul_Totem_A);
                        return true;
                    }
                    if(item == Consts.EggBomb) {
                        gb = myGroups[2];
                        return true;
                    }
                    if(item == Consts.Telescope) {
                        gb = myGroups[3];
                        return true;
                    }
                    if(item == Consts.Scarecrow) {
                        gb = myGroups[4];
                        return true;
                    }
                }
                else if(type == RequestBuilder.ElementType.Location) {
                    if(item.StartsWith("DreamOrb_")) {
                        gb = rb.GetGroupFor(ItemNames.Whispering_Root_Crossroads);
                        return true;
                    }
                    if(Consts.VineNames.Contains(item) || item == Consts.Chain) {
                        gb = myGroups[0];
                        return true;
                    }
                    if(Consts.JarNames.Contains(item)) {
                        gb = rb.GetGroupFor(ItemNames.Soul_Totem_A);
                        return true;
                    }
                    if(Consts.HivePlatNames.Contains(item)) {
                        gb = myGroups[1];
                        return true;
                    }
                    if(Consts.EggBombNames.Contains(item)) {
                        gb = myGroups[2];
                        return true;
                    }
                    if(item == Consts.Telescope) {
                        gb = myGroups[3];
                        return true;
                    }
                    if(item == Consts.Scarecrow) {
                        gb = myGroups[4];
                        return true;
                    }
                    if(Consts.StalactiteNames.Contains(item)) {
                        gb = myGroups[5];
                        return true;
                    }
                }
                gb = default;
                return false;
            }
        }
    }
}