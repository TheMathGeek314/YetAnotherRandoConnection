using System.Collections.Generic;
using System.IO;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Json;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.Menu;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace YetAnotherRandoConnection {
    public static class LogicAdder {
        public static void Hook() {
            RandomizerMenuAPI.OnGenerateStartLocationDict += RemoveGPStart;
            RCData.RuntimeLogicOverride.Subscribe(50, ApplyLogic);
        }
        
        private static void RemoveGPStart(Dictionary<string, RandomizerMod.RandomizerData.StartDef> startDefs)
        {
            if (!YetAnotherRandoConnection.Settings.Any)
                return;

            List<string> keys = new (startDefs.Keys);
            foreach (var startName in keys)
            {
                var start = startDefs[startName];
                // Lower GP stag is a jail if Vines are on.
                if (start.SceneName == SceneNames.Fungus1_13 && YetAnotherRandoConnection.Settings.Vines)
                    startDefs[startName] = start with {RandoLogic = "FALSE"};
            }
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!YetAnotherRandoConnection.Settings.Any)
                return;
            JsonLogicFormat fmt = new();
            using Stream s = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.logic.json");
            lmb.DeserializeFile(LogicFileType.Locations, fmt, s);

            using Stream st = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.waypoints.json");
            lmb.DeserializeFile(LogicFileType.Waypoints, fmt, st);

            using Stream str = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.LogicOverrides.json");
            lmb.DeserializeFile(LogicFileType.LogicEdit, fmt, str);

            DefineTermsAndItems(lmb, fmt);
        }

        private static void DefineTermsAndItems(LogicManagerBuilder lmb, JsonLogicFormat fmt) {
            using Stream t = typeof(LogicAdder).Assembly.GetManifestResourceStream("YetAnotherRandoConnection.Resources.terms.json");
            lmb.DeserializeFile(LogicFileType.Terms, fmt, t);

            lmb.AddItem(new SingleItem(Consts.EssenceOrb, new TermValue(lmb.GetTerm("ESSENCE"), 1)));

            foreach(string vine in Consts.VineNames) {
                lmb.AddItem(new SingleItem(vine, new TermValue(lmb.GetTerm(vine), 1)));
            }

            lmb.AddItem(new SingleItem(Consts.Chain, new TermValue(lmb.GetTerm(Consts.Chain), 1)));

            lmb.AddItem(new EmptyItem(Consts.SoulJar));

            lmb.AddItem(new EmptyItem(Consts.EggBomb));

            lmb.AddItem(new EmptyItem(Consts.Telescope));
        }
    }
}
