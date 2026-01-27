using System.Collections.Generic;
using ItemChanger;

namespace YetAnotherRandoConnection {
    public class Consts {
        public const string EssenceOrb = "Essence_Orb";
        public const string Chain = "Chain-Storerooms";
        public const string SoulJar = "Soul_Jar";
        public const string EggBomb = "Jelly_Egg_Bomb";
        public const string Telescope = "Lurien's_Telescope";
        public const string Scarecrow = "Scarecrow";

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

        public static string GetOrbNumName(string area, int num) {
            string prefix = $"DreamOrb_{area}_";
            switch(area) {
                case "Hive":
                    if(num < 3)
                        return $"{prefix}{num + 1}";
                    if(num == 3)
                        return $"{prefix}5";
                    if(num < 8)
                        return $"{prefix}{num + 3}";
                    return $"{prefix}{num + 4}";
                case "HowlingCliffs":
                    if(num < 4)
                        return $"{prefix}{num + 2}";
                    if(num == 4)
                        return $"{prefix}7";
                    if(num < 7)
                        return $"{prefix}{num + 4}";
                    if(num == 7)
                        return $"{prefix}12";
                    if(num < 15)
                        return $"{prefix}{num + 6}";
                    if(num == 15)
                        return $"{prefix}23";
                    if(num < 24)
                        return $"{prefix}{num + 9}";
                    if(num == 24)
                        return $"{prefix}34";
                    if(num < 27)
                        return $"{prefix}{num + 11}";
                    if(num < 29)
                        return $"{prefix}{num + 12}";
                    if(num < 31)
                        return $"{prefix}{num + 13}";
                    if(num == 31)
                        return $"{prefix}46";
                    if(num < 34)
                        return $"{prefix}{num + 16}";
                    if(num == 34)
                        return $"{prefix}51";
                    if(num < 37)
                        return $"{prefix}{num + 18}";
                    if(num < 42)
                        return $"{prefix}{num + 19}";
                    if(num == 42)
                        return $"{prefix}62";
                    if(num < 45)
                        return $"{prefix}{num + 21}";
                    return $"{prefix}{num + 22}";
                case "KingdomsEdge":
                    if(num < 39)
                        return $"{prefix}{num}";
                    return $"{prefix}{num + 1}";
                default:
                    return $"{prefix}{num}";
            }
        }

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

