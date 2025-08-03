namespace YetAnotherRandoConnection {
    public class GlobalSettings {
        public bool Vines = false;
        public bool Jars = false;
        public bool Telescope = false;
        public bool Orbs = false;

        public bool Any => Vines || Jars || Telescope || Orbs;
    }
}
