namespace YetAnotherRandoConnection {
    public class GlobalSettings {
        public bool Vines = false;
        public bool SoulJars = false;
        public bool Telescope = false;
        public bool DreamOrbs = false;

        public bool Any => Vines || SoulJars || Telescope || DreamOrbs;
    }
}
