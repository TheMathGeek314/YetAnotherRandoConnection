using System.Threading.Tasks;
using UnityEngine;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using Modding;

namespace YetAnotherRandoConnection {
    public class ScarecrowItem: AbstractItem {
        public static GameObject hopperPrefab1, hopperPrefab2;

        public ScarecrowItem() {
            name = Consts.Scarecrow;
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_scarecrow");
            AddTag<PersistentItemTag>().Persistence = Persistence.Persistent;
            UIDef = new MsgUIDef {
                name = new BoxedString(RandoInterop.clean(name)),
                shopDesc = new BoxedString("Watch out!"),
                sprite = new EmbeddedSprite("pin_scarecrow")
            };
        }

        public override void GiveImmediate(GiveInfo info) {
            RaycastHit2D raycast = Physics2D.Raycast(HeroController.instance.transform.position + new Vector3(0, 1, 0), Vector2.up, 50, LayerMask.GetMask("Terrain"));
            Vector3 origin = raycast.collider == null ? HeroController.instance.transform.position + new Vector3(0, 50, 0) : new Vector3(raycast.point.x, raycast.point.y - 2, 0);
            SummonHoppers(origin);
            YARCModule.Instance.RecordItem(RandoInterop.clean(name));
        }

        private async void SummonHoppers(Vector3 origin) {
            for(int i = 0; i < 10; i++) {
                GameObject hopper1 = GameObject.Instantiate(hopperPrefab1, origin + new Vector3(2 - i * 0.5f, 0, 0), Quaternion.identity);
                GameObject hopper2 = GameObject.Instantiate(hopperPrefab2, origin + new Vector3(i * 0.5f - 2, 0, 0), Quaternion.identity);

                foreach(GameObject hopper in new GameObject[] { hopper1, hopper2 }) {
                    HealthManager hm = hopper.GetComponent<HealthManager>();
                    hm.SetGeoLarge(0);
                    hm.SetGeoMedium(0);
                    hm.SetGeoSmall(0);
                    hopper.SetActive(true);
                }

                await Task.Yield();
            }
        }
    }
}