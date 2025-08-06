using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Util;
using Satchel;

namespace YetAnotherRandoConnection {
    public class SoulJarContainer: Container {
        public override string Name => Consts.SoulJar;
        //public override bool SupportsDrop => true;
        public override bool SupportsInstantiate => true;

        public static GameObject jarPrefab;

        public override GameObject GetNewContainer(ContainerInfo info) {
            GameObject jar = GameObject.Instantiate(jarPrefab);
            jar.SetActive(true);
            FsmState soulState = jar.GetComponent<PlayMakerFSM>().GetValidState("Soul");
            soulState.RemoveFirstActionOfType<FlingObjectsFromGlobalPool>();
            ContainerGiveInfo cGiveInfo = info.giveInfo;
            soulState.AddCustomAction(() => {
                GiveInfo giveInfo = new() {
                    Container = Name,
                    FlingType = FlingType.Everywhere,
                    Transform = jar.transform,
                    MessageType = MessageType.Corner
                };
                GameObject itemParent = new("Item parent");
                itemParent.transform.position = jar.transform.position;
                foreach(AbstractItem item in cGiveInfo.items) {
                    if(!item.IsObtained()) {
                        if(item.GiveEarly(Container.Totem)) {
                            item.Give(cGiveInfo.placement, giveInfo.Clone());
                        }
                        else {
                            GameObject shiny = ShinyUtility.MakeNewShiny(cGiveInfo.placement, item, cGiveInfo.flingType);
                            ShinyUtility.PutShinyInContainer(itemParent, shiny);
                            if(giveInfo.FlingType == FlingType.Everywhere)
                                ShinyUtility.FlingShinyRandomly(shiny.LocateMyFSM("Shiny Control"));
                            else
                                ShinyUtility.FlingShinyDown(shiny.LocateMyFSM("ShinyControl"));
                        }
                    }
                }
                foreach(Transform t in itemParent.transform)
                    t.gameObject.SetActive(true);
                cGiveInfo.placement.AddVisitFlag(VisitState.Opened);
            });
            return jar;
        }
    }
}
