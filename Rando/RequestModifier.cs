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

        public static void ApplyOrbDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                foreach((string area, string scene, int count) in Consts.RootCounts) {
                    for(int num = 1; num <= count; num++) {
                        string name = Consts.GetOrbNumName(area, num);
                        Modding.Logger.Log($"[YARC] - ({area}, {num}) => AddLocationByName({name})");
                        rb.AddLocationByName(name);
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
                        });
                    }
                }
            }
        }

        public static void ApplyVineDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Vines) {
                foreach(string vineName in Consts.VineNames) {
                    rb.AddLocationByName(vineName);
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
                    });
                }
            }
        }

        public static void ApplyJarDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.SoulJars) {
                foreach(string jarName in Consts.JarNames) {
                    rb.AddLocationByName(jarName);
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
                    });
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

            /*foreach(string jar in Consts.JarNames) {
                rb.EditItemRequest(jar, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = jar,
                        MajorItem = false,
                        PriceCap = 1
                    };
                });
                if(YetAnotherRandoConnection.Settings.SoulJars) {
                    rb.AddItemByName(jar);
                }
            }*/
            rb.EditItemRequest(Consts.SoulJar, info => {
                info.getItemDef = () => new ItemDef() {
                    Name = Consts.SoulJar,
                    MajorItem = false,
                    PriceCap = 1
                };
            });
            if(YetAnotherRandoConnection.Settings.SoulJars) {
                rb.AddItemByName(Consts.SoulJar, 13);
            }
        }

        private static void RemoveRoots(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.DreamOrbs) {
                rb.RemoveItemsWhere(item => item.StartsWith("Whispering_Root-"));
                //rb.RemoveLocationsWhere(location => location.StartsWith("Whispering_Root-"));
            }
        }
    }
}
