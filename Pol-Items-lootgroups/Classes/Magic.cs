using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pol_Items_lootgroups.Classes
{

    static class Magic
    {

        const int ARMOR_ORE_COLOR_CHANCE = 2; // chance represents 0-100%
        const int SHIELD_ORE_COLOR_CHANCE = 10; // chance represents 0-100%
        const int ARMOR_LEATHER_COLOR_CHANCE = 1; // chance represents 0-100%
        const int ARMOR_BONE_COLOR_CHANCE = 1; // chance represents 0-100%
        const int MAGIC_TOOL_CHANCE = 10; // chance represents 0-100%
        const int WEAPON_ORE_COLOR_CHANCE = 7; // chance represents 0-100%
        const int WEAPON_WOOD_COLOR_CHANCE = 7; // chance represents 0-100%

        static public string[] SkillList = new string[]
        {
            "Alchemy",
            "Anatomy",
            "Animal Lore",
            "Item Identification",
            "Arms Lore",
            "Parry",
            "Blacksmithy",
            "Bowcraft",
            "Peacemaking",
            "Camping",
            "Carpentry",
            "Cartography",
            "Cooking",
            "Detect Hidden",
            "Leadership",
            "Evaluating Intelligence",
            "Healing",
            "Fishing",
            "Forensics Evaluation",
            "Herding",
            "Hiding",
            "Provocation",
            "Inscription",
            "Lockpicking",
            "Magery",
            "Magic Resistance",
            "Tactics",
            "Snooping",
            "Musicanship",
            "Poisoning",
            "Archery",
            "Necromancy",
            "Stealing",
            "Tailoring",
            "Taming",
            "Taste Identification",
            "Tinkering",
            "Tracking",
            "Veterinary",
            "Swordmanship",
            "Macefighting",
            "Fencing",
            "Wrestling",
            "Lumber Jacking",
            "Mining",
            "Meditation",
            "Stealth",
            "Remove Trap"
        };

        public static string GetAmorSuffix(int modifier)
        {
            switch (modifier)
            {
                case 1:
                    return "Defense";
                case 2:
                    return "Gurading";
                case 3:
                    return "Protection";
                case 4:
                    return "Fortification";
                case 5:
                    return "Invulnerability";
                default:
                    return "Defense";
            }
        }

        public static string GetQualityModifierName(int modifier)
        {          
            switch (modifier)
            {
                case 1:
                    return "Substantial";
                case 2:
                    return "Durable";
                case 3:
                    return "Massive";
                case 4:
                    return "Fortified";
                case 5:
                    return "An Indestructible";
                default:
                    return "Durable";
            }
        }

        private static string getInstrumentSuffix(int modifier)
        {
            switch (modifier)
            {
                case 1:
                    return "Sonority";
                case 2:
                    return "Accord";
                case 3:
                    return "Melody";
                case 4:
                    return "Harmony";
                case 5:
                    return "Consonance";
                default:
                    return "Sonority";
            }

        }

        private static string getWeaponSuffix(int modifier)
        {
            switch(modifier)
            {
                case 1:
                    return "Ruin";
                case 2:
                    return "Might";
                case 3:
                    return "Force";
                case 4:
                    return "Power";
                case 5:
                    return "Vanquishing";
                default:
                    return "Ruin";
            }
        }

        private static string getFirePrefix(int modifier, bool hasPrefix)
        {
            switch(modifier)
            {
                case 1:
                    return "Flaming";
                case 2:
                    return (!hasPrefix) ? "An Infernal" : "Infernal";
                case 3:
                    return "Burning";
                case 4:
                    return "Molten";
                case 5:
                    return (!hasPrefix) ? "An Incinerating" : "Incinerating";
                default:
                    return "Flaming";
            }
        }

        private static string getElePrefix(int modifier, bool hasPrefix)
        {
            switch (modifier)
            {
                case 1:
                    return (!hasPrefix) ? "An Electric" : "Electric";
                case 2:
                    return "Dazzling";
                case 3:
                    return "Thundering";
                case 4:
                    return "Striking";
                case 5:
                    return "Blinding";
                default:
                    return "Dazzling";
            }
        }

        private static string getColdPrefix(int modifier, bool hasPrefix)
        {
            switch (modifier)
            {
                case 1:
                    return (!hasPrefix) ? "An Icy" : "Icy";
                case 2:
                    return "Frozen";
                case 3:
                    return "Shivering";
                case 4:
                    return "Frosty";
                case 5:
                    return "Frigid";
                default:
                    return (!hasPrefix) ? "An Icy" : "Icy";
            }
        }

        private static string doClothSkillModifier(string itemName, int quality)
        {
            // Skill from skilltree
            string skill = SkillList[Dice.getRandom(0, SkillList.Count() - 1)];

            int modifier = determineMagicModifier(quality, true);

            // make fortified clothes more rare.. having 50% chance of lowering the tier by one
            if (modifier == 4 && Dice.getRandom(1, 2) != 1)
            {
                modifier--;
            }

            // make indestructible clothes more rare.. having 90% chance of lowering the tier by one
            if (modifier == 5 && Dice.getRandom(1, 10) != 1)
            {
                modifier--;
            }

            // return re-formatted cloth name
            return string.Format("{0} {1} Of {2}", GetQualityModifierName(modifier), itemName, skill);
        }

        private static string doJewelSkillModifier(string itemName, int quality)
        {
            

            // 1 out of 5 chance for magic jewel
            if (Dice.getRandom(1,5) == 1)
            {
                int modifier = (quality / 2) + Dice.getRandom(0,4);

                // cap modifier to 5
                if (modifier > 5)
                {
                    modifier = 5;
                }

                // skill from skilltree
                string skill = SkillList[Dice.getRandom(0, SkillList.Count() - 1)];

                return string.Format("{0} Of {1} ({2})", itemName, skill, GetQualityModifierName(modifier));

            }

            // no luck
            return itemName;
        }

        private static string doOreModifier(string itemName, int quality, bool hasQuality, int colorChance)
        {
            // will item have color
            bool hasColor = false;

            // what ore we will use
            int ore = 4; // 4 = iron

            // you'll have color chance of colorChance%
            if (Dice.getRandom(1, 100) <= colorChance)
            {
                hasColor = true;
            }

            if (hasColor)
            {
                // get random num between 1-quality so we can choose zone of ores to be chosen from
                int zoneRand = Dice.getRandom(1, quality);

                // if quality is between 1-30
                if (0 < zoneRand && zoneRand <= 30)
                {
                    ore = Dice.getRandom(1, 4); // get ore type between 1-4
                }

                // if quality is between 31-50
                if (30 < zoneRand && zoneRand <= 50)
                {
                    ore = Dice.getRandom(5, 10); // get ore type between 5-10
                }

                // if quality is between 51-70
                if (50 < zoneRand && zoneRand <= 70)
                {
                    ore = Dice.getRandom(11, 18); // get ore type between 11-18

                    // if ore is agapite, change it to silver insted
                    if(ore == 17)
                        ore = 31;

                    // if ore is malachite, change it to lunar instead
                    if (ore == 18)
                        ore = 32;
                }

                // if quality is between 71-90
                if(70 < zoneRand && zoneRand <= 90)
                {
                    ore = Dice.getRandom(17, 22); // get ore type between 17-22

                    // if ore is daemonspine, change it to solat instead
                    if (ore == 21)
                        ore = 29;

                    // if ore is snograz, change it to royal instead
                    if (ore == 22)
                        ore = 33;
                }


                // if quality is between 91-95
                if(90 < zoneRand && zoneRand <= 95)
                {
                    ore = Dice.getRandom(21, 25); // get ore type between 21-25

                    // if ore is wraiths bone, change it to startear instead
                    if (ore == 25)
                        ore = 30;
                }

                // if quality is between 96-98
                if (95 < zoneRand && zoneRand <= 98)
                {
                    ore = Dice.getRandom(25, 28); // get ore type between 25-28
                }

                // if quality is between 99-100
                if (98 < zoneRand && zoneRand <= 100)
                {
                    ore = Dice.getRandom(34, 48); // get ore type between 34-48
                }

            }


            switch (ore)
            {
                case 1:
                    return string.Format("{0} {1}", "Dull Copper", itemName);
                case 2:
                    return string.Format("{0} {1}", "Copper", itemName);
                case 3:
                    return string.Format("{0} {1}", "Bronze", itemName);
                case 4:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Iron" : "Iron", itemName);
                case 5:
                    return string.Format("{0} {1}", "Verite", itemName);
                case 6:
                    return string.Format("{0} {1}", "Syntian", itemName);
                case 7:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Azurite" : "Azurite", itemName);
                case 8:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Executor" : "Executor", itemName);
                case 9:
                    return string.Format("{0} {1}", "Radlius", itemName);
                case 10:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Aughlite" : "Aughlite", itemName);
                case 11:
                    return string.Format("{0} {1}", "Spirit Stone", itemName);
                case 12:
                    return string.Format("{0} {1}", "Devils Claw", itemName);
                case 13:
                    return string.Format("{0} {1}", "Shadow", itemName);
                case 14:
                    return string.Format("{0} {1}", "Gloom", itemName);
                case 15:
                    return string.Format("{0} {1}", "Devils Tooth", itemName);
                case 16:
                    return string.Format("{0} {1}", "Pyrite", itemName);
                case 17:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Agapite" : "Agapite", itemName);
                case 18:
                    return string.Format("{0} {1}", "Malachite", itemName);
                case 19:
                    return string.Format("{0} {1}", "Kryztal", itemName);
                case 20:
                    return string.Format("{0} {1}", "Golden", itemName);
                case 21:
                    return string.Format("{0} {1}", "Daemonspine", itemName);
                case 22:
                    return string.Format("{0} {1}", "Snograz", itemName);
                case 23:
                    return string.Format("{0} {1}", "Valorite", itemName);
                case 24:
                    return string.Format("{0} {1}", "Fatigue", itemName);
                case 25:
                    return string.Format("{0} {1}", "Wraiths Bone", itemName);
                case 26:
                    return string.Format("{0} {1}", "Scarletite", itemName);
                case 27:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Onyx" : "Onyx", itemName);
                case 28:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Empyrean" : "Empyrean", itemName);
                case 29:
                    return string.Format("{0} {1}", "Solar", itemName);
                case 30:
                    return string.Format("{0} {1}", "Startear", itemName);
                case 31:
                    return string.Format("{0} {1}", "Silver", itemName);
                case 32:
                    return string.Format("{0} {1}", "Lunar", itemName);
                case 33:
                    return string.Format("{0} {1}", "Royal", itemName);
                case 34:
                    return string.Format("{0} {1}", "Daemon's Fear", itemName);
                case 35:
                    return string.Format("{0} {1}", "Daemon Rock", itemName);
                case 36:
                    return string.Format("{0} {1}", "Daemon Skull", itemName);
                case 37:
                    return string.Format("{0} {1}", "Lhiolite", itemName);
                case 38:
                    return string.Format("{0} {1}", "Gredstinuel", itemName);
                case 39:
                    return string.Format("{0} {1}", "Dwarven Olupius", itemName);
                case 40:
                    return string.Format("{0} {1}", "Tekrhan", itemName);
                case 41:
                    return string.Format("{0} {1}", "Hekhranish", itemName);
                case 42:
                    return string.Format("{0} {1}", "Dwarfish Orkanian", itemName);
                case 43:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Aorkrhan" : "Aorkrhan", itemName);
                case 44:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Auropian" : "Auropian", itemName);
                case 45:
                    return string.Format("{0} {1}", "Shalrius", itemName);
                case 46:
                    return string.Format("{0} {1}", "Ghuronius", itemName);
                case 47:
                    return string.Format("{0} {1}", "Keltrunius", itemName);
                case 48:
                    return string.Format("{0} {1}", (!hasQuality) ? "An " : "" + "Altrintium", itemName);
                default:
                    return string.Format("{0} {1}", (!hasQuality) ? "An Iron" : "Iron", itemName);
            }


        }


        private static string doLeatherModifier(string itemName, int quality, bool hasQuality, int colorChance)
        {
            // will item have color
            bool hasColor = false;

            // what leather we will use
            int leather = 0; 

            // you'll have color chance of colorChance%
            if (Dice.getRandom(1, 100) <= colorChance)
            {
                hasColor = true;
            }

            // if we dont have color, nothing will be changed
            if(!hasColor)
            {
                return itemName;
            }

       

            int zoneRand = Dice.getRandom(0, quality);

            // if quality is between 0-30
            if (zoneRand <= 30)
            {
                leather = Dice.getRandom(1, 7); // random leather between 1-7
            }

            // if quality is between 31-50
            if (30 < zoneRand && zoneRand <= 50)
            {
                leather = Dice.getRandom(8, 17); // random leather between 8-17
            }

            // if quality is between 51-60
            if (50 < zoneRand && zoneRand <= 60)
            {
                leather = Dice.getRandom(35, 37); // random leather between 35-37
            }

            // if quality is between 61-70
            if (60 < zoneRand && zoneRand <= 70)
            {
                leather = Dice.getRandom(18, 20); // random leather between 18-20
            }

            // if quality is between 71-80
            if (70 < zoneRand && zoneRand <= 80)
            {
                leather = Dice.getRandom(31, 34); // random leather between 31-34
            }

            // if quality is between 81-90
            if (80 < zoneRand && zoneRand <= 90)
            {
                leather = Dice.getRandom(25, 30); // random leather between 25-30
            }

            // if quality is between 91-100
            if (90 < zoneRand && zoneRand <= 100)
            {
                leather = Dice.getRandom(21, 24); // random leather between 21-24
            }


            switch (leather)
            {
                case 1:
                    return string.Format("{0} {1}", "Rat Hide", itemName);
                case 2:
                    return string.Format("{0} {1}", "Troll Hide", itemName);
                case 3:
                    return string.Format("{0} {1}", "Lizard Hide", itemName);
                case 4:
                    return string.Format("{0} {1}", "Silver Hide", itemName);
                case 5:
                    return string.Format("{0} {1}", "Pig Hide", itemName);
                case 6:
                    return string.Format("{0} {1}", "Llama Hide", itemName);
                case 7:
                    return string.Format("{0} {1}", "Gorilla Hide", itemName);
                case 8:
                    return string.Format("{0} {1}", "Frenzied Ostard Hide", itemName);
                case 9:
                    return string.Format("{0} {1}", "Grizzly Bear Hide", itemName);
                case 10:
                    return string.Format("{0} {1}", "Gremlin Hide", itemName);
                case 11:
                    return string.Format("{0} {1}", (hasQuality) ? "An Imp Hide" : "Imp Hide", itemName);
                case 12:
                    return string.Format("{0} {1}", "Cyclop Hide", itemName);
                case 13:
                    return string.Format("{0} {1}", "Titan Hide", itemName);
                case 14:
                    return string.Format("{0} {1}", (hasQuality) ? "An Orc Hide" : "Orc Hide", itemName);
                case 15:
                    return string.Format("{0} {1}", "Lizardmanwarlock Hide", itemName);
                case 16:
                    return string.Format("{0} {1}", (hasQuality) ? "An Unicorn Hide" : "Unicorn Hide", itemName);
                case 17:
                    return string.Format("{0} {1}", "Dolphin Hide", itemName);
                case 18:
                    return string.Format("{0} {1}", "Fire Giant Hide", itemName);
                case 19:
                    return string.Format("{0} {1}", "Water Giant Hide", itemName);
                case 20:
                    return string.Format("{0} {1}", "Hill Giant Hide", itemName);
                case 21:
                    return string.Format("{0} {1}", "Stone Giant Hide", itemName);
                case 22:
                    return string.Format("{0} {1}", "Frost Giant Hide", itemName);
                case 23:
                    return string.Format("{0} {1}", "Storm Giant Hide", itemName);
                case 24:
                    return string.Format("{0} {1}", "Cloud Giant Hide", itemName);
                case 25:
                    return string.Format("{0} {1}", "Red Dragon Hide", itemName);
                case 26:
                    return string.Format("{0} {1}", "Grey Dragon Hide", itemName);
                case 27:
                    return string.Format("{0} {1}", "White Dragon Hide", itemName);
                case 28:
                    return string.Format("{0} {1}", "Black Dragon Hide", itemName);
                case 29:
                    return string.Format("{0} {1}", "Green Dragon Hide", itemName);
                case 30:
                    return string.Format("{0} {1}", "Royal Dragon Hide", itemName);
                case 31:
                    return string.Format("{0} {1}", "Red Daemon Hide", itemName);
                case 32:
                    return string.Format("{0} {1}", "Black Daemon Hide", itemName);
                case 33:
                    return string.Format("{0} {1}", (hasQuality) ? "An Ice Fiend Hide" : "Ice Fiend Hide", itemName);
                case 34:
                    return string.Format("{0} {1}", "Fire Fiend Hide", itemName);
                case 35:
                    return string.Format("{0} {1}", "Nightmare Hide", itemName);
                case 36:
                    return string.Format("{0} {1}", "Seaserpent Hide", itemName);
                case 37:
                    return string.Format("{0} {1}", "Polar Bear Hide", itemName);
            }

            // dummy return, should never reach here
            return null;
        }

        private static string doBoneModifier(string itemName, int quality, bool hasPrefix, int colorChance)
        {
            // will item have color
            bool hasColor = false;

            // what leather we will use
            int bone = 0;

            // you'll have color chance of colorChance%
            if (Dice.getRandom(1, 100) <= colorChance)
            {
                hasColor = true;
            }

            // if we dont have color, nothing will be changed
            if (!hasColor)
            {
                return itemName;
            }



            int zoneRand = Dice.getRandom(0, quality);

            // if quality is between 1-30
            if(zoneRand <= 30)
            {
                bone = Dice.getRandom(1, 4); // get bone type between 1-4
            }

            // if quality is between 31-50
            if (30 < zoneRand && zoneRand <= 50)
            {
                bone = Dice.getRandom(5, 7); // get bone type between 5-7
            }

            // if quality is between 51-80
            if(50 < zoneRand && zoneRand <= 80)
            {
                bone = Dice.getRandom(8, 11); ; // get bone type between 8-11
            }

            // if quality is btween 81-90
            if(80 < zoneRand && zoneRand <= 90)
            {
                bone = 12; // bonetype will be 12
            }

            //if quality is between 91-100
            if(90 < zoneRand && zoneRand <= 100)
            {
                bone = Dice.getRandom(13, 14); // get bone type between 13-14
            }


            switch (bone)
            {
                case 1:
                    return string.Format("{0} {1}", "Rat Bone", itemName);
                case 2:
                    return string.Format("{0} {1}", "Spider Bone", itemName);
                case 3:
                    return string.Format("{0} {1}", (hasPrefix) ? "An Orc Bone" : "Orc Bone", itemName);
                case 4:
                    return string.Format("{0} {1}", "Lizard Bone", itemName);
                case 5:
                    return string.Format("{0} {1}", "Troll Bone", itemName);
                case 6:
                    return string.Format("{0} {1}", "Large Humanoid Bone", itemName);
                case 7:
                    return string.Format("{0} {1}", "Gargoyle Bone", itemName);
                case 8:
                    return string.Format("{0} {1}", "Vampire Bone", itemName);
                case 9:
                    return string.Format("{0} {1}", (hasPrefix) ? "An Enchancted Bone" : "Enchanted Bone", itemName);
                case 10:
                    return string.Format("{0} {1}", "Terathan Bone", itemName);
                case 11:
                    return string.Format("{0} {1}", (hasPrefix) ? "An Ophidian Bone" : "Ophidian Bone", itemName);
                case 12:
                    return string.Format("{0} {1}", "Daemon Bone", itemName);
                case 13:
                    return string.Format("{0} {1}", "Phoenix Bone", itemName);
                case 14:
                    return string.Format("{0} {1}", "Dragon Bone", itemName);
            }

            // dummy return, should never reach here
            return null;
        }


        private static string makeArmorMagical(MagicAllowed.MagicAllow magicProps, int quality)
        {
            // quality of an item (subs - indy)
            int qualityModifier = determineMagicModifier(quality, true);

            // suffix of an item (defense - invul)
            int suffixModifier = determineMagicModifier(quality,true);

            // effect quality
            int effectModifier = determineMagicModifier(quality, false);


            bool hasQuality = false;
            bool hasSuffix = false;
            
            // you'll have 1 out 6 chance of having quality on an item
            if(Dice.getRandom(1,6) == 1)
            {
                hasQuality = true;
            }

            string itemName = magicProps.Name;

            // you'll have 1 out of 10 chance of having suffix on an item
            if(Dice.getRandom(1,10) == 10)
            {
                hasSuffix = true;
            }
            
            
            switch (magicProps.Effect)
            {
                case "ore":
                    // try and add ore modifier to an item
                    itemName = doOreModifier(itemName, effectModifier, hasQuality, (magicProps.MagicType == "armor") ? ARMOR_ORE_COLOR_CHANCE : SHIELD_ORE_COLOR_CHANCE);
                    break;
                case "leather":
                    // try and add leather modifier to an item
                    itemName = doLeatherModifier(itemName, effectModifier, hasQuality, ARMOR_LEATHER_COLOR_CHANCE);
                    break;
                case "bone":
                    // try and add bone modifier to an item
                    itemName = doBoneModifier(itemName, effectModifier, hasQuality, ARMOR_BONE_COLOR_CHANCE);
                    break;
            }
            
            
            // if we have suffix to be added
            if (hasSuffix)
            {
                if (magicProps.MagicType == "armor")
                {
                    // make forti armors more rare.. having 50% chance of lowering the tier by one
                    if (suffixModifier == 4 && Dice.getRandom(1, 2) != 1)
                    {
                        suffixModifier--;
                    }

                    // make invul armors more rare.. having 90% chance of lowering the tier by one
                    if (suffixModifier == 5 && Dice.getRandom(1, 10) != 1)
                    {
                        suffixModifier--;
                    }
                }

                if (magicProps.MagicType == "shield")
                {
                    // make forti and invul shields more rare, having 90% chance of lowering by 2 tiers
                    if(suffixModifier >= 5 && Dice.getRandom(1,10) != 1)
                    {
                        suffixModifier -= 2;
                    }
                }

                // add suffix to the item name
                itemName = string.Format("{0} Of {1}", itemName, GetAmorSuffix(suffixModifier));

            }

            if (hasQuality)
            {
                itemName = string.Format("{0} {1}", GetQualityModifierName(qualityModifier), itemName);
            }

            // return item name
            return itemName;
        }

        private static int determineMagicModifier(int quality, bool divideByHundred)
        {
            // base quality is 25% determine and 75% from random chance.. (ranging from 0 to 100)
            int baseQuality = (quality * 10 / 4) + Dice.getRandom(0, (quality * 30 / 4));

            // random quality, making it possible to always have a chance for good item, though unelikely, with extra 10 for luck
            int randomQuality = (Dice.getRandom(1,10) * Dice.getRandom(1,10)) + 10;


            // add 25 to base quality

            int magicQuality = baseQuality + 25;

            if (randomQuality > magicQuality)
            {
                if (Dice.getRandom(1,4) == 1)
                {
                    // if you're lucky enough already to get higher random quality value than base then
                    // let's see if you have more luck and get this value and add extra 15
                    magicQuality = randomQuality + 15;
                }
            }
            else if (Dice.getRandom(1, 10) == 1)
            {
                // if you're especially unlucky you'll get lowered down to random quality + 15
                magicQuality = randomQuality + 15;
            }

            // cap magic quality to 100
            if (magicQuality > 100)
            {
                magicQuality = 100;
            }

            // return modifier num     
            int modifier;

            
            if (divideByHundred)
            {
                // modifier can be 1-5 with a 50% chance of being 1 tier better
                modifier = (magicQuality / 20) + Dice.getRandom(0, 1);
            }
            else
            {
                // some functions use 1-100 instead
                modifier = magicQuality;
            }

            if (modifier <= 0)
            {
                // modifier can be less than 1 in-case of low magic quality or
                // if you're especially unlucky, set to minimum of 1
                modifier = 1;
            }


            return modifier;
        }

        private static string addCharges(MagicAllowed.MagicAllow magicProps, int quality)
        {
            string itemName = magicProps.Name;

            switch (magicProps.MagicType)
            {
                case "wand":
                    int chargeQuality = determineMagicModifier(quality, false);
                    int wandCharges = 0;

                    int wandType = Dice.getRandom(1, 17); // get wand type between 1-16

                    switch (wandType)
                    {
                        case 1:
                            wandCharges = (chargeQuality / 3) + 10;
                            return string.Format("{0} {1} ({2})", itemName, "Of Nightsight", wandCharges);
                        case 2:
                            wandCharges = (chargeQuality / 5) + 10;
                            return string.Format("{0} {1} ({2})", itemName, "Of Blessing", wandCharges);
                        case 3:
                            wandCharges = (chargeQuality / 5) + 10;
                            return string.Format("{0} {1} ({2})", itemName, "Of Strength", wandCharges);
                        case 4:
                            wandCharges = (chargeQuality / 5) + 10;
                            return string.Format("{0} {1} ({2})", itemName, "Of Agility", wandCharges);
                        case 5:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 2;
                            return string.Format("{0} {1} ({2})", itemName, "Of Cunning", wandCharges);
                        case 6:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 2;
                            return string.Format("{0} {1} ({2})", itemName, "Of Curing", wandCharges);
                        case 7:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 2;
                            return string.Format("{0} {1} ({2})", itemName, "Of Protection", wandCharges);
                        case 8:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 2;
                            return string.Format("{0} {1} ({2})", itemName, "Of Healing", wandCharges);
                        case 9:
                        case 10:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 5;
                            return string.Format("{0} {1} ({2})", itemName, "Of Life", wandCharges);
                        case 11:
                        case 12:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 5;
                            return string.Format("{0} {1} ({2})", itemName, "Of Fireball", wandCharges);
                        case 13:
                        case 14:
                        case 15:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 20) / 5;
                            return string.Format("{0} {1} ({2})", itemName, "Of Lightning", wandCharges);
                        case 16:
                        case 17:
                            wandCharges = (Dice.getRandom(0, chargeQuality) + 10) / 5;
                            return string.Format("{0} {1} ({2})", itemName, "Of Hellfire", wandCharges);
                    }
                    break;                 
                case "tool":
                    // making tools more rare, 10% chance of actually getting one...
                    if (Dice.getRandom(1, 100) <= MAGIC_TOOL_CHANCE)
                    {
                        int toolModifier = determineMagicModifier(quality, true);

                        // higher modifier equals less charges
                        int toolCharges = 300 - (toolModifier * 30) - Dice.getRandom(0, toolModifier * 40);

                        // limit lowest charges to 10
                        if (toolCharges < 10)
                        {
                            toolCharges = 10;
                        }

                        return string.Format("{0} {1} ({2})", GetQualityModifierName(toolModifier), itemName, toolCharges);
                    }
                    else
                    {
                        return itemName; // Tool won't be modifed to magic
                    }
            }

            // should never reach here is config is correct
            return null;
        }

        

        private static string doInstrumentModifier(string itemName, int quality)
        {
            int instrumentModifier = determineMagicModifier(quality, true);
            return string.Format("{0} Of {1}", itemName, getInstrumentSuffix(instrumentModifier));
        }

        private static string doWoodModifier(string itemName, int quality, bool hasPrefix, int colorChance)
        {
            // will item have color
            bool hasColor = false;

            // what leather we will use
            int wood = 0;

            // you'll have color chance of colorChance%
            if (Dice.getRandom(1, 100) <= colorChance)
            {
                hasColor = true;
            }

            // if we dont have color, nothing will be changed
            if (!hasColor)
            {
                return itemName;
            }

            // get random number between 1 - max quality
            int zoneRand = Dice.getRandom(1, quality);

            // if quality is between 1-25
            if(0 < zoneRand && zoneRand <= 25)
            {
                wood = 1; // WalnutLog
            }

            // if quality is between 26-60
            if (25 < zoneRand && zoneRand <= 60)
            {
                wood = 2; // CypressLog
            }

            // if quality is between 61-80
            if (60 < zoneRand && zoneRand <= 80)
            {
                wood = 3; // OakLog
            }

            // if quality is between 81-100
            if (80 < zoneRand && zoneRand <= 100)
            {
                wood = 4; // YewLog
            }


            switch (wood)
            {
                case 1:
                    return string.Format("{0} {1}", "Walnut", itemName);
                case 2:
                    return string.Format("{0} {1}", "Cypress", itemName);
                case 3:
                    return string.Format("{0} {1}", (!hasPrefix) ? "An Oak" : "Oak", itemName);
                case 4:
                    return string.Format("{0} {1}", "Yew", itemName);
            }


            // dummy return, should never reach here
            return null;
        }

        private static string doElementalModifier(int modifier, bool hasPrefix)
        {
            // choose randomly between 3 elements
            switch (Dice.getRandom(1,3))
            {
                case 1:
                   return getFirePrefix(modifier, hasPrefix);
                case 2:
                   return getElePrefix(modifier, hasPrefix);
                case 3:
                   return getColdPrefix(modifier, hasPrefix);
                default:
                   return getFirePrefix(modifier, hasPrefix);
            }
        }

        private static string makeWeaponMagical(MagicAllowed.MagicAllow magicProps, int quality)
        {
            string itemName = magicProps.Name;

            // get weapon(quality) modifier
            int weaponModifier = determineMagicModifier(quality, true);

            // remember 75% of determineMagicModifer is random, so we'll
            // generate new number for suffix modifier
            int suffixModifier = determineMagicModifier(quality, true);

            // remember 75% of determineMagicModifer is random, so we'll
            // generate new number for elemental modifier
            int elementalModifier = determineMagicModifier(quality, true);

            int effectQuality = determineMagicModifier(quality, false);

            bool hasQuality = false;

            bool hasElementalMod = false;

            bool hasSuffix = false;

            // 1 out of 15 chance to get only ore modification, otherwise try and add mod's
            if(Dice.getRandom(1,15) == 1 && magicProps.Effect == "ore")
            {
                return doOreModifier(itemName, quality, false, 100);
            }
            else
            {
                // ----- 1st modifier --------
                // 2 out of 5 chance for quality,
                // 1 out of 5 for elemental modifier,
                // 2 out of 5 for suffix
                int rand = Dice.getRandom(1, 5);

                switch (rand)
                {
                    case 1:
                    case 2:
                        // 1-2 == quality
                        hasQuality = true;
                        break;
                    case 3:
                        // 3 == elemental
                        hasElementalMod = true;
                        break;
                    default:
                        // 4-5 == suffix
                        hasSuffix = true;
                        break;
                }

                // ----- 2nd modifier --------
                // 1 out of 7 chance for having extra upgrade
                if (Dice.getRandom(1, 7) == 7)
                {
                    // goes same, do until we have value that was not added with 1st modifier
                    // 2 out of 5 chance for quality,
                    // 1 out of 5 for elemental modifier,
                    // 2 out of 5 for suffix
                    bool correctValueFound = false;
                    while (!correctValueFound)
                    {
                        rand = Dice.getRandom(1, 5);

                        switch (rand)
                        {
                            case 1:
                            case 2:
                                if (!hasQuality)
                                    correctValueFound = true; // it did not get quality with 1st modifier, add it now
                                break;
                            case 3:
                                if (!hasElementalMod)
                                    correctValueFound = true; // it did not get element with 1st modifier, add it now
                                break;
                            default:
                                if (!hasSuffix)
                                    correctValueFound = true; // it did not get suffix with 1st modifier, add it now
                                break; // neither mod will be added
                        }
                    }
                }


                // 1 out of 10 chance of getting all upgrades
                if (Dice.getRandom(1, 10) == 10)
                {
                    hasQuality = true;
                    hasElementalMod = true;
                    hasSuffix = true;
                }

                switch (magicProps.Effect)
                {
                    case "ore":
                        // try and add ore modification
                        itemName = doOreModifier(itemName, effectQuality, (hasQuality || hasElementalMod) ? true : false, WEAPON_ORE_COLOR_CHANCE);
                        break;
                    case "wood":
                        // try and add wood modification
                        itemName = doWoodModifier(itemName, effectQuality, (hasQuality || hasElementalMod) ? true : false, WEAPON_WOOD_COLOR_CHANCE);
                        break;
                }
                

                if (hasElementalMod)
                {
                    itemName = string.Format("{0} {1}", doElementalModifier(elementalModifier, hasQuality), itemName);
                }

                if (hasQuality)
                {
                    // check if we had luck with color roll
                    if (!hasElementalMod)
                    {
                        itemName = string.Format("{0} {1}", GetQualityModifierName(weaponModifier), itemName);
                    } 
                    else
                    {
                        itemName = string.Format("{0} And {1}", GetQualityModifierName(weaponModifier), itemName);
                    }
                }

                if (hasSuffix)
                {
                    itemName = string.Format("{0} Of {1}", itemName, getWeaponSuffix(suffixModifier));
                }


                // Will we add charges? 1 out of 10
                if(Dice.getRandom(1,10) == 1)
                {

                    string chargeSuffix = "";
                    int charges = 0;

                    // roll of what charges will be applied
                    switch(Dice.getRandom(1,14))
                    {
                        case 1:
                        case 2:
                            chargeSuffix = "Feeblemindedness";
                            charges = 30 + Dice.getRandom(1, quality);
                            break;
                        case 3:
                        case 4:
                            chargeSuffix = "Clumsiness";
                            charges = 30 + Dice.getRandom(1, quality);
                            break;
                        case 5:
                        case 6:
                            chargeSuffix = "Weakness";
                            charges = 30 + Dice.getRandom(1, quality);
                            break;
                        case 7:
                            chargeSuffix = "Harming";
                            charges = 20 + Dice.getRandom(1, quality/2);
                            break;
                        case 8:
                            chargeSuffix = "Flametoungue";
                            charges = 20 + Dice.getRandom(1, quality / 2);
                            break;
                        case 9:
                        case 10:
                            chargeSuffix = "Cursing";
                            charges = 20 + Dice.getRandom(1, quality);
                            break;
                        case 11:
                            chargeSuffix = "Lightning";
                            charges = 20 + Dice.getRandom(1, quality/2);
                            break;
                        case 12:
                            chargeSuffix = "Paralyzation";
                            charges = 20 + Dice.getRandom(1, quality / 2);
                            break;
                        case 13:
                            chargeSuffix = "Dispelling";
                            charges = 20 + Dice.getRandom(1, quality);
                            break;
                        case 14:
                            chargeSuffix = "Draining";
                            charges = 20 + Dice.getRandom(1, quality);
                            break;
                    }

                    if (hasSuffix)
                    {
                        itemName = string.Format("{0} And {1} ({2})", itemName, chargeSuffix, charges);
                    }
                    else
                    {
                        itemName = string.Format("{0} Of {1} ({2})", itemName, chargeSuffix, charges);
                    }
                }
            }

            return itemName;
        }


        public static string TurnIntoMagic(MagicAllowed.MagicAllow magicProps, int quality)
        {
            switch (magicProps.MagicType)
            {
                case "cloth":
                    return doClothSkillModifier(magicProps.Name, quality);
                case "armor":
                case "shield":
                    return makeArmorMagical(magicProps, quality);
                case "instrument":
                    return doInstrumentModifier(magicProps.Name, quality);
                case "tool":
                case "wand":
                    return addCharges(magicProps, quality);
                case "weapon":
                    return makeWeaponMagical(magicProps, quality);
                case "jewel":
                    return doJewelSkillModifier(magicProps.Name, quality);

            }       

            return null;
        }


        public static MagicAllowed.MagicAllow MagicAllowedAndType(string itemName)
        {
            string obj = itemName;                    

            foreach( var allowed in MagicAllowed.AllowedMagicItems )
            {
                if (allowed.Name.ToString().Trim().ToLower() == itemName.Trim().ToLower())
                {
                    if(!string.IsNullOrWhiteSpace(allowed.MagicType))
                    {
                        return allowed;
                    }
                }
            }

            return null;
        }

    }
}
