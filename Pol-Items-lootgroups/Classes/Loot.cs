using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pol_Items_lootgroups.Classes
{
    class Loot
    {
        public string Name { get; set; }
        public string Gold { get; set; }
        public int MagicQuality { get; set; }
        public int ChestChance { get; set; }
        public int ChestQuality { get; set; }
        public int Head { get; set; }
        public int Blood { get; set; }

        public List<StandardItem> items = new List<StandardItem>();
        public List<MagicItem> magicItems = new List<MagicItem>();
        public List<RandomItemFromGroup> randItemFromGroup = new List<RandomItemFromGroup>();
        public List<RandomMagicItemFromGroup> randMagicItemFromGroup = new List<RandomMagicItemFromGroup>();
        public List<Skillbook> skillBooks = new List<Skillbook>();
        public List<CutUp> cutUps = new List<CutUp>();
    }

    class StandardItem
    {   
        public string Name { get; set; }
        public string UpToHowMuchDropped { get; set; }
        public int ChanceOfDropPerItem { get; set; }
        public bool CamelCaseOK { get; set; } = false;
    }

    class MagicItem
    {
        public string Name { get; set; }
        public int ChanceOfDrop { get; set; }
        public bool CamelCaseOK { get; set; } = false;
    }

    class RandomItemFromGroup : StandardItem
    {
    }

    class RandomMagicItemFromGroup
    {
        public string Name { get; set; }
        public int ChanceOfDrop { get; set; }
        public bool CamelCaseOK { get; set; } = false;
    }

    class Skillbook
    {
        public int skillTier { get; set; }
        public int ChanceOfDrop { get; set; }
        public bool CamelCaseOK { get; set; } = false;
    }

    class CutUp
    {
        public string ItemName { get; set; }
        public string Amount { get; set; }
        public bool CamelCaseOK { get; set; } = false;
    }

}
