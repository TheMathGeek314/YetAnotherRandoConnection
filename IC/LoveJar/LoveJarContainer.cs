using UnityEngine;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Components;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Util;

namespace YetAnotherRandoConnection {
    public abstract class LoveJarParent: Container {
        public static GameObject emptyJarPrefab;
    }

    public abstract class LoveJarContainer<T>: LoveJarParent where T: LoveJarContainer<T> {
        public override bool SupportsDrop => true;
        public override bool SupportsInstantiate => true;

        public static GameObject contentsPrefab;
        protected static Vector3 contentOffset;

        public override GameObject GetNewContainer(ContainerInfo info) {
            GameObject jar = GameObject.Instantiate(emptyJarPrefab);
            jar.name = $"{jar.name}-{info.giveInfo.placement.Name}";
            jar.GetComponent<PersistentBoolItem>().enabled = false;
            jar.SetActive(true);
            if(contentsPrefab != null) {
                GameObject.Instantiate(contentsPrefab, jar.transform.position + contentOffset, Quaternion.identity, jar.transform).SetActive(true);
            }
            ContainerGiveInfo cgi = info.giveInfo;
            PlayMakerFSM bottleControl = jar.LocateMyFSM("Bottle Control");
            bottleControl.GetState("Idle").RemoveActionsOfType<BoolTest>();
            bottleControl.GetState("Shatter").AddFirstAction(new Lambda(() => {
                GiveInfo giveInfo = new() {
                    Container = Name,
                    FlingType = FlingType.Everywhere,
                    Transform = jar.transform,
                    MessageType = MessageType.Corner
                };
                GameObject itemParent = new("Item parent");
                itemParent.transform.position = jar.transform.position;
                foreach(AbstractItem item in cgi.items) {
                    if(item.GiveEarly(Consts.LoveJar)) {
                        item.Give(cgi.placement, giveInfo);
                    }
                    else {
                        GameObject shiny = ShinyUtility.MakeNewShiny(cgi.placement, item, cgi.flingType);
                        ShinyUtility.PutShinyInContainer(itemParent, shiny);
                        ShinyUtility.FlingShinyRandomly(shiny.LocateMyFSM("Shiny Control"));
                    }
                }
                foreach(Transform t in itemParent.transform)
                    t.gameObject.SetActive(true);
                cgi.placement.AddVisitFlag(VisitState.Opened);
            }));
            jar.AddComponent<DropIntoPlace>();
            return jar;
        }

        public override void ApplyTargetContext(GameObject obj, float x, float y, float elevation) {
            doApplyTargetContext(obj, x, y, elevation, true);
        }

        public override void ApplyTargetContext(GameObject obj, GameObject target, float elevation) {
            doApplyTargetContext(obj, target.transform.position.x, target.transform.position.y, elevation, target.activeSelf);
        }

        private void doApplyTargetContext(GameObject obj, float x, float y, float elevation, bool active) {
            obj.transform.position = new Vector3(x, y - elevation + 0.2f, -0.01f);
            obj.SetActive(active);
        }
    }

    public class LoveJarEmptyContainer: LoveJarContainer<LoveJarEmptyContainer> {
        public override string Name => Consts.LoveJar + "-Empty";
    }

    public class LoveJarCrawlidContainer: LoveJarContainer<LoveJarCrawlidContainer> {
        public override string Name => Consts.LoveJar + "-Crawlid";

        public LoveJarCrawlidContainer() {
            contentOffset = new Vector3(1.2045f, -0.4559f, 0.02f);
        }
    }

    public class LoveJarHuskContainer: LoveJarContainer<LoveJarHuskContainer> {
        public override string Name => Consts.LoveJar + "-Husk";

        public LoveJarHuskContainer() {
            contentOffset = new Vector3(0.11f, -0.4f, 0.019f);
        }
    }

    public class LoveJarLeapingContainer: LoveJarContainer<LoveJarLeapingContainer> {
        public override string Name => Consts.LoveJar + "-Leaping_Husk";

        public LoveJarLeapingContainer() {
            contentOffset = new Vector3(-0.4092f, -0.3968f, 0.02f);
        }
    }

    public class LoveJarObbleContainer: LoveJarContainer<LoveJarObbleContainer> {
        public override string Name => Consts.LoveJar + "-Obble";

        public LoveJarObbleContainer() {
            contentOffset = new Vector3(-0.1223f, -0.8664f, 0.02f);
        }
    }

    public class LoveJarSentryContainer: LoveJarContainer<LoveJarSentryContainer> {
        public override string Name => Consts.LoveJar + "-Sentry";

        public LoveJarSentryContainer() {
            contentOffset = new Vector3(-0.36f, -0.5289f, 0.02f);
        }
    }

    public class LoveJarVengeflyContainer : LoveJarContainer<LoveJarVengeflyContainer> {
        public override string Name => Consts.LoveJar + "-Vengefly";
        
        public LoveJarVengeflyContainer() {
            contentOffset = new Vector3(0, -0.22f, 0.01f);
        }
    }
}