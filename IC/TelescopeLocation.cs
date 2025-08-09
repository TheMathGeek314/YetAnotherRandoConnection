using Modding;
using System.Linq;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;
using Satchel;

namespace YetAnotherRandoConnection {
    public class TelescopeLocation: AutoLocation {
        private GameObject CanvasObject;
        private GameObject ImagePanel;
        protected override void OnLoad() {
            On.PlayMakerFSM.OnEnable += EditTelescope;
        }

        protected override void OnUnload() {
            On.PlayMakerFSM.OnEnable -= EditTelescope;
        }

        private void EditTelescope(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            if(self.gameObject.name == "Telescope Inspect" && self.FsmName == "Conversation Control") {
                self.GetValidState("Cinematic").AddCustomAction(() => {
                    if(CanvasObject == null)
                        CanvasObject = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceCamera, new Vector2(Screen.width, Screen.height));
                    Sprite sprite = Ref.Settings.Placements[Consts.Telescope].Items.First().GetPreviewSprite();
                    CanvasUtil.RectData rectData = new(new Vector2(250, 250), new Vector2(0, 0));
                    ImagePanel = CanvasUtil.CreateImagePanel(CanvasObject, sprite, rectData);
                    ImagePanel.SetActive(true);
                });
                self.GetValidState("Stop").AddCustomAction(() => {
                    ImagePanel.SetActive(false);
                });
                self.GetValidState("Talk Finish").InsertCustomAction(() => {
                    GiveInfo giveInfo = new() {
                        FlingType = FlingType.DirectDeposit,
                        Container = Container.Unknown,
                        MessageType = MessageType.Big
                    };
                    Ref.Settings.Placements[Consts.Telescope].GiveAll(giveInfo);
                }, 0);
            }
        }
    }
}