        public static readonly List<string> UsefulVines = new() {
            "Vine-Root_Left",
            "Vine-Root_Right",
            "Vine-Vessel_Fragment",
            "Vine-Hunter_Lower",
            "Vine-Cornifer_Grub",
            "Vine-Cornifer_Left_2",
            "Vine-Thorns",
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

        public static readonly List<string> JarNames = new() {
            "Soul_Jar-Shade_Soul",
            "Soul_Jar-Entrance_Lower",
            "Soul_Jar-Entrance_Middle",
            "Soul_Jar-Entrance_Upper",
            "Soul_Jar-East_Lower",
            "Soul_Jar-East_Upper",
            "Soul_Jar-Center_Left",
            "Soul_Jar-Center_Left_Dupe",
            "Soul_Jar-Center_Right",
            "Soul_Jar-Soul_Master",
            "Soul_Jar-Soul_Master_Dupe",
            "Soul_Jar-Soul_Master_Dupe_2",
            "Soul_Jar-West",
            "Soul_Jar-Lighthouse",
            "Soul_Jar-Lighthouse_Dupe"
        };

        public static readonly List<string> HivePlatNames = new() {
            "Hive_Platform-Lower_East",
            "Hive_Platform-Lower_North",
            "Hive_Platform-Lower_South",
            "Hive_Platform-Lower_West",
            "Hive_Platform-Upper_Right",
            "Hive_Platform-Upper_Left",
            "Hive_Platform-East_Right",
            "Hive_Platform-East_Middle"
        };

        public static readonly List<string> EggBombNames = new() {
            "Egg_Bomb-Map_Right",
            "Egg_Bomb-Map_Left",
            "Egg_Bomb-Tall_Room_Left",
            "Egg_Bomb-Tall_Room_Right",
            "Egg_Bomb-Notch_Corner_1",
            "Egg_Bomb-Notch_Corner_2",
            "Egg_Bomb-Notch_End",
            "Egg_Bomb-Notch_Entrance",
            "Egg_Bomb-Notch_Exit",
            "Egg_Bomb-Notch_Left_1",
            "Egg_Bomb-Notch_Left_2",
            "Egg_Bomb-Notch_Lower_1",
            "Egg_Bomb-Notch_Lower_2",
            "Egg_Bomb-Notch_Lower_3",
            "Egg_Bomb-Notch_Middle_1",
            "Egg_Bomb-Notch_Middle_2",
            "Egg_Bomb-Notch_Middle_3",
            "Egg_Bomb-Notch_Middle_4",
            "Egg_Bomb-Notch_Middle_5",
            "Egg_Bomb-Notch_Middle_6",
            "Egg_Bomb-Notch_Middle_7",
            "Egg_Bomb-Notch_Middle_8",
            "Egg_Bomb-Notch_Middle_9",
            "Egg_Bomb-Notch_Middle_10",
            "Egg_Bomb-Notch_Middle_11",
            "Egg_Bomb-Notch_Middle_13",
            "Egg_Bomb-Notch_Middle_14",
            "Egg_Bomb-Notch_Middle_12",
            "Egg_Bomb-Notch_Right_1",
            "Egg_Bomb-Notch_Right_2",
            "Egg_Bomb-Notch_Upper_1",
            "Egg_Bomb-Notch_Upper_2",
            "Egg_Bomb-Notch_Upper_3",
            "Egg_Bomb-Notch_Upper_4",
            "Egg_Bomb-Notch_Upper_5",
            "Egg_Bomb-Notch_Upper_6",
            "Egg_Bomb-Notch_Upper_7",
            "Egg_Bomb-Notch_Upper_8"
        };

        public static readonly List<string> StalactiteNames = new() {
            "Stalactite-King's_Pass_Focus_Tablet",
            "Stalactite-King's_Pass_Hidden",
            "Stalactite-King's_Pass_Lifeblood",
            "Stalactite-King's_Pass_Spike_Pit_1",
            "Stalactite-King's_Pass_Spike_Pit_2",
            "Stalactite-King's_Pass_Spike_Pit_3",
            "Stalactite-King's_Pass_Spike_Pit_4",
            "Stalactite-King's_Pass_Spike_Pit_5",
            "Stalactite-Brooding_Mawlek",
            "Stalactite-Cornifer_1",
            "Stalactite-Cornifer_2",
            "Stalactite-Cornifer_3",
            "Stalactite-Acid_Grub",
            "Stalactite-Aspid_Arena_1",
            "Stalactite-Aspid_Arena_2",
            "Stalactite-Above_Crossroads_Lever",
            "Stalactite-East_of_Black_Egg_1",
            "Stalactite-East_of_Black_Egg_2",
            "Stalactite-Peak_Toll_Left_1",
            "Stalactite-Peak_Toll_Left_2",
            "Stalactite-Peak_Toll_Left_3",
            "Stalactite-Peak_Toll_Right_1",
            "Stalactite-Peak_Toll_Right_2",
            "Stalactite-Baldur_Shell_Left",
            "Stalactite-Baldur_Shell_Lower",
            "Stalactite-Baldur_Shell_Grub_1",
            "Stalactite-Baldur_Shell_Grub_2",
            "Stalactite-Baldur_Shell_Upper",
            "Stalactite-Markoth_Right",
            "Stalactite-Markoth_Left",
            "Stalactite-Markoth_Upper_1",
            "Stalactite-Markoth_Upper_2",
            "Stalactite-Markoth_Journal_1",
            "Stalactite-Markoth_Journal_2",
            "Stalactite-Markoth_Journal_3",
            "Stalactite-Camp_Grub_Entrance_1",
            "Stalactite-Camp_Grub_Entrance_2",
            "Stalactite-Camp_Grub_1",
            "Stalactite-Camp_Grub_2",
            "Stalactite-Camp_Grub_3",
            "Stalactite-Camp_Left",
            "Stalactite-Camp_Middle",
            "Stalactite-Camp_Upper_1",
            "Stalactite-Camp_Upper_2",
            "Stalactite-Hornet_1",
            "Stalactite-Hornet_2",
            "Stalactite-Hornet_3",
            "Stalactite-Hornet_4",
            "Stalactite-Hornet_5",
            "Stalactite-Oro_Dive_Entrance_1",
            "Stalactite-Oro_Dive_Entrance_2",
            "Stalactite-Oro_Dive_Second_1",
            "Stalactite-Oro_Dive_Second_2",
            "Stalactite-Oro_Dive_Primal_Aspids",
            "Stalactite-Oro_Dive_Grub_Path_1",
            "Stalactite-Oro_Dive_Grub_Path_2",
            "Stalactite-Oro_Dive_Grub_Path_3",
            "Stalactite-Oro_Dive_Grub_1",
            "Stalactite-Oro_Dive_Grub_2",
            "Stalactite-Oro_Dive_Grub_3",
            "Stalactite-Oro_Dive_Grub_4",
            "Stalactite-Quick_Slash_Entrance_1",
            "Stalactite-Quick_Slash_Entrance_2",
            "Stalactite-Quick_Slash_Exit_1",
            "Stalactite-Quick_Slash_Exit_2",
            "Stalactite-Quick_Slash_Exit_3",
            "Stalactite-Quick_Slash_Exit_4"
        };
    }
}