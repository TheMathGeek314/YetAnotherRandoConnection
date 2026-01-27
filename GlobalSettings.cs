namespace YetAnotherRandoConnection {
    public class GlobalSettings {
        public bool DreamOrbs = false;
        public bool Vines = false;
        public bool SoulJars = false;
        public bool HivePlatforms = false;
        public bool JellyEggBombs = false;
        public bool Telescope = false;
        public bool Scarecrow = false;
        public bool Stalactites = false;

        public bool Any => DreamOrbs || Vines || SoulJars || HivePlatforms || JellyEggBombs || Telescope || Scarecrow || Stalactites;

        [MenuChanger.Attributes.MenuRange(-1, 99)]
        public int VineGroup = -1;

        [MenuChanger.Attributes.MenuRange(-1, 99)]
        public int HivePlatformGroup = -1;

        [MenuChanger.Attributes.MenuRange(-1, 99)]
        public int EggBombGroup = -1;

        [MenuChanger.Attributes.MenuRange(-1, 99)]
        public int TelescopeGroup = -1;

        [MenuChanger.Attributes.MenuRange(-1, 99)]
        public int ScarecrowGroup = -1;

        [MenuChanger.Attributes.MenuRange(-1, 99)]
        public int StalactiteGroup = -1;
    }
}