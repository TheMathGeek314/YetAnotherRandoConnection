namespace YetAnotherRandoConnection {
    public class GlobalSettings {
        public bool DreamOrbs = false;
        public bool Vines = false;
        public bool SoulJars = false;
        public bool Telescope = false;

        public bool Any => DreamOrbs || Vines || SoulJars || Telescope;
    }
}
