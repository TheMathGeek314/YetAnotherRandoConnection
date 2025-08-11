using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using static RandomizerMod.Localization;

namespace YetAnotherRandoConnection {
    public class RandoMenuPage {
        internal MenuPage YarcRandoPage;
        internal MenuElementFactory<GlobalSettings> yarcMEF;
        internal GridItemPanel yarcGIP;

        internal SmallButton JumpToYarcButton;

        internal static RandoMenuPage Instance { get; private set; }

        public static void OnExitMenu() {
            Instance = null;
        }

        public static void Hook() {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button) {
            button = Instance.JumpToYarcButton;
            return true;
        }

        private void SetTopLevelButtonColor() {
            if(JumpToYarcButton != null) {
                JumpToYarcButton.Text.color = YetAnotherRandoConnection.Settings.Any ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }
        }

        private static void ConstructMenu(MenuPage landingPage) => Instance = new(landingPage);

        private RandoMenuPage(MenuPage landingPage) {
            YarcRandoPage = new MenuPage(Localize("YetAnotherRandoConnection"), landingPage);
            yarcMEF = new(YarcRandoPage, YetAnotherRandoConnection.Settings);
            yarcGIP = new(YarcRandoPage, new(0, 300), 3, SpaceParameters.VSPACE_LARGE, SpaceParameters.HSPACE_SMALL, true, yarcMEF.Elements);
            Localize(yarcMEF);
            foreach(IValueElement e in yarcMEF.Elements) {
                e.SelfChanged += obj => SetTopLevelButtonColor();
            }

            JumpToYarcButton = new(landingPage, Localize("YetAnotherRandoConnection"));
            JumpToYarcButton.AddHideAndShowEvent(landingPage, YarcRandoPage);
            SetTopLevelButtonColor();
        }

        internal void ResetMenu(GlobalSettings settings) {
            yarcMEF.SetMenuValues(settings);
            SetTopLevelButtonColor();
        }
    }
}
