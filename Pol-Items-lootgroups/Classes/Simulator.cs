using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pol_Items_lootgroups.Classes
{ 
  

    static class Simulator
    {
        // inherithing CutUp class, since i already has Name and Amount attribut what we need.
        // just chance amount from string to int...
        private class ItemStack : CutUp
        {
            virtual new public int Amount { get; set; }
        }

        const decimal GOLD_MULTIPLIER_DUNG = 1.45m;
        const decimal GOLD_MULTIPLIER_OUT = 0.75m;

        static private bool _debug = false;

        private static List<String> _droppedLoot = new List<string>();
        private static List<String> _warnings = new List<string>();
        private static List<ItemGroup> _itemGroups = null;
        private static List<Item> _items = null;
        
        // generate loot stacks here...
        private static List<ItemStack> _itemStacks = new List<ItemStack>();
       

        private static ItemGroup getItemGroupByName(string groupName)
        {
            foreach (var group in _itemGroups)
            {
                if (group.Name == groupName)
                {
                    return group;
                }
            }

            return null;
        }

        public static string getItemHex(string itemName)
        {
            foreach(var item in _items)
            {
                if (item.getName() == itemName)
                {
                    return item.getObjNumberHex();
                }
            }

            return null;
        }

        public static bool isValidItem(string itemName)
        {
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    if (item.getName().Trim() == itemName.Trim())
                    {
                        return true;
                    }

                    if (item.getObjNumberHex().ToLower() == itemName.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }

            return true;
        }

        private static void addToStack(string objName, int amount)
        {
            foreach( var item in _itemStacks)
            {
                if (item.ItemName == objName)
                {
                    item.Amount += amount;
                    return;
                }
            }

            // If we reach here the item was not found, just add it as new..
            ItemStack tempStack = new ItemStack();
            tempStack.ItemName = objName;
            tempStack.Amount = amount;
            _itemStacks.Add(tempStack);

        }

        private static void transferStacksToLoot()
        {
            foreach(var stack in _itemStacks)
            {
                _droppedLoot.Add(string.Format("{0}({1})", stack.ItemName, stack.Amount));
            }

            _itemStacks.Clear();
        }


        public static void MakeLoot(Loot template, List<ItemGroup> itemGroups = null, List<Item> items = null)
        {
            ///////// Reset loot drop //////////
            _droppedLoot.Clear();
            _warnings.Clear();
            
            ///////// Populate items //////////
            if(items == null)
            {
                _warnings.Add("No items loaded, so no validation check.");
            }
            else
            {
                _items = items;
            }

            if (itemGroups != null)
            {
                // Populate global itemgroups
                _itemGroups = itemGroups;
            }
            else
            {
                _warnings.Add("Item Groups are not loaded!");
            }

            ///////// Let's generate gold ////////////

            if (!string.IsNullOrWhiteSpace(template.Gold) && Dice.isValidDieString(template.Gold))
            {
                int goldSum = Dice.Roll(template.Gold);


                int goldDung = Convert.ToInt32(GOLD_MULTIPLIER_DUNG * goldSum);
                int goldOut = Convert.ToInt32(GOLD_MULTIPLIER_OUT * goldSum);

                _droppedLoot.Add(string.Format("{0}{1}", "Gold Dungeon: ", goldDung));
                _droppedLoot.Add(string.Format("{0}{1}", "Gold Outside: ", goldOut));
            }

            ///////// Done with gold /////////


            //////// Do we make locked chest? /////////
            if (template.ChestQuality > 0)
            {
                MakeLockedChest(template.ChestQuality, template.ChestChance);
            }
            //////// End of locked chest /////////


            //////// Do we make skill books? /////////
            if (template.skillBooks.Count > 0)
            {
                foreach (var skillBook in template.skillBooks)
                {
                    MakeSkillBook(skillBook.skillTier, skillBook.ChanceOfDrop);
                }
                
            }
            //////// End of skill books /////////


            //////// Magic items /////////

            if (template.MagicQuality > 0)
            {
                if (template.randMagicItemFromGroup.Count > 0)
                {
                    if (_itemGroups != null)
                    {
                        foreach (var group in template.randMagicItemFromGroup)
                        {
                            MakeMagicItem(group.Name, template.MagicQuality, group.ChanceOfDrop);
                        }
                    }
                }
                else
                {
                    _warnings.Add("Template has magic quality but no magic items! Variable is useless!");
                }

            }
            else
            {
                if (template.randMagicItemFromGroup.Count > 0)
                {
                    _warnings.Add("Template has magic quality of 0 or null but contains magic item drops (Won't be dropped)");
                }
            }

            //////// End of magic items /////////

            //////// Random item out of group /////////

            if (_itemGroups != null)
            {               

                if (template.randItemFromGroup.Count > 0)
                {
                    foreach (var group in template.randItemFromGroup)
                    {
                        MakeItemOutOfGroup(group.Name, group.UpToHowMuchDropped, group.ChanceOfDropPerItem, false);
                    }
                    //transferStacksToLoot();
                }
            }           

            //////// End of random out of group /////////



            //////// Make specified loot /////////

            if (template.items.Count() > 0)
            {
                foreach (var item in template.items)
                {
                    MakeSpecifiedLoot(item.Name, item.UpToHowMuchDropped, item.ChanceOfDropPerItem, true);
                }
                //transferStacksToLoot();
            }

            //////// End of specified loot /////////                 

            // Add total stacks of specific items and random from group to loot
            transferStacksToLoot();
           

            //////// On corpse cut /////////

            if (template.cutUps.Count() > 0)
            {
                _droppedLoot.Add("");
                _droppedLoot.Add("On_corpse_cut");
                _droppedLoot.Add("{");
                foreach (var item in template.cutUps)
                {
                    MakeSpecifiedLoot("\t" + item.ItemName, item.Amount, 100, false);
                }
                transferStacksToLoot();
                _droppedLoot.Add("}");
            }

            //////// end of corpse cut /////////
        }

        private static void MakeMagicItem(string group, int quality, int chance)
        {
            if (Dice.getRandom(1, 100) <= chance)
            {
                ItemGroup itemGroup = getItemGroupByName(group);

                if (itemGroup == null)
                {
                    _warnings.Add("Magic: Could not find item group of '" + group + "'");
                    return;
                }

                

                MagicAllowed.MagicAllow magicProps = null;
                int iterations = 0;

                string nameOfItemToBeDropped = "";

                // run the loop until finding the item that is allowed to be magic and return what type of magic
                while (magicProps == null)
                {
                    int itemOutOfGroup = Dice.getRandom(0, itemGroup.Items.Count() - 1);
                    nameOfItemToBeDropped = itemGroup.Items[itemOutOfGroup];

                    if (!isValidItem(nameOfItemToBeDropped))
                    {
                        _warnings.Add(string.Format("Magic: Item '{0}' in item group '{1}' is not valid!", nameOfItemToBeDropped, itemGroup.Name));
                    }
                    else
                    {
                        magicProps = Magic.MagicAllowedAndType(itemGroup.Items[itemOutOfGroup]);                       
                    }

                    iterations++;
                    if (iterations >= 100)
                    {
                        _warnings.Add("Could not find allowed magic item in 100 iterations in item group '" + itemGroup.Name + "' latest item '" + nameOfItemToBeDropped +"'");
                        _warnings.Add("dbg: Items in MagicAllowed.AllowedMagicItems " + MagicAllowed.AllowedMagicItems.Count());
                        //iterations = 0;
                        //magicProps = null;
                        return;
                    }

                }

                if (magicProps != null)
                {
                    string eventualItemName = Magic.TurnIntoMagic(magicProps, quality);

                    if (eventualItemName != null)
                    {
                        _droppedLoot.Add(eventualItemName);
                    }
                    else
                    {
                        _warnings.Add(string.Format("Magic: Item '{0}' in item group '{1}' has configuration error!", nameOfItemToBeDropped, itemGroup.Name));
                    }
                }

                
            }
        }

        private static void MakeSpecifiedLoot(string objName, string amount, int chance, bool limitedToTwenty)
        {
            if(!isValidItem(objName))
            {
                if (limitedToTwenty)
                {                  
                    _warnings.Add(string.Format("Item '{0}' is not valid!'", objName));
                }
                else
                {
                    _warnings.Add(string.Format("CutUp '{0}' is not valid!'", objName.Trim()));
                }
                
                return;
            }

            if (Dice.isValidDieString(amount))
            {
                int literalAmount = Dice.Roll(amount);

                if (limitedToTwenty)
                {
                    // Stupid clause imo...
                    if (literalAmount > 20)
                    {
                        literalAmount = 20;
                    }
                }

                // Drop chance is per 1 item dropped, so it goes trough them one-by-one
                for (int i = 1; i <= literalAmount; i++)
                {
                    if (Dice.getRandom(1, 100) <= chance)
                    {
                        //_droppedLoot.Add(string.Format("{0}({1})", objName, simAmount));
                        addToStack(objName, 1);
                    }

                }

            }
            else
            {
                _warnings.Add("Dice string on " + objName + " is invalid");
            }
        }

        private static void MakeSkillBook(int tier, int chance)
        {
            // will we succeed?
            if (Dice.getRandom(1, 100) <= chance)
            {
                // random starts from 0 here, since arrays in c# start from location [0] and minus 1 since Count sum of literal Count
                // and due array starts from 0 we'll get number shift of 1
                int skillVal = Dice.getRandom(0, Magic.SkillList.Count() - 1);

                string skillPrefix = Magic.SkillList[skillVal];

                switch (tier)
                {
                    case 1:
                        _droppedLoot.Add(string.Format("{0} HandBook Of {1}", "Apprentice", skillPrefix));
                        break;
                    case 2:
                        _droppedLoot.Add(string.Format("{0} HandBook Of {1}", "Expert", skillPrefix));
                        break;
                    case 3:
                        _droppedLoot.Add(string.Format("{0} HandBook Of {1}", "Master", skillPrefix));
                        break;
                    case 4:
                        _droppedLoot.Add(string.Format("{0} HandBook Of {1}", "Legendary", skillPrefix));
                        break;
                    default:
                        _warnings.Add(string.Format("There is a skill book with Tier({0}) where's only 1-4 are allowed!",tier));
                        break;
                }
            }
        }

        private static void MakeItemOutOfGroup(string itemGroup, string dice, int chance, bool lockedChest)
        {
            if (!Dice.isValidDieString(dice))
            {
                _warnings.Add("Dice string on " + itemGroup + " is invalid");
                return;
            }

            int literalAmount = Dice.Roll(dice);

            // limit it to 100 items out of group
            if (literalAmount > 100)
            {
                literalAmount = 100;
            }

            ItemGroup group = getItemGroupByName(itemGroup);

            if (group == null)
            {
                _warnings.Add("Random: Could not find item group of '" + itemGroup + "'");
                return;
            }


            for (int i = 1; i <= literalAmount; i++)
            {
                // random starts from 0 here, since arrays in c# start from location [0] and minus 1 since Count sum of literal Count
                // and due array starts from 0 we'll get number shift of 1
                int itemOutOfGroup = Dice.getRandom(0, group.Items.Count() - 1);

                if (Dice.getRandom(1, 100) <= chance)
                {
                    if (!isValidItem(group.Items[itemOutOfGroup]))
                    {
                        if (_debug)
                        {
                            if (MessageBox.Show("Will we debug '" + group.Items[itemOutOfGroup] + "'?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string debug = "// Debug -- itemgroup\r\n{\r\n";
                                foreach (char c in group.Items[itemOutOfGroup])
                                {
                                    debug += string.Format("\tint({0}) char('{1}')", (int)c, c) + "\r\n";
                                }
                                debug += string.Format("\t len({0}) indexInGroup({1})\r\n", group.Items[itemOutOfGroup].Length, itemOutOfGroup);
                                debug += "}";
                                Clipboard.SetText(debug);
                            }
                        }

                        _warnings.Add(string.Format("Item '{0}' in group '{1}' is not valid!", group.Items[itemOutOfGroup], group.Name));

                    }
                    else
                    {
                        addToStack(group.Items[itemOutOfGroup], 1);
                    }
                }
            }

        }

        

        private static void MakeLockedChest(int quality, int chance)
        {
            if (Dice.getRandom(1, 100) > chance)
            {
                // No luck, locked chest will be not made
                return;
            }

            int baseQuality = (quality / 2) + (Dice.getRandom(1, (quality / 2)));
            int randomQuality = ((Dice.getRandom(1, 10) * Dice.getRandom(1,10) / 10) + 1);

            if (randomQuality > baseQuality)
            {
                // got lucky and we got free upgrade on chest.
                baseQuality = randomQuality;
            }

            // now let's limit that upgrade to maximum of 2 tiers
            if (baseQuality > (quality + 2))
            {
                baseQuality = (quality + 2);
            }

            // Highest tier chest is 10, so we'll limit quality to 10
            if (baseQuality > 10)
            {
                baseQuality = 10;
            }

            // now let's see if we'll add trap to that chest aswell

            if (Dice.getRandom(1, 3) == 1)
            {
                // yep, let's add the trap
                switch (Dice.getRandom(1, 3))
                {
                    case 1:
                        _droppedLoot.Add(string.Format("{0} Tier({1}) Trap({2})", "a locked chest", baseQuality, "explosion"));
                        break;
                    case 2:
                        _droppedLoot.Add(string.Format("{0} Tier({1}) Trap({2})", "a locked chest", baseQuality, "poison"));
                        break;
                    case 3:
                        _droppedLoot.Add(string.Format("{0} Tier({1}) Trap({2})", "a locked chest", baseQuality, "djinni"));
                        break;
                }
            }
            else
            {
                _droppedLoot.Add(string.Format("{0} Tier({1}) Trap({2})", "a locked chest", baseQuality, "no trap"));
            }
        }


        public static List<string> getLootInfo()
        {
            return _droppedLoot;
        }

        public static List<string> getWarnings()
        {
            return _warnings;
        }
    }
}
