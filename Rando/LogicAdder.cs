using System.IO;
using RandomizerCore;
using RandomizerCore.Json;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace YetAnotherRandoConnection {
    public static class LogicAdder {
        public static void Hook() {
            RCData.RuntimeLogicOverride.Subscribe(50, ApplyLogic);
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb) {
            if(!YetAnotherRandoConnection.Settings.Any)
                return;
            JsonLogicFormat fmt = new();
            using Stream s = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.logic.json");
            lmb.DeserializeFile(LogicFileType.Locations, fmt, s);

            using Stream st = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.waypoints.json");
            lmb.DeserializeFile(LogicFileType.Waypoints, fmt, st);

            DefineTermsAndItems(lmb, fmt);
        }

        private static void DefineTermsAndItems(LogicManagerBuilder lmb, JsonLogicFormat fmt) {
            using Stream t = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.terms.json");
            lmb.DeserializeFile(LogicFileType.Terms, fmt, t);

            Term essence = lmb.GetTerm("ESSENCE");
            lmb.AddItem(new SingleItem(Consts.EssenceOrb, new TermValue(essence, 1)));

            foreach(string vine in Consts.VineNames) {
                lmb.AddItem(new SingleItem(vine, new TermValue(lmb.GetTerm(vine), 1)));
            }
        }
    }
}
