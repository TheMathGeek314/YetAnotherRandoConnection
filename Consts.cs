using System.Collections.Generic;
using ItemChanger;

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

        public static readonly List<string> VineNames = new() {
            //"Chain-Storerooms",
            "Vine-Root_Left",
            "Vine-Root_Right",
            "Vine-Vessel_Fragment",
            "Vine-Hunter_Upper",
            "Vine-Hunter_Middle",
            "Vine-Hunter_Lower",
            "Vine-Hunter_Left",
            "Vine-Cornifer_Grub",
            "Vine-Cornifer_Spikes",
            "Vine-Cornifer_Upper",
            "Vine-Cornifer_Shortcut",
            "Vine-Cornifer_Left_1",
            "Vine-Cornifer_Left_2",
            "Vine-Cornifer_Left_3",
            "Vine-Thorns",
            "Vine-Moss_Knight",
            "Vine-Stag_Journal",
            "Vine-Stag_Lower",
            "Vine-Durandas"
        };

        public static readonly List<string> VineDescriptions = new() {
            "And they were roommates!\r\n\r\nOh my god they were roommates.",
            "Can you guys say \"Colorado!\"\r\nI'M A GIRAFFE!",
            "Chris! Is that a weed?\r\n\r\nNo, this is a crayon.\r\n\r\nI'm calling the police!",
            "Good evening.",
            "Get to Del Taco! They got a new thing called\r\nFR E SH A VOCA DO",
            "Happy Christmas!\r\n\r\nIt's Chrisman!\r\n\r\nMerry Crisis!\r\n\r\nMerry Chrysler!",
            "Have anybody ever tell you you look like Beyonce?\r\n\r\nNah, they usually tell me I look like Shalissa.",
            "Hey Ron.\r\nHey Billy.\r\n\r\nThat hurt.",
            "Hi, welcome to Chili's!",
            "I don't even know which way the Quiznos is.",
            "It's a avocado! Thanks!",
            "It is Wednesday, my dudes.\r\n\r\nAaaaaaaAAAAAAAHHH",
            "Ladies and gentlemen, welcome to T-T-T-T-T-T-TARGET",
            "Look at all those chickens!",
            "Look at this graph!",
            "oh hi, thanks for checking in, i'm still a piece of GARBAGE",
            "Road work ahead? Uh yeah, I sure hope it does!",
            "Sabra gives you all your daily nutrients, like zero grams of trans fat and oh my god cholesterol!",
            "So I am confusion. Why is this one \"Kansas\" but this one is not \"Ar-Kansas?\" America explain! Explain, what do you mean it Arkansas??",
            "Stop, I could've dropped my croissant!",
            "Well, when life gives you lemons!",
            "WHAT ARE THOOOOOOOSE?\r\n\r\nThey are my crocs.",
            "You stupid.\r\n\r\nNo I not.\r\n\r\nWhat's 9 + 10?\r\n\r\n21.\r\n\r\nYou stupid."
        };
    }
}
