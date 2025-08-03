using ItemChanger;
using System.Collections.Generic;

namespace YetAnotherRandoConnection {
    public class Consts {
        public const string EssenceOrb = "Essence_Orb";

        public static readonly List<(string, string, int)> RootCounts = [
            ("Crossroads", SceneNames.Crossroads_07, 29),
            ("Greenpath", SceneNames.Fungus1_13, 44),
            ("FungalWastes_Canyon", SceneNames.Fungus2_33, 20),
            ("FungalWastes_Village", SceneNames.Fungus2_17, 18),
            ("Deepnest", SceneNames.Deepnest_39, 45),
            ("QueensGardens", SceneNames.Fungus3_11, 29),
            ("KingdomsEdge", SceneNames.Deepnest_East_07, 51),
            ("Waterways", SceneNames.Abyss_01, 35),
            ("City", SceneNames.Ruins1_17, 28),
            ("RestingGrounds", SceneNames.RestingGrounds_05, 20),
            ("SpiritsGlade", SceneNames.RestingGrounds_08, 34),
            ("CrystalPeak", SceneNames.Mines_23, 21),
            ("HowlingCliffs", SceneNames.Cliffs_01, 46),
            ("AncestralMound", SceneNames.Crossroads_ShamanTemple, 42),
            ("Hive", SceneNames.Hive_02, 20)
        ];

        public static readonly Dictionary<string, string> OrbAreas = new() {
            { SceneNames.Crossroads_07, "Crossroads" },
            { SceneNames.Fungus1_13, "Greenpath" },
            { SceneNames.Fungus2_33, "FungalWastes_Canyon" },
            { SceneNames.Fungus2_17, "FungalWastes_Village" },
            { SceneNames.Deepnest_39, "Deepnest" },
            { SceneNames.Fungus3_11, "QueensGardens" },
            { SceneNames.Deepnest_East_07, "KingdomsEdge" },
            { SceneNames.Abyss_01, "Waterways" },
            { SceneNames.Ruins1_17, "City" },
            { SceneNames.RestingGrounds_05, "RestingGrounds" },
            { SceneNames.RestingGrounds_08, "SpiritsGlade" },
            { SceneNames.Mines_23, "CrystalPeak" },
            { SceneNames.Cliffs_01, "HowlingCliffs" },
            { SceneNames.Crossroads_ShamanTemple, "AncestralMound" },
            { SceneNames.Hive_02, "Hive" }
        };
    }
}
