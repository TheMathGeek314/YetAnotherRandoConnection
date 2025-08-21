using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Components;
using ItemChanger.Extensions;
using ItemChanger.Util;
using Satchel;

namespace YetAnotherRandoConnection {
    public class SoulJarContainer: Container {
        public override string Name => Consts.SoulJar;
        public override bool SupportsDrop => true;
        public override bool SupportsInstantiate => true;

        public static GameObject jarPrefab;

        public override GameObject GetNewContainer(ContainerInfo info) {
            GameObject jar = GameObject.Instantiate(jarPrefab);
            jar.name = $"{jar.name}-{info.giveInfo.placement.Name}";
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
                foreach(Transform t in itemParent.transform)
                    t.gameObject.SetActive(true);
                cGiveInfo.placement.AddVisitFlag(VisitState.Opened);
            });

            GameObject parent = new($"{jar.name} parent");
            jar.transform.parent = parent.transform;
            jar.transform.localPosition = Vector3.zero;
            BoxCollider2D box = parent.AddComponent<BoxCollider2D>();
            box.size = new Vector2(1, 2);
            box.offset = new Vector2(0, -1.2f);
            parent.layer = LayerMask.NameToLayer("Corpse");
            parent.AddComponent<DropIntoPlace>();
            return parent;
        }

        public override void ApplyTargetContext(GameObject obj, float x, float y, float elevation) {
            doApplyTargetContext(obj, x, y, elevation, true);
        }

        public override void ApplyTargetContext(GameObject obj, GameObject target, float elevation) {
            doApplyTargetContext(obj, target.transform.position.x, target.transform.position.y, elevation, target.activeSelf);
        }

        private void doApplyTargetContext(GameObject obj, float x, float y, float elevation, bool active) {
            obj.transform.position = new Vector3(x, y - elevation + 0.8719f, -0.01f);
            obj.SetActive(active);
        }
    }
}