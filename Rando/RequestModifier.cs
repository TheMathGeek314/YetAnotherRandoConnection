using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace YetAnotherRandoConnection {
    public class RequestModifier {
        public static void Hook() {
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyOrbDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyVineDefs);
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyJarDefs);
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
                        /*rb.AddLocationByName(name);
                        rb.EditLocationRequest(name, info => {
                            info.customPlacementFetch = (factory, placement) => {
                                AbstractLocation orbLoc = Finder.GetLocation(name);
                                orbLoc.flingType = FlingType.DirectDeposit;
                                AbstractPlacement ap = orbLoc.Wrap();
                                factory.AddPlacement(ap);
                                return ap;
                            };
                            info.getLocationDef = () => new() {
                                Name = name,
                                FlexibleCount = false,
                                AdditionalProgressionPenalty = true,
                                SceneName = scene
                            };
                        });*/
                    }
                }
            }
        }

        public static void ApplyVineDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Vines) {
                foreach(string vineName in Consts.VineNames) {
                    AddAndEditLocation(rb, vineName, FlingType.DirectDeposit, false, VineCoords.placementToPosition[vineName].Item1);
                    /*rb.AddLocationByName(vineName);
                    rb.EditLocationRequest(vineName, info => {
                        info.customPlacementFetch = (factory, placement) => {
                            AbstractLocation vineLoc = Finder.GetLocation(vineName);
                            vineLoc.flingType = FlingType.DirectDeposit;
                            AbstractPlacement ap = vineLoc.Wrap();
                            factory.AddPlacement(ap);
                            return ap;
                        };
                        info.getLocationDef = () => new() {
                            Name = vineName,
                            FlexibleCount = false,
                            AdditionalProgressionPenalty = false,
                            SceneName = VineCoords.placementToPosition[vineName].Item1
                        };
                    });*/
                }
                AddAndEditLocation(rb, Consts.Chain, FlingType.DirectDeposit, false, SceneNames.Ruins1_28);
                /*rb.AddLocationByName(Consts.Chain);
                rb.EditLocationRequest(Consts.Chain, info => {
                    info.customPlacementFetch = (factory, placement) => {
                        AbstractLocation chainLoc = Finder.GetLocation(Consts.Chain);
                        chainLoc.flingType = FlingType.DirectDeposit;
                        AbstractPlacement ap = chainLoc.Wrap();
                        factory.AddPlacement(ap);
                        return ap;
                    };
                    info.getLocationDef = () => new() {
                        Name = Consts.Chain,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false,
                        SceneName = SceneNames.Ruins1_28
                    };
                });*/
            }
        }

        public static void ApplyJarDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.SoulJars) {
                foreach(string jarName in Consts.JarNames) {
                    AddAndEditLocation(rb, jarName, FlingType.Everywhere, false, JarCoords.placementToPosition[jarName].Item1);
                    /*rb.AddLocationByName(jarName);
                    rb.EditLocationRequest(jarName, info => {
                        info.customPlacementFetch = (factory, placement) => {
                            AbstractLocation jarLoc = Finder.GetLocation(jarName);
                            jarLoc.flingType = FlingType.Everywhere;
                            AbstractPlacement ap = jarLoc.Wrap();
                            factory.AddPlacement(ap);
                            return ap;
                        };
                        info.getLocationDef = () => new() {
                            Name = jarName,
                            FlexibleCount = false,
                            AdditionalProgressionPenalty = false,
                            SceneName = JarCoords.placementToPosition[jarName].Item1
                        };
                    });*/
                }
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

            foreach(string vine in Consts.VineNames) {
                rb.EditItemRequest(vine, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = vine,
                        MajorItem = false,
                        PriceCap = 50
                    };
                });
                if(YetAnotherRandoConnection.Settings.Vines) {
                    rb.AddItemByName(vine);
                }
            }
            rb.EditItemRequest(Consts.Chain, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.Chain,
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
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.SoulJars) {
                rb.AddItemByName(Consts.SoulJar, Consts.JarNames.Count);
            }
        }

        private static void RemoveRoots(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                rb.RemoveItemsWhere(item => item.StartsWith("Whispering_Root-"));
            }
        }
    }
}
