using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pol_Items_lootgroups.Classes
{
    class Item
    {
        private string _objNumberHex { get; set; }
        private string _itemName { get; set; }
        private string _itemPackage { get; set; }
        private bool _isMagicAllowed { get; set; }

        // setup the variables from array
        public bool fromArray(string[] elements)
        {
            try
            {
                _objNumberHex = elements[0].Trim();
                _itemName = elements[1].Trim();
                if (elements.Count() != 2)
                {
                    _itemPackage = elements[2].Trim();
                }
                return true;
            }
            catch ( Exception ex)
            {
                MessageBox.Show("Error when trying to load item from array! Stack error: " + ex.Message);
                return false;
            }
        }

        public void setMagicAllowed(bool isAllowed)
        {
            _isMagicAllowed = isAllowed;
        }

        public bool getMagicAllowed()
        {
            return _isMagicAllowed;
        }

        // Get item information
        public string getName()
        {
            return _itemName;
        }


        public string getObjNumberHex()
        {
            return _objNumberHex;
        }

        public string getPackage()
        {
            return _itemPackage;
        }





    }
}
