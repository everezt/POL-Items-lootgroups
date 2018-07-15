using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pol_Items_lootgroups.Classes
{
    static class MagicAllowed
    {
        public class MagicAllow
        {
            public string Name { get; set; }
            public string MagicType { get; set; }
            public string Effect { get; set; }
        }


        public static List<MagicAllow> AllowedMagicItems = new List<MagicAllow>()
        {

            // metal weapons
            new MagicAllow { Name = "Longsword", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "WarAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "BattleAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "DoubleAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "ExecutionersAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "LargeBattleAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "TwoHandedAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "HammerPick", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Mace", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Maul", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "WarHammer", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "WarMace", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Bardiche", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Halberd", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "ShortSpear", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Spear", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "WarFork", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "BroadSword", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Cutlass", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Dagger", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Kryss", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Katana", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Scimitar", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "VikingSword", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Hatchet", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "PickAxe", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "ButcherKnife", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "Cleaver", MagicType = "weapon", Effect = "ore" },
            new MagicAllow { Name = "PitchFork", MagicType = "weapon", Effect = "ore" },

            // wood weapons
            new MagicAllow { Name = "Bow", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "ShortBow", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "LongBow", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "Crossbow", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "HeavyCrossbow", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "RepeatingCrossbow", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "CompositeBow", MagicType = "weapon", Effect = "wood" },

            // staves
            new MagicAllow { Name = "BlackStaff", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "GnarledStaff", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "QuarterStaff", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "ShepherdsCrook", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "MageStaff", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "NecroStaff", MagicType = "weapon", Effect = "wood" },
            new MagicAllow { Name = "Club", MagicType = "weapon", Effect = "wood" },

            // leather armors
            new MagicAllow { Name = "LeatherGorget", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherSleeves", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherGloves", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherCap", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherLeggings", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherTunic", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherTunic2", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "StuddedGorget", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "StuddedGloves", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "StuddedSleeves", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "StuddedLeggings", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "StuddedTunic", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "FemaleStudded", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "FemaleLeather", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherSkirt", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherBustier", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "StuddedBustier", MagicType = "armor", Effect = "leather" },
            new MagicAllow { Name = "LeatherShorts", MagicType = "armor", Effect = "leather" },

            // metal armors
            new MagicAllow { Name = "ChainmailCoif", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "ChainmailGloves", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "ChainmailLeggings", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "ChainmailTunic", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "CloseHelm", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "Helmet", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "Bascinet", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "NoseHelm", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlateHelm", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "RingmailTunic", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "RingmailSleeves", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "RingmailLeggings", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "RingmailGloves", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlatemailBreastplate", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlatemailArms", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlatemailLegs", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlatemailGorget", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlatemailGloves", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "PlatemailGloves2", MagicType = "armor", Effect = "ore" },
            new MagicAllow { Name = "FemalePlate", MagicType = "armor", Effect = "ore" },

            // shields
            new MagicAllow { Name = "BronzeShield", MagicType = "shield", Effect = "" },
            new MagicAllow { Name = "Buckler", MagicType = "shield", Effect = "ore" },
            new MagicAllow { Name = "KiteShield", MagicType = "shield", Effect = "ore" },
            new MagicAllow { Name = "HeaterShield", MagicType = "shield", Effect = "ore" },
            new MagicAllow { Name = "KiteShieldB2", MagicType = "shield", Effect = "ore" },
            new MagicAllow { Name = "WoodenShield", MagicType = "shield", Effect = "wood" },
            new MagicAllow { Name = "MetalShield", MagicType = "shield", Effect = "" },

            // bone armors           
            new MagicAllow { Name = "Bonearms", MagicType = "armor", Effect = "bone" },
            new MagicAllow { Name = "Bonetunic", MagicType = "armor", Effect = "bone" },
            new MagicAllow { Name = "Bonegloves", MagicType = "armor", Effect = "bone" },
            new MagicAllow { Name = "Bonelegs", MagicType = "armor", Effect = "bone" },
            new MagicAllow { Name = "Bonehelm", MagicType = "armor", Effect = "bone" },

            // wands
            new MagicAllow { Name = "wand1", MagicType = "wand", Effect = "charges" },
            new MagicAllow { Name = "wand2", MagicType = "wand", Effect = "charges" },
            new MagicAllow { Name = "wand3", MagicType = "wand", Effect = "charges" },
            new MagicAllow { Name = "wand4", MagicType = "wand", Effect = "charges" },

            // jewels
            new MagicAllow { Name = "necklace", MagicType = "jewel", Effect = "" },
            new MagicAllow { Name = "bracelet", MagicType = "jewel", Effect = "" },
            new MagicAllow { Name = "ring", MagicType = "jewel", Effect = "" },
            new MagicAllow { Name = "necklace2", MagicType = "jewel", Effect = "" },
            new MagicAllow { Name = "necklace3", MagicType = "jewel", Effect = "" },
            new MagicAllow { Name = "earrings", MagicType = "jewel", Effect = "" },

            // clothes
            new MagicAllow { Name = "Robe", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "HalfApron", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "FullApron", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "BodySash", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "FancyShirt", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "FancyDress", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "PlainDress", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Doublet", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "JestersSuit", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Tunic", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Surcoat", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Cloak", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Shirt", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "LongPants", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Sandals", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Shoes", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Skirt", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "ShortPants", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "Kilt", MagicType = "cloth", Effect = "" },
            new MagicAllow { Name = "LongPants2", MagicType = "cloth", Effect = "" },

            // tools
            new MagicAllow { Name = "SewingKit", MagicType = "tool", Effect = "charges" },
            new MagicAllow { Name = "SkinningKnife", MagicType = "tool", Effect = "charges" },
            new MagicAllow { Name = "SmithyHammer", MagicType = "tool", Effect = "charges" },

            // instruments
            new MagicAllow { Name = "Flute", MagicType = "instrument", Effect = "" },
            new MagicAllow { Name = "Tamborine", MagicType = "instrument", Effect = "" },
            new MagicAllow { Name = "TamborineWithRibbon", MagicType = "instrument", Effect = "" },
            new MagicAllow { Name = "Lute", MagicType = "instrument", Effect = "" },
            new MagicAllow { Name = "Lute2", MagicType = "instrument", Effect = "" },
            new MagicAllow { Name = "Drum", MagicType = "instrument", Effect = "" }
        };

    }
}
