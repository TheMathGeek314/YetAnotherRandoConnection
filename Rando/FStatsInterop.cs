using System;
using System.Collections.Generic;
using System.Linq;
using FStats;
using FStats.StatControllers;
using FStats.Util;

namespace YetAnotherRandoConnection {
    public static class FStatsInterop {

        public static void Hook() {
            API.OnGenerateFile += GenerateStats;
        }

        private static void GenerateStats(Action<StatController> generateStats) {
            if(!YetAnotherRandoConnection.Settings.Any)
                return;

            generateStats(new YARCStats());
        }
    }

    public class YARCStats: StatController {
        public override void Initialize() 
        {
            YARCModule.OnItemObtained += AddEntry;
        }
        public override void Unload() 
        {
            YARCModule.OnItemObtained -= AddEntry;
        }
        public record KeyItem(string item, float timestamp);
        public List<KeyItem> KeyItems = [];
        public List<string> UsedKeys = [];
        public void AddEntry(string entry) {
            if(!UsedKeys.Contains(entry)) {
                KeyItem key = new(entry, FStatsMod.LS.Get<Common>().CountedTime);
                KeyItems.Add(key);
                UsedKeys.Add(entry);
            }
        }
        public override IEnumerable<DisplayInfo> GetDisplayInfos() {
            List<string> rows = KeyItems.OrderBy(x => x.timestamp).Select(x => $"{x.item}: {x.timestamp.PlaytimeHHMMSS()}").ToList();
            bool orbRando = YetAnotherRandoConnection.Settings.DreamOrbs;
            string subTitle = "";
            if(YetAnotherRandoConnection.Settings.DreamOrbs)
                subTitle += $"Orbs obtained: {YARCModule.Instance.Orbs} / 482";
            if(YetAnotherRandoConnection.Settings.JellyEggBombs) {
                if(subTitle.Length > 1)
                    subTitle += " | ";
                subTitle += $"Bombs triggered: {YARCModule.Instance.Bombs}";
            }
            yield return new() {
                Title = "YARC",
                MainStat = subTitle,
                StatColumns = Columnize(rows),
                Priority = BuiltinScreenPriorityValues.ExtensionStats
            };
        }
        private const int COL_SIZE = 10;
        private List<string> Columnize(List<string> rows) {
            int columnCount = (rows.Count + COL_SIZE - 1) / COL_SIZE;
            List<string> list = [];
            for(int i = 0; i < columnCount; i++) {
                list.Add(string.Join("\n", rows.Slice(i, columnCount)));
            }
            return list;
        }
    }
}