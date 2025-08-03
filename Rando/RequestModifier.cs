using ItemChanger;
using ItemChanger.Items;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace YetAnotherRandoConnection {
    public class RequestModifier {
        public static void Hook() {
            RequestBuilder.OnUpdate.Subscribe(-100, ApplyOrbDefs);
            RequestBuilder.OnUpdate.Subscribe(-499, SetupItems);
        }

        public static void ApplyOrbDefs(RequestBuilder rb) {
            if(YetAnotherRandoConnection.Settings.Orbs) {
                foreach((string area, string scene, int count) in Consts.RootCounts) {
                    for(int num = 1; num <= count; num++) {
                        string name = $"DreamOrb_{area}_{num}";
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
            if(YetAnotherRandoConnection.Settings.Orbs) {
                rb.AddItemByName(Consts.EssenceOrb, 482);
            }
        }
    }
}
