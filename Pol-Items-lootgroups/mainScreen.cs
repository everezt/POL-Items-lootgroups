using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pol_Items_lootgroups.Classes;

namespace Pol_Items_lootgroups
{
    public partial class mainScreen : Form
    {
        public mainScreen()
        {
            InitializeComponent();
        }

        // contain all items in here
        private List<Item> fullItems = new List<Item>();
        // contain filtered items here
        private List<Item> filteredItems = new List<Item>();
        // contain itemgroups here
        private List<ItemGroup> itemGroups = new List<ItemGroup>();
        // contain loot template
        private List<Loot> lootTemplates = new List<Loot>();
        // Do we use filtered list
        private bool isFiltered = false;
        // Have we loaded all items?
        private bool itemsLoaded = false;
        // Have we loaded lootgroups?
        private bool groupsLoaded = false;
        // Have we loaded loot table?
        private bool lootTableLoaded = false;

        private void loadObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileForm = new OpenFileDialog();
            openFileForm.Filter = "Object types file |*.txt";

            

            if (openFileForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fullItems.Clear();
                    filteredItems.Clear();
                    itemList.Items.Clear();
                    isFiltered = false;

                    string[] lines = System.IO.File.ReadAllLines(openFileForm.FileName);
                    int lineCount = lines.Count() - 1;
                    int currDone = 0;
                    progressBar1.Maximum = lineCount + 1;
                    progressBar1.Visible = true;

                    for (int i = 0; i <= lineCount; i++)
                    {
                        currDone++;

                        if (Parser.isLineUsed(lines[i]))
                        {
                            Item tempItem = new Item();

                            if (tempItem.fromArray(Parser.parseLineElements(lines[i])))
                            {
                                itemList.Items.Add(tempItem.getName());
                                fullItems.Add(tempItem);
                            }
                        }
                         
                        progressBar1.Value = currDone;
                    }

                    itemList.SelectedIndex = 0;

                    progressBar1.Visible = false;

                    itemsLoaded = true;
                    if (groupsLoaded)
                    {
                        addItem.Enabled = true;
                        convertBtn.Enabled = true;
                    }

                    if (lootTableLoaded)
                    {
                        addTemplateItem.Enabled = true;
                        addOnCutItem.Enabled = true;
                        addMagicItem.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    addTemplateItem.Enabled = false;
                    addOnCutItem.Enabled = false;
                    addMagicItem.Enabled = false;

                    progressBar1.Visible = false;
                    MessageBox.Show("Error: Could not read this file! Stack error: " + ex.Message);
                }
            }
        }

        private void mainScreen_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* reducted
            int id = listBox1.SelectedIndex;
            if (!isFiltered)
            {
                hexBox.Text = fullItems[id].getObjNumberHex();
                nameBox.Text = fullItems[id].getName();
                catBox.Text = fullItems[id].getCategory();
            }
            else
            {
                hexBox.Text = filteredItems[id].getObjNumberHex();
                nameBox.Text = filteredItems[id].getName();
                catBox.Text = filteredItems[id].getCategory();
            }
            */
                      
        }

        // filter items in itemlist
        private void filterButton_Click(object sender, EventArgs e)
        {
            itemList.Items.Clear();
            filteredItems.Clear();

            int lineCount = fullItems.Count() - 1;
            int currDone = 0;

            progressBar1.Maximum = lineCount + 1;
            progressBar1.Visible = true;

            if (!string.IsNullOrWhiteSpace(filterValue.Text))
            {
                foreach (var item in fullItems)
                {

                    currDone++;

                    if (filterType.SelectedIndex == 0)
                    {
                        // By name
                        if (item.getName().ToLower().Contains(filterValue.Text.ToLower()))
                        {
                            filteredItems.Add(item);
                            itemList.Items.Add(item.getName());
                        }
                    }
                    else
                    {
                        // By category
                        if (item.getPackage() != null)
                        {
                            if (item.getPackage().ToLower().Contains(filterValue.Text.ToLower()))
                            {
                                filteredItems.Add(item);
                                itemList.Items.Add(item.getName());
                            }
                        }                        
                    }

                    progressBar1.Value = currDone;
                }

                isFiltered = true;               
            }
            else
            {
                foreach (var item in fullItems)
                {
                    currDone++;
                    itemList.Items.Add(item.getName());
                    progressBar1.Value = currDone;
                }

                isFiltered = false;
            }

            if (itemList.Items.Count >= 1)
            {
                itemList.SelectedIndex = 0;
            }

            progressBar1.Visible = false;

        }

        private void loadItemgroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileForm = new OpenFileDialog();
            openFileForm.Filter = "Itemgroups file |*.cfg";

            if (openFileForm.ShowDialog() == DialogResult.OK)
            {
                try
                {  

                    itemGroups.Clear();
                    lootGroupList.Items.Clear();
                    lootGroupItems.Items.Clear();

                    string text = System.IO.File.ReadAllText(openFileForm.FileName);

                    string[] groupData = Parser.parseGroups(text);

                    int count = groupData.Count() - 1;
                    int currDone = 0;
                    progressBar1.Maximum = count + 1;
                    progressBar1.Visible = true;

                    ItemGroup tempGroup = new ItemGroup();

                    for (int i = 0; i <= count; i++)
                    {
                        currDone++;


                        string[] lines = Parser.parseToLines(groupData[i]);

                        for (int j = 0; j <= lines.Count() - 1; j++)
                        {
                            string[] elements = Parser.parseLineElements(lines[j]);

                            if (Parser.isLineUsed(lines[j]))
                            {
                                for (int k = 0; k <= elements.Count() -1; k++)
                                {
                                    switch (elements[k].ToLower().Trim())
                                    {
                                        case "group":

                                            if (tempGroup.Name == null)
                                            {
                                                tempGroup.Name = elements[k + 1].Trim();
                                            }
                                            else
                                            {
                                                itemGroups.Add(tempGroup);
                                                tempGroup = new ItemGroup();
                                                tempGroup.Name = elements[k + 1].Trim();

                                            }
                                            lootGroupList.Items.Add(elements[k + 1].Trim());
                                            break;
                                        case "item":
                                            tempGroup.Items.Add(elements[k + 1].Trim());
                                            break;

                                    }
                                }
                            }
                        }
                           
                        progressBar1.Value = currDone;
                    }


                    // add last itemgroup info to the list aswell
                    itemGroups.Add(tempGroup);

                    lootGroupList.SelectedIndex = 0;
                    lootGroupItems.SelectedIndex = 0;

                    progressBar1.Visible = false;

                    addLootGroup.Enabled = true;
                    delLootGroup.Enabled = true;
                    exportLootGroups.Enabled = true;

                    groupsLoaded = true;
                    if (itemsLoaded)
                    {
                        addItem.Enabled = true;
                        convertBtn.Enabled = true;
                    }
                    delItem.Enabled = true;

                    if (lootTableLoaded)
                    {
                        addRandMagicGroup.Enabled = true;
                        addRandGroups.Enabled = true;
                    }
                }
               
                catch (Exception ex)
                {
                    progressBar1.Visible = false;
                    MessageBox.Show("Error: Could not read this file! Stack error: " + ex.Message);
                }
            }
            else
            {
                addLootGroup.Enabled = false;
                delLootGroup.Enabled = false;
                exportLootGroups.Enabled = false;

                groupsLoaded = false;      
                addItem.Enabled = false;
                delItem.Enabled = false;

                addRandMagicGroup.Enabled = false;
                addRandGroups.Enabled = false;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lootGroupItems.Items.Clear();

            if (lootGroupList.SelectedIndex < 0)
            {
                lootGroupList.SelectedIndex = -1;
            }

            if (lootGroupList.SelectedIndex >= 0)
            {
                foreach (var item in itemGroups[lootGroupList.SelectedIndex].Items)
                {
                    lootGroupItems.Items.Add(item);
                }
            }
        }

        private void exportLootGroups_Click(object sender, EventArgs e)
        {
            string fileData = Parser.exportItemGroups(itemGroups);

            System.IO.Stream myStream;
            SaveFileDialog saveFileDiag = new SaveFileDialog();
            saveFileDiag.Filter = "Itemgroups file|*.cfg";

            if (saveFileDiag.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDiag.OpenFile()) != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(fileData);
                    myStream.Write(data, 0, data.Length);
                    myStream.Close();
                }
            }
        }

        private void delLootGroup_Click(object sender, EventArgs e)
        {
            if (lootGroupList.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure about deleting " + lootGroupList.Items[lootGroupList.SelectedIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int itemIndex = lootGroupList.SelectedIndex;

                    itemGroups.RemoveAt(lootGroupList.SelectedIndex);
                    lootGroupList.Items.RemoveAt(lootGroupList.SelectedIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootGroupList);

                }
                   
            }
        }

        private void addLootGroup_Click(object sender, EventArgs e)
        {
            string newName = lootGroupName.Text;
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                {
                    throw new Exception("Insert new lootgroup name");
                }

                foreach (var group in itemGroups)
                {
                    if (newName.ToLower() == group.Name.ToLower())
                    {
                        throw new Exception("This lootgroup name already exists, choose another");
                    }
                }

                ItemGroup tempGroup = new ItemGroup();
                tempGroup.Name = newName;
                itemGroups.Add(tempGroup);
                lootGroupList.Items.Add(newName);
                lootGroupList.SelectedIndex = lootGroupList.Items.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void delItem_Click(object sender, EventArgs e)
        {
            if (lootGroupItems.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure about deleting " + lootGroupItems.Items[lootGroupItems.SelectedIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int itemIndex = lootGroupItems.SelectedIndex;

                    itemGroups[lootGroupList.SelectedIndex].Items.RemoveAt(lootGroupItems.SelectedIndex);
                    lootGroupItems.Items.RemoveAt(lootGroupItems.SelectedIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootGroupItems);
                }
            }
        }

        private void addItem_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedIndex >= 0)
            {
                lootGroupItems.Items.Add(itemList.Items[itemList.SelectedIndex]);
                itemGroups[lootGroupList.SelectedIndex].Items.Add(itemList.Items[itemList.SelectedIndex].ToString());
                lootGroupItems.SelectedIndex = lootGroupItems.Items.Count - 1;
            }
        }

        private void showParsingError(string error, string templateName, string attribute, string parameter, string description = "" )
        {
            MessageBox.Show(string.Format("{0}: {1}->{2}->{3}\r\n{4}", error, templateName, attribute, parameter, description));
        }

        private void loadLootTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileForm = new OpenFileDialog();
            openFileForm.Filter = "Npc lootgroup file (npc_lootgroups.cfg) |*.cfg";

           

            if (openFileForm.ShowDialog() == DialogResult.OK)
            {

                lootTemplates.Clear();
                templateList.Items.Clear();
                lootItemsList.Items.Clear();
                lootMagicItemList.Items.Clear();
                lootRandFromGroupList.Items.Clear();
                lootRandMagicFromGroupList.Items.Clear();
                lootSkillBookList.Items.Clear();

                try
                {

                    string text = System.IO.File.ReadAllText(openFileForm.FileName);

                    string[] groupData = Parser.parseGroups(text);

                    int count = groupData.Count() - 1;
                    int currDone = 0;
                    progressBar1.Maximum = count + 1;
                    progressBar1.Visible = true;

                    Loot tempLootTemplate = new Loot();

                    for (int i = 0; i <= count; i++)
                    {
                        currDone++;


                        string[] lines = Parser.parseToLines(groupData[i]);

                        for (int j = 0; j <= lines.Count() - 1; j++)
                        {
                            string[] elements;

                            // fix for "wrong" locks
                            if (lines[j].Trim() == "{")
                            {
                                elements = Parser.parseLineElements(lines[j - 1]);
                                if (elements[0] != "lootgroup")
                                {
                                    elements[0] = "lootgroup";
                                }
                                else
                                {
                                    elements = Parser.parseLineElements(lines[j]);
                                }
                            }
                            else
                            {
                                elements = Parser.parseLineElements(lines[j]);
                            }

                           

                            if (Parser.isLineUsed(lines[j]))
                            {
                                for (int k = 0; k <= elements.Count() - 1; k++)
                                {
                                    switch (elements[k].ToLower().Trim())
                                    {
                                        case "lootgroup":

                                            if (tempLootTemplate.Name == null)
                                            {
                                                tempLootTemplate.Name = elements[k + 1].Trim();
                                            }
                                            else
                                            {
                                                lootTemplates.Add(tempLootTemplate);
                                                tempLootTemplate = new Loot();
                                                tempLootTemplate.Name = elements[k + 1].Trim();

                                            }
                                            templateList.Items.Add(elements[k + 1].Trim());
                                            break;
                                        case "gold":
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempLootTemplate.Gold = elements[k + 1].Trim();
                                            }
                                            else
                                            {
                                                //MessageBox.Show("Found gold attribute, but found no amount parameter, setting it to null! Template: " + tempLootTemplate.Name);
                                                showParsingError("Missing", tempLootTemplate.Name, "gold", "amount", "Setting it to 0!");
                                                tempLootTemplate.Gold = "0";
                                            }                                           
                                            break;
                                        case "magic_quality":
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempLootTemplate.MagicQuality = int.Parse(elements[k + 1].Trim());
                                            }
                                            else
                                            {
                                                tempLootTemplate.MagicQuality = 0;
                                                showParsingError("Missing", tempLootTemplate.Name, "magic_quality", "amount", "Setting it to 0!");
                                                //MessageBox.Show("Found magic_quality attribute, but found no amount parameter, setting it to 0! Template:" + tempLootTemplate.Name);
                                            }                                          
                                            break;
                                        case "chest_chance":
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempLootTemplate.ChestChance = int.Parse(elements[k + 1].Trim());
                                            }
                                            else
                                            {
                                                tempLootTemplate.ChestChance = 0;
                                                showParsingError("Missing", tempLootTemplate.Name, "chest_chance", "amount", "Setting it to 0!");
                                                //MessageBox.Show("Found chest_chance attribute, but found no chance parameter, setting it to 0! Template: " + tempLootTemplate.Name);
                                            }
                                            break;
                                        case "chest_quality":
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempLootTemplate.ChestQuality = int.Parse(elements[k + 1].Trim());
                                            }
                                            else
                                            {
                                                showParsingError("Missing", tempLootTemplate.Name, "chest_quality", "amount", "Setting it to 0!");
                                                tempLootTemplate.ChestQuality = 0;
                                                //MessageBox.Show("Found chest_quality attribute, but found no amount paramenter, setting it to 0! Template: " + tempLootTemplate.Name);
                                            }
                                            
                                            break;
                                        case "item":
                                            StandardItem tempItem = new StandardItem();
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempItem.Name = elements[k + 1].Trim();

                                                if (Parser.checkParams(elements, 2))
                                                {
                                                    tempItem.UpToHowMuchDropped = elements[k + 2].Trim();
                                                }
                                                else
                                                {
                                                    showParsingError("Missing", tempLootTemplate.Name, "item", "tries", "Setting it to 1!");
                                                    //MessageBox.Show("Found item attribute, but found no 'tries' parameter, setting it to 0! Template:" + tempLootTemplate.Name);
                                                    tempItem.UpToHowMuchDropped = "1";
                                                }

                                                if (Parser.checkParams(elements, 3))
                                                {
                                                    tempItem.ChanceOfDropPerItem = int.Parse(elements[k + 3].Trim());
                                                }
                                                else
                                                {
                                                    showParsingError("Missing", tempLootTemplate.Name, "item", "chance", "Setting it to 1!");
                                                    //MessageBox.Show("Found Item attribute, but found no chance parameter, setting it to 0! Template: " + tempLootTemplate.Name);
                                                    tempItem.ChanceOfDropPerItem = 1;
                                                }
                                          
                                                tempLootTemplate.items.Add(tempItem);
                                            }
                                            else
                                            {
                                                showParsingError("Error", tempLootTemplate.Name, "item", "objName", "Ignoring a line!");
                                                //.Show("Found item attribute, but found no item template, ignoring line! Template: " + tempLootTemplate.Name);
                                            }              
                                            break;
                                        case "random":
                                            RandomItemFromGroup randTempItem = new RandomItemFromGroup();
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                randTempItem.Name = elements[k + 1].Trim();

                                                if (Parser.checkParams(elements, 2))
                                                {
                                                    randTempItem.UpToHowMuchDropped = elements[k + 2].Trim();
                                                }
                                                else
                                                {
                                                    showParsingError("Missing", tempLootTemplate.Name, "random", "tries", "Setting it to 1!");
                                                    randTempItem.UpToHowMuchDropped = "1";
                                                    //MessageBox.Show("Missing => " + tempLootTemplate.Name + " ->Random->tries parameter! Setting it to 0!");
                                                }

                                                if (Parser.checkParams(elements, 3))
                                                {
                                                    randTempItem.ChanceOfDropPerItem = int.Parse(elements[k + 3].Trim());
                                                }
                                                else
                                                {
                                                    showParsingError("Missing", tempLootTemplate.Name, "random", "chance", "Setting it to 1!");
                                                    randTempItem.ChanceOfDropPerItem = 1;
                                                }

                                                tempLootTemplate.randItemFromGroup.Add(randTempItem);
                                            }
                                            else
                                            {
                                                showParsingError("Error", tempLootTemplate.Name, "random", "itemGroup", "Ignoring the line!");
                                                //MessageBox.Show("Found random attribute, but found no item group attribute. Ignoring the line! Template: " + tempLootTemplate.Name);
                                            }
                                            break;
                                        case "magic_item":                                            
                                            MagicItem magicTempItem = new MagicItem();
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                magicTempItem.Name = elements[k + 1].Trim();

                                                if (Parser.checkParams(elements, 2))
                                                {
                                                    magicTempItem.ChanceOfDrop = int.Parse(elements[k + 2].Trim());
                                                }
                                                else
                                                {
                                                    showParsingError("Missing", tempLootTemplate.Name, "magic_item", "chance", "Setting it to 1!");
                                                    magicTempItem.ChanceOfDrop = 1;
                                                }

                                                tempLootTemplate.magicItems.Add(magicTempItem);
                                            }
                                            else
                                            {
                                                showParsingError("Error", tempLootTemplate.Name, "magic_item", "objName", "Ignoring the line!");
                                            }
                                            
                                            
                                            break;
                                        case "magic_group":
                                            RandomMagicItemFromGroup randMagicTempItem = new RandomMagicItemFromGroup();
                                            if (Parser.checkParams(elements, 1))
                                            {
                                               
                                                randMagicTempItem.Name = elements[k + 1].Trim();


                                                if (Parser.checkParams(elements, 2))
                                                {
                                                    randMagicTempItem.ChanceOfDrop = int.Parse(elements[k + 2].Trim());

                                                }
                                                else
                                                {
                                                    randMagicTempItem.ChanceOfDrop = 1;
                                                    showParsingError("Missing", tempLootTemplate.Name, "magic_group", "chance", "Setting it to 1!");
                                                }

                                                tempLootTemplate.randMagicItemFromGroup.Add(randMagicTempItem);
                                            }
                                            else
                                            {
                                                showParsingError("Error", tempLootTemplate.Name, "magic_group", "itemGroup", "Ignoring the line!");
                                            }
                                            break;
                                        case "skillbook":
                                            Skillbook tempSkillBook = new Skillbook();
                                            if(Parser.checkParams(elements, 1))
                                            {
                                                tempSkillBook.skillTier = int.Parse(elements[k + 1].Trim());
                                            }
                                            else
                                            {
                                                tempSkillBook.skillTier = 1;
                                                showParsingError("Missing", tempLootTemplate.Name, "skillbook", "tier", "Setting it to 1!");
                                            }

                                            if (Parser.checkParams(elements, 2))
                                            {
                                                tempSkillBook.ChanceOfDrop = int.Parse(elements[k + 2].Trim());
                                            }
                                            else
                                            {
                                                tempSkillBook.ChanceOfDrop = 1;
                                                showParsingError("Missing", tempLootTemplate.Name, "skillbook", "chance", "Setting it to 1!");
                                            }

                                           
                                            tempLootTemplate.skillBooks.Add(tempSkillBook);                                          
                                            break;
                                        case "head":
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempLootTemplate.Head = int.Parse(elements[k + 1].Trim());
                                            }
                                            else
                                            {
                                                tempLootTemplate.Head = 0;
                                                showParsingError("Missing", tempLootTemplate.Name, "head", "true/false", "Setting it to false!");
                                            }                                           
                                            break;
                                        case "blood":
                                            if (Parser.checkParams(elements, 1))
                                            {
                                                tempLootTemplate.Blood = int.Parse(elements[k + 1].Trim());
                                            }
                                            else
                                            {
                                                tempLootTemplate.Blood = 0;
                                                showParsingError("Missing", tempLootTemplate.Name, "blood", "true/false", "Setting it to false!");
                                            }
                                            
                                            break;
                                        case "cutup":
                                            CutUp tempCutUpItem = new CutUp();
                                            if(Parser.checkParams(elements, 1))
                                            {
                                                tempCutUpItem.ItemName = elements[k + 1].Trim();
                                                
                                                if(Parser.checkParams(elements, 2))
                                                {
                                                    tempCutUpItem.Amount = elements[k + 2].Trim();
                                                }
                                                else
                                                {
                                                    tempCutUpItem.Amount = "1";
                                                    showParsingError("Missing", tempLootTemplate.Name, "cutup", "amount", "Setting it 1!");
                                                }

                                                tempLootTemplate.cutUps.Add(tempCutUpItem);
                                            }
                                            else
                                            {
                                                showParsingError("Error", tempLootTemplate.Name, "cutup", "objName", "Ignoring the line!");
                                            }
                                            
                                            
                                            
                                            break;

                                    }
                                }
                            }
                        }

                        progressBar1.Value = currDone;
                    }


                    // add last itemgroup info to the list aswell

                    if (tempLootTemplate.Name != null)
                    {
                        lootTemplates.Add(tempLootTemplate);
                    }

                    


                    

                    if (lootTemplates.Count() >= 1)
                    {
                        lootTableLoaded = true;

                        templateInfoGroup.Enabled = true;
                        addSkillBookGroup.Enabled = true;
                        exportTemplate.Enabled = true;
                        updateTemplate.Enabled = true;

                        templateList.SelectedIndex = 0;


                        progressBar1.Visible = false;

                        if (groupsLoaded)
                        {
                            addRandGroups.Enabled = true;
                            addRandMagicGroup.Enabled = true;
                        }

                        if (itemsLoaded)
                        {
                            addTemplateItem.Enabled = true;
                            addOnCutItem.Enabled = true;
                            addMagicItem.Enabled = true;
                        }
                    }
                    else
                    {
                        throw new Exception("Did not find any templates in this file...");
                    }

                    

                    
                }
                
                catch (Exception ex)
                {
                    lootTableLoaded = false;
                    lootTemplates.Clear();
                    templateList.Items.Clear();
                    lootItemsList.Items.Clear();
                    lootMagicItemList.Items.Clear();
                    lootRandFromGroupList.Items.Clear();
                    lootRandMagicFromGroupList.Items.Clear();
                    lootSkillBookList.Items.Clear();

                    templateInfoGroup.Enabled = false;
                    addSkillBookGroup.Enabled = false;
                    exportTemplate.Enabled = false;
                    updateTemplate.Enabled = false;

                    progressBar1.Visible = false;

                    addRandGroups.Enabled = false;
                    addRandMagicGroup.Enabled = false;

                    addTemplateItem.Enabled = false;
                    addOnCutItem.Enabled = false;
                    addMagicItem.Enabled = false;

                    MessageBox.Show("Error: Could not read this file! Stack error: " + ex.Message);
                }
                
            }
           
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currId = templateList.SelectedIndex;

            lootItemsList.Items.Clear();
            lootMagicItemList.Items.Clear();
            lootRandFromGroupList.Items.Clear();
            lootRandMagicFromGroupList.Items.Clear();
            lootSkillBookList.Items.Clear();
            lootOnCorpseCutList.Items.Clear();

            if (lootTemplates[currId].Gold == null)
            {
                goldBox.Text = "";
            }
            else
            {
                goldBox.Text = lootTemplates[currId].Gold;
            }
            goldBox.BackColor = SystemColors.Control;

            magicQualityBox.Text = lootTemplates[currId].MagicQuality.ToString();
            magicQualityBox.BackColor = Color.White;
            chestChanceBox.Text = lootTemplates[currId].ChestChance.ToString();
            chestChanceBox.BackColor = Color.White;
            chestQualityBox.Text = lootTemplates[currId].ChestQuality.ToString();
            chestQualityBox.BackColor = Color.White;


            if (lootTemplates[currId].Head == 1)
            {
                headBox.Checked = true;
                //headBox.BackColor = SystemColors.Control;
            } else
            {
                headBox.Checked = false;
                //headBox.BackColor = SystemColors.Control;
            }


            if (lootTemplates[currId].Blood == 1)
            {
                bloodBox.Checked = true;
                //bloodBox.BackColor = SystemColors.Control;
            }
            else
            {
                bloodBox.Checked = false;
                //bloodBox.BackColor = SystemColors.Control;
            }

            
            
            
            
           
            

            foreach (var item in lootTemplates[currId].items)
            {
                lootItemsList.Items.Add(item.Name);
            }
               
            
            if (lootItemsList.Items.Count <= 0)
            {
                itemInfoGroup.Enabled = false;
                delItemTemplate.Enabled = false;
                updateItemTemplate.Enabled = false;
            }
            else
            {
                lootItemsList.SelectedIndex = 0;
                itemInfoGroup.Enabled = true;
                delItemTemplate.Enabled = true;
                updateItemTemplate.Enabled = true;
            }        

            foreach (var item in lootTemplates[currId].magicItems)
            {
                lootMagicItemList.Items.Add(item.Name);
            }
            

            if (lootMagicItemList.Items.Count <= 0)
            {
                magicItemInfoGroup.Enabled = false;
                delMagicItem.Enabled = false;
                updateMagicItem.Enabled = false;
            }
            else
            {
                lootMagicItemList.SelectedIndex = 0;
                magicItemInfoGroup.Enabled = true;
                delMagicItem.Enabled = true;
                updateMagicItem.Enabled = true;
            }

            foreach (var item in lootTemplates[currId].randItemFromGroup)
            {
                lootRandFromGroupList.Items.Add(item.Name);
            }

            if (lootRandFromGroupList.Items.Count <= 0)
            {
                randItemGroup.Enabled = false;
                delRandItemGroup.Enabled = false;
                updateRandItemGroup.Enabled = false;
            }
            else
            {
                lootRandFromGroupList.SelectedIndex = 0;
                randItemGroup.Enabled = true;
                delRandItemGroup.Enabled = true;
                updateRandItemGroup.Enabled = true;
            }

            foreach (var item in lootTemplates[currId].randMagicItemFromGroup)
            {
                lootRandMagicFromGroupList.Items.Add(item.Name);
            }

            if (lootRandMagicFromGroupList.Items.Count <= 0)
            {
                randMagicInfoGroup.Enabled = false;
                delRandMagicGroup.Enabled = false;
                updateRandMagicGroup.Enabled = false;
            }
            else
            {
                lootRandMagicFromGroupList.SelectedIndex = 0;
                randMagicInfoGroup.Enabled = true;
                delRandMagicGroup.Enabled = true;
                updateRandMagicGroup.Enabled = true;
            }

            foreach (var item in lootTemplates[currId].skillBooks)
            {
                lootSkillBookList.Items.Add("skillbook");
            }

            if (lootSkillBookList.Items.Count <= 0)
            {
                skillBookInfoGroup.Enabled = false;
                delSkillBook.Enabled = false;
                updateSkillBook.Enabled = false;
            }
            else
            {
                lootSkillBookList.SelectedIndex = 0;
                delSkillBook.Enabled = true;
                updateSkillBook.Enabled = true;
                skillBookInfoGroup.Enabled = true;
            }

            foreach (var item in lootTemplates[currId].cutUps)
            {
                lootOnCorpseCutList.Items.Add(item.ItemName);
            }

            if (lootOnCorpseCutList.Items.Count <= 0)
            {
                delCorpseCut.Enabled = false;
                updateCorpseCut.Enabled = false;
                cutUpInfoGroup.Enabled = false;
            }
            else
            {
                lootOnCorpseCutList.SelectedIndex = 0;
                cutUpInfoGroup.Enabled = true;
                delCorpseCut.Enabled = true;
                updateCorpseCut.Enabled = true;
            }

            itemTriesBox.BackColor = SystemColors.Control;
            itemChanceBox.BackColor = Color.White;

            magicChanceBox.BackColor = Color.White;

            randItemTriesBox.BackColor = SystemColors.Control;
            randItemChanceBox.BackColor = Color.White;

            randMagicItemChanceBox.BackColor = Color.White;

            bookTierBox.BackColor = Color.White;
            bookChanceBox.BackColor = Color.White;

            cutAmountBox.BackColor = SystemColors.Control;






        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lootItemsList.SelectedIndex >= 0)
            {
                int tempId = templateList.SelectedIndex;
                int itemId = lootItemsList.SelectedIndex;

                itemTriesBox.Text = lootTemplates[tempId].items[itemId].UpToHowMuchDropped;
                itemChanceBox.Text = lootTemplates[tempId].items[itemId].ChanceOfDropPerItem.ToString();              
            }

            itemTriesBox.BackColor = SystemColors.Control;
            itemChanceBox.BackColor = Color.White;
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lootMagicItemList.SelectedIndex >= 0)
            {
                int tempId = templateList.SelectedIndex;
                int itemId = lootMagicItemList.SelectedIndex;

                magicChanceBox.Text = lootTemplates[tempId].magicItems[itemId].ChanceOfDrop.ToString();

                magicChanceBox.BackColor = Color.White;
            }
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lootRandFromGroupList.SelectedIndex >= 0)
            {
                int tempId = templateList.SelectedIndex;
                int itemId = lootRandFromGroupList.SelectedIndex;

                randItemTriesBox.Text = lootTemplates[tempId].randItemFromGroup[itemId].UpToHowMuchDropped;
                randItemChanceBox.Text = lootTemplates[tempId].randItemFromGroup[itemId].ChanceOfDropPerItem.ToString();

                randItemTriesBox.BackColor = SystemColors.Control;
                randItemChanceBox.BackColor = Color.White;
            }            
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {           
                if (lootRandMagicFromGroupList.SelectedIndex >= 0)
                {
                    int tempId = templateList.SelectedIndex;
                    int itemId = lootRandMagicFromGroupList.SelectedIndex;

                    randMagicItemChanceBox.Text = lootTemplates[tempId].randMagicItemFromGroup[itemId].ChanceOfDrop.ToString();

                    randMagicItemChanceBox.BackColor = Color.White;
                }         
        }

        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lootSkillBookList.SelectedIndex >= 0)
            {
                int tempId = templateList.SelectedIndex;
                int itemId = lootSkillBookList.SelectedIndex;


                bookTierBox.Text = lootTemplates[tempId].skillBooks[itemId].skillTier.ToString();
                bookChanceBox.Text = lootTemplates[tempId].skillBooks[itemId].ChanceOfDrop.ToString();

                bookTierBox.BackColor = Color.White;
                bookChanceBox.BackColor = Color.White;

            }
        }

        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lootOnCorpseCutList.SelectedIndex >= 0)
            {
                int tempId = templateList.SelectedIndex;
                int itemId = lootOnCorpseCutList.SelectedIndex;

                cutAmountBox.Text = lootTemplates[tempId].cutUps[itemId].Amount.ToString();

                cutAmountBox.BackColor = SystemColors.Control;
            }
        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void dealWithTemplateItemButtons()
        {
            if (itemsLoaded)
            {
                addTemplateItem.Enabled = true;
                itemInfoGroup.Enabled = true;
            }

            if (lootItemsList.Items.Count <= 0)
            {
                itemInfoGroup.Enabled = false;
                delItemTemplate.Enabled = false;
                updateItemTemplate.Enabled = false;
            }
            else
            {
                itemInfoGroup.Enabled = true;
                delItemTemplate.Enabled = true;
                updateItemTemplate.Enabled = true;
            }
        }

        private void addTemplateItem_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedIndex >= 0)
            {
                StandardItem tempItem = new StandardItem();
                tempItem.Name = itemList.Items[itemList.SelectedIndex].ToString();
                tempItem.ChanceOfDropPerItem = 10;
                tempItem.UpToHowMuchDropped = "1d1";
                lootTemplates[templateList.SelectedIndex].items.Add(tempItem);
                lootItemsList.Items.Add(tempItem.Name);
                lootItemsList.SelectedIndex = lootItemsList.Items.Count-1;

                dealWithTemplateItemButtons();
            }
        }

        private void delItemTemplate_Click(object sender, EventArgs e)
        {
            if (lootItemsList.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure about deleting " + lootItemsList.Items[lootItemsList.SelectedIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int itemIndex = lootItemsList.SelectedIndex;
                    lootTemplates[templateList.SelectedIndex].items.RemoveAt(itemIndex);
                    lootItemsList.Items.RemoveAt(itemIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootItemsList);

                    dealWithTemplateItemButtons();
                }

            }
        }

        private void updateItemTemplate_Click(object sender, EventArgs e)
        {
            if (lootItemsList.SelectedIndex >= 0)
            {
                try
                {
                    int itemIndex = lootItemsList.SelectedIndex;
                    int templateIndex = templateList.SelectedIndex;

                    
                    if (!Dice.isValidDiceString(itemTriesBox.Text))
                    {
                        throw new Exception("Can't update! Tries is not valid die string!");
                    }

                    int chance;
                    if (!int.TryParse(itemChanceBox.Text, out chance))
                    {
                        throw new Exception("Can't update! Chance is not a number!");
                    }

                    if ( chance <= 0 )
                    {
                        throw new Exception("Can't update! Chance can't be less than 0!");
                    }

                    lootTemplates[templateIndex].items[itemIndex].ChanceOfDropPerItem = chance;
                    lootTemplates[templateIndex].items[itemIndex].UpToHowMuchDropped = itemTriesBox.Text;

                    itemTriesBox.BackColor = SystemColors.Control;
                    itemChanceBox.BackColor = Color.White;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void itemTriesBox_Click(object sender, EventArgs e)
        {
            DiceGenerator diceGen = new DiceGenerator(ref itemTriesBox, itemInfoGroup.PointToScreen(itemTriesBox.Location));
            diceGen.ShowDialog();
        }

        private void randItemTriesBox_Click(object sender, EventArgs e)
        {            
            DiceGenerator diceGen = new DiceGenerator(ref randItemTriesBox, randItemGroup.PointToScreen(randItemTriesBox.Location));
            diceGen.ShowDialog();
        }

        private void goldBox_Click(object sender, EventArgs e)
        {
            DiceGenerator diceGen = new DiceGenerator(ref goldBox, templateInfoGroup.PointToScreen(goldBox.Location));
            diceGen.ShowDialog();
        }

        private void cutAmountBox_Click(object sender, EventArgs e)
        {
            DiceGenerator diceGen = new DiceGenerator(ref cutAmountBox, cutUpInfoGroup.PointToScreen(cutAmountBox.Location));
            diceGen.ShowDialog();
        }

        private void itemTriesBox_TextChanged(object sender, EventArgs e)
        {
            itemTriesBox.BackColor = Color.Orange;
        }

        private void itemChanceBox_TextChanged(object sender, EventArgs e)
        {
            itemChanceBox.BackColor = Color.Orange;
        }

        private void updateRandItemGroup_Click(object sender, EventArgs e)
        {
            if (lootRandFromGroupList.SelectedIndex >= 0)
            {
                try
                {
                    int itemIndex = lootRandFromGroupList.SelectedIndex;
                    int templateIndex = templateList.SelectedIndex;


                    if (!Dice.isValidDiceString(randItemTriesBox.Text))
                    {
                        throw new Exception("Can't update! Tries is not valid die string!");
                    }

                    int chance;
                    if (!int.TryParse(randItemChanceBox.Text, out chance))
                    {
                        throw new Exception("Can't update! Chance is not a number!");
                    }

                    if (chance <= 0)
                    {
                        throw new Exception("Can't update! Chance can't be 0 or less!");
                    }

                    lootTemplates[templateIndex].randItemFromGroup[itemIndex].ChanceOfDropPerItem = chance;
                    lootTemplates[templateIndex].randItemFromGroup[itemIndex].UpToHowMuchDropped = randItemTriesBox.Text;

                    randItemTriesBox.BackColor = SystemColors.Control;
                    randItemChanceBox.BackColor = Color.White;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void randItemTriesBox_TextChanged(object sender, EventArgs e)
        {
            randItemTriesBox.BackColor = Color.Orange;
        }

        private void randItemChanceBox_TextChanged(object sender, EventArgs e)
        {
            randItemChanceBox.BackColor = Color.Orange;
        }

        private void updateCorpseCut_Click(object sender, EventArgs e)
        {
            if (lootOnCorpseCutList.SelectedIndex >= 0)
            {
                try
                {
                    int itemIndex = lootOnCorpseCutList.SelectedIndex;
                    int templateIndex = templateList.SelectedIndex;


                    if (!Dice.isValidDiceString(cutAmountBox.Text))
                    {
                        throw new Exception("Can't update! Tries is not valid die string!");
                    }

                    
                    lootTemplates[templateIndex].cutUps[itemIndex].Amount = cutAmountBox.Text;

                    cutAmountBox.BackColor = SystemColors.Control;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void cutAmountBox_TextChanged(object sender, EventArgs e)
        {
            cutAmountBox.BackColor = Color.Orange;
        }

        private void updateRandMagicGroup_Click(object sender, EventArgs e)
        {
            if (lootRandMagicFromGroupList.SelectedIndex >= 0)
            {
                try
                {
                    int itemIndex = lootRandMagicFromGroupList.SelectedIndex;
                    int templateIndex = templateList.SelectedIndex;


                    int chance;

                    if(!int.TryParse(randMagicItemChanceBox.Text, out chance))
                    {
                        throw new Exception("Can't update! Chance is not a number!");
                    }

                    if (chance <= 0)
                    {
                        throw new Exception("Can't update! Chance can't be 0 or less!");
                    }


                    lootTemplates[templateIndex].randMagicItemFromGroup[itemIndex].ChanceOfDrop = chance;

                    randMagicItemChanceBox.BackColor = Color.White;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void randMagicItemChanceBox_TextChanged(object sender, EventArgs e)
        {
            randMagicItemChanceBox.BackColor = Color.Orange;
        }

        private void addSkillBook_Click(object sender, EventArgs e)
        {
            try
            {
                int templateIndex = templateList.SelectedIndex;

                int tier, chance;

                if (!int.TryParse(addBookTierBox.Text, out tier))
                {
                    throw new Exception("Can't add skillbook! Tier is not a number!");
                }

                if (!int.TryParse(addBookChanceBox.Text, out chance))
                {
                    throw new Exception("Can't add skillbook! Chance is not a number!");
                }

                if (chance <= 0)
                {
                    throw new Exception("Can't add skillbook! Chance can't be 0 or less");
                }

                if (tier <= 0 || tier >= 5)
                {
                    throw new Exception("Can't add skillbook! Tier can be 1-4! \r\n1 - apprentice\r\n2 - expert\r\n3 - master\r\n4 - legendary");
                }

                Skillbook tempSkill = new Skillbook();
                tempSkill.ChanceOfDrop = chance;
                tempSkill.skillTier = tier;
                lootTemplates[templateIndex].skillBooks.Add(tempSkill);
                lootSkillBookList.Items.Add("skillbook");

                lootSkillBookList.SelectedIndex = lootSkillBookList.Items.Count - 1;

                dealWithSkillBookButtons();               

            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void addRandGroups_Click(object sender, EventArgs e)
        {
            if (lootGroupList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;

                RandomItemFromGroup tempItem = new RandomItemFromGroup();
                tempItem.Name = lootGroupList.Items[lootGroupList.SelectedIndex].ToString();
                tempItem.ChanceOfDropPerItem = 10;
                tempItem.UpToHowMuchDropped = "1d2";
                lootTemplates[templateIndex].randItemFromGroup.Add(tempItem);
                lootRandFromGroupList.Items.Add(tempItem.Name);

                lootRandFromGroupList.SelectedIndex = lootRandFromGroupList.Items.Count - 1;

                dealWithRandItemButtons();
            }            
           
        }

        public void dealWithRandItemButtons()
        {
            if (groupsLoaded)
            {
                addRandGroups.Enabled = true;
                randItemGroup.Enabled = true;
            }

            if (lootRandFromGroupList.Items.Count <= 0)
            {
                randItemGroup.Enabled = false;
                delRandItemGroup.Enabled = false;
                updateRandItemGroup.Enabled = false;
            }
            else
            {
                randItemGroup.Enabled = true;
                delRandItemGroup.Enabled = true;
                updateRandItemGroup.Enabled = true;
            }
        }

        private void delRandItemGroup_Click(object sender, EventArgs e)
        {
            if (lootRandFromGroupList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;
                int itemIndex = lootRandFromGroupList.SelectedIndex;

                if (MessageBox.Show("Are you sure about deleting " + lootRandFromGroupList.Items[itemIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lootTemplates[templateIndex].randItemFromGroup.RemoveAt(itemIndex);
                    lootRandFromGroupList.Items.RemoveAt(itemIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootRandFromGroupList);

                    dealWithRandItemButtons();
                }

            }
        }

        private void dealWithCutUpButtons()
        {
            if (itemsLoaded)
            {
                addOnCutItem.Enabled = true;
                cutUpInfoGroup.Enabled = true;
            }

            if (lootOnCorpseCutList.Items.Count <= 0)
            {
                cutUpInfoGroup.Enabled = false;
                delCorpseCut.Enabled = false;
                updateCorpseCut.Enabled = false;
            }
            else
            {
                cutUpInfoGroup.Enabled = true;
                delCorpseCut.Enabled = true;
                updateCorpseCut.Enabled = true;
            }
        }

        private void addOnCutItem_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;

                CutUp tempItem = new CutUp();
                tempItem.ItemName = itemList.Items[itemList.SelectedIndex].ToString();
                tempItem.Amount = "1";
                lootTemplates[templateIndex].cutUps.Add(tempItem);
                lootOnCorpseCutList.Items.Add(tempItem.ItemName);
                lootOnCorpseCutList.SelectedIndex = lootOnCorpseCutList.Items.Count - 1;

                dealWithCutUpButtons();
            }
        }

        private void delCorpseCut_Click(object sender, EventArgs e)
        {
            if (lootOnCorpseCutList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;
                int itemIndex = lootOnCorpseCutList.SelectedIndex;

                if (MessageBox.Show("Are you sure about deleting " + lootOnCorpseCutList.Items[itemIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lootTemplates[templateIndex].cutUps.RemoveAt(itemIndex);
                    lootOnCorpseCutList.Items.RemoveAt(itemIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootOnCorpseCutList);

                    dealWithCutUpButtons();
                }

            }
        }

        private void dealWithRandMagicItemButtons()
        {
            if (groupsLoaded)
            {
                addRandMagicGroup.Enabled = true;
                randMagicInfoGroup.Enabled = true;
            }

            if (lootRandMagicFromGroupList.Items.Count <= 0)
            {
                randMagicInfoGroup.Enabled = false;
                delRandItemGroup.Enabled = false;
                updateRandMagicGroup.Enabled = false;
            }
            else
            {
                randMagicInfoGroup.Enabled = true;
                delRandItemGroup.Enabled = true;
                updateRandMagicGroup.Enabled = true;
            }
        }

        private void addRandMagicGroup_Click(object sender, EventArgs e)
        {
            if (lootGroupList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;

                RandomMagicItemFromGroup tempItem = new RandomMagicItemFromGroup();
                tempItem.Name = lootGroupList.Items[lootGroupList.SelectedIndex].ToString();
                tempItem.ChanceOfDrop = 10;
                lootTemplates[templateIndex].randMagicItemFromGroup.Add(tempItem);
                lootRandMagicFromGroupList.Items.Add(tempItem.Name);

                lootRandMagicFromGroupList.SelectedIndex = lootRandMagicFromGroupList.Items.Count - 1;

                dealWithRandMagicItemButtons();
            }
        }

        private void delRandMagicGroup_Click(object sender, EventArgs e)
        {
            if (lootRandMagicFromGroupList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;
                int itemIndex = lootRandMagicFromGroupList.SelectedIndex;

                if (MessageBox.Show("Are you sure about deleting " + lootRandMagicFromGroupList.Items[itemIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lootTemplates[templateIndex].randMagicItemFromGroup.RemoveAt(itemIndex);
                    lootRandMagicFromGroupList.Items.RemoveAt(itemIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootRandMagicFromGroupList);

                    dealWithRandMagicItemButtons();
                }

            }
        }

        private void magicChanceBox_TextChanged(object sender, EventArgs e)
        {
            magicChanceBox.BackColor = Color.Orange;
        }

        private void dealWithMagicItemButtons()
        {
            if (itemsLoaded)
            {
                addMagicItem.Enabled = true;
                magicItemInfoGroup.Enabled = true;
            }

            if (lootMagicItemList.Items.Count <= 0)
            {
                magicItemInfoGroup.Enabled = false;
                delMagicItem.Enabled = false;
                updateMagicItem.Enabled = false;
            }
            else
            {
                magicItemInfoGroup.Enabled = true;
                delMagicItem.Enabled = true;
                updateMagicItem.Enabled = true;
            }
        }

        private void addMagicItem_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;

                MagicItem tempItem = new MagicItem();
                tempItem.Name = itemList.Items[itemList.SelectedIndex].ToString();
                tempItem.ChanceOfDrop = 10;
                lootTemplates[templateIndex].magicItems.Add(tempItem);
                lootMagicItemList.Items.Add(tempItem.Name);
                lootMagicItemList.SelectedIndex = lootMagicItemList.Items.Count - 1;

                dealWithMagicItemButtons();
            }
        }

        private void updateMagicItem_Click(object sender, EventArgs e)
        {
            if (lootMagicItemList.SelectedIndex >= 0)
            {
                try
                {
                    int itemIndex = lootMagicItemList.SelectedIndex;
                    int templateIndex = templateList.SelectedIndex;


                    int chance;

                    if (!int.TryParse(magicChanceBox.Text, out chance))
                    {
                        throw new Exception("Can't update! Chance is not a number!");
                    }

                    if (chance <= 0)
                    {
                        throw new Exception("Can't update! Chance can't be 0 or less!");
                    }


                    lootTemplates[templateIndex].magicItems[itemIndex].ChanceOfDrop = chance;

                    magicChanceBox.BackColor = Color.White;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void delMagicItem_Click(object sender, EventArgs e)
        {
            if (lootMagicItemList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;
                int itemIndex = lootMagicItemList.SelectedIndex;

                if (MessageBox.Show("Are you sure about deleting " + lootMagicItemList.Items[itemIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lootTemplates[templateIndex].magicItems.RemoveAt(itemIndex);
                    lootMagicItemList.Items.RemoveAt(itemIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootMagicItemList);

                    dealWithMagicItemButtons();
                }
            }
        }

        private void dealWithSkillBookButtons()
        {
            if (lootTableLoaded)
            {
                addSkillBook.Enabled = true;
                skillBookInfoGroup.Enabled = true;
            }

            if (lootSkillBookList.Items.Count <= 0)
            {
                skillBookInfoGroup.Enabled = false;
                updateSkillBook.Enabled = false;
                delSkillBook.Enabled = false;
            }
            else
            {
                skillBookInfoGroup.Enabled = true;
                updateSkillBook.Enabled = true;
                delSkillBook.Enabled = true;
            }
            
        }

        private void delSkillBook_Click(object sender, EventArgs e)
        {
            if (lootSkillBookList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;
                int itemIndex = lootSkillBookList.SelectedIndex;

                if (MessageBox.Show("Are you sure about deleting " + lootSkillBookList.Items[itemIndex].ToString() + "?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lootTemplates[templateIndex].skillBooks.RemoveAt(itemIndex);
                    lootSkillBookList.Items.RemoveAt(itemIndex);

                    selectNewItemAfterDelete(itemIndex, ref lootSkillBookList);

                    dealWithSkillBookButtons();
                }
            }
        }

       


        private void updateSkillBook_Click(object sender, EventArgs e)
        {
            if (lootSkillBookList.SelectedIndex >= 0)
            {
                try
                {
                    int itemIndex = lootSkillBookList.SelectedIndex;
                    int templateIndex = templateList.SelectedIndex;

                    int tier, chance;

                    if (!int.TryParse(bookTierBox.Text, out tier))
                    {
                        throw new Exception("Can't update! Tier is not a number!");
                    }

                    if (!int.TryParse(bookChanceBox.Text, out chance))
                    {
                        throw new Exception("Can't update! Chance is not a number!");
                    }

                    if (chance <= 0)
                    {
                        throw new Exception("Can't update! Chance can't be 0 or less");
                    }

                    if (tier <= 0 || tier >= 5)
                    {
                        throw new Exception("Can't update! Tier can be 1-4! \r\n1 - apprentice\r\n2 - expert\r\n3 - master\r\n4 - legendary");
                    }

   
                    lootTemplates[templateIndex].skillBooks[itemIndex].ChanceOfDrop = chance;
                    lootTemplates[templateIndex].skillBooks[itemIndex].skillTier = tier;

                    dealWithSkillBookButtons();

                    bookTierBox.BackColor = Color.White;
                    bookChanceBox.BackColor = Color.White;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private bool checkTextBoxes(Control parentControl)
        {
            var c = GetAll(parentControl, typeof(TextBox));
            foreach (var txtbox in c)
            {
                if (txtbox.BackColor == Color.Orange)
                {
                    if (MessageBox.Show("There are un-updated items, are you sure you want to change template?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void bookTierBox_TextChanged(object sender, EventArgs e)
        {
            bookTierBox.BackColor = Color.Orange;
        }

        private void bookChanceBox_TextChanged(object sender, EventArgs e)
        {
            bookChanceBox.BackColor = Color.Orange;
        }

        private void goldBox_TextChanged(object sender, EventArgs e)
        {
            goldBox.BackColor = Color.Orange;
        }

        private void magicQualityBox_TextChanged(object sender, EventArgs e)
        {
            magicQualityBox.BackColor = Color.Orange;
        }

        private void chestChanceBox_TextChanged(object sender, EventArgs e)
        {
            chestChanceBox.BackColor = Color.Orange;
        }

        private void chestQualityBox_TextChanged(object sender, EventArgs e)
        {
            chestQualityBox.BackColor = Color.Orange;
        }

        private void headBox_CheckedChanged(object sender, EventArgs e)
        {
            //headBox.BackColor = Color.Orange;
        }

        private void bloodBox_CheckedChanged(object sender, EventArgs e)
        {
            //bloodBox.BackColor = Color.Orange;
        }

        private void updateTemplate_Click(object sender, EventArgs e)
        {
            if (templateList.SelectedIndex >= 0)
            {
                int templateIndex = templateList.SelectedIndex;

                try
                {
                    if(!Dice.isValidDiceString(goldBox.Text) && goldBox.Text != "")
                    {
                        throw new Exception("Can't update! Gold is not valid die string!");
                    }

                    int magicQuality, chestChance, chestQuality;

                    if (!int.TryParse(magicQualityBox.Text, out magicQuality))
                    {
                        throw new Exception("Can't update! Magic quality is not a number!");
                    }

                    if (!int.TryParse(chestChanceBox.Text, out chestChance))
                    {
                        throw new Exception("Can't update! Chest chance it not a number!");
                    }

                    if (!int.TryParse(chestQualityBox.Text, out chestQuality))
                    {
                        throw new Exception("Can't update! Chest quality is not a number!"); 
                    }

                    if (chestChance < 0)
                    {
                        throw new Exception("Can't update! Chest chance can't be less than 0!");
                    }

                    if (chestQuality < 0)
                    {
                        throw new Exception("Can't update! Chest quality can't be less than 0");
                    }

                    if (chestChance > 100)
                    {
                        throw new Exception("Can't update! Chest chance can't be more than 100");
                    }

                    if (chestChance == 0 && chestQuality > 0)
                    {
                        throw new Exception("Can't update! You've set chest chance to 0, but have set chest quality more than 0");
                    }

                    if (chestChance > 0 && chestQuality == 0)
                    {
                        throw new Exception("Can't update! You've set chest chance, but have chest quality of 0!");
                    }

                    if (magicQuality < 0)
                    {
                        throw new Exception("Can't update! Magic quality can't be less than 0!");
                    }

                    if (magicQuality > 0 && lootRandMagicFromGroupList.Items.Count == 0)
                    {
                        throw new Exception("Can't update! You've set magic quality more than 0, but you don't drop any magic items!");
                    }

                    if(magicQuality == 0 && lootRandMagicFromGroupList.Items.Count > 0)
                    {
                        throw new Exception("Can't update! You've magic item drops, but you've set magic quality to 0");
                    }

                    lootTemplates[templateIndex].Gold = goldBox.Text;
                    lootTemplates[templateIndex].MagicQuality = magicQuality;
                    lootTemplates[templateIndex].ChestChance = chestChance;
                    lootTemplates[templateIndex].ChestQuality = chestQuality;

                    if (headBox.Checked)
                    {
                        lootTemplates[templateIndex].Head = 1;
                    }
                    else
                    {
                        lootTemplates[templateIndex].Head = 0;
                    }

                    if (bloodBox.Checked)
                    {
                        lootTemplates[templateIndex].Blood = 1;
                    }
                    else
                    {
                        lootTemplates[templateIndex].Blood = 0;
                    }

                    goldBox.BackColor = SystemColors.Control;
                    chestQualityBox.BackColor = Color.White;
                    chestChanceBox.BackColor = Color.White;
                    magicQualityBox.BackColor = Color.White;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
       
        private void checkAndTryToFixCamelcasingToolStripMenuItem_Click(object sender, EventArgs e)
        { /* redacted for now
            if (groupsLoaded && itemsLoaded && lootTableLoaded)
            {
                bool foundProblems = false;

                foreach(var template in lootTemplates)
                {

                    foreach(var itemGroup in itemGroups)
                    {
                        
                        foreach(var item in template.randItemFromGroup)
                        {
                            if (item.Name == itemGroup.Name)
                            {
                                item.CamelCaseOK = true;
                            }
                        }

                        foreach(var item in template.randItemFromGroup.Where(c => c.CamelCaseOK == false))
                        {
                            if (item.Name.ToLower() == itemGroup.Name.ToLower())
                            {
                                foundProblems = true;
                                item.Name = itemGroup.Name;
                                item.CamelCaseOK = true;
                            }
                        }

                        bool foundItemThatsNotInItemGroupFile;

                        foreach (var item in template.randItemFromGroup.Where(c => c.CamelCaseOK == false))
                        {
                            if (item.Name.ToLower() == itemGroup.Name.ToLower())
                            {
                                foundProblems = true;
                                item.Name = itemGroup.Name;
                                item.CamelCaseOK = true;
                            }
                        }



                    }
                }

                if (foundProblems)
                {
                    MessageBox.Show("Done, fixed some problems, export to save!");
                }
                else
                {
                    MessageBox.Show("Done, no problems found!");
                }
                

                templateList.SelectedIndex = templateList.SelectedIndex;
                
            }
            else
            {
                MessageBox.Show("All files must be loaded to do that!");
            }*/
        }

        private void simulButton_Click(object sender, EventArgs e)
        {
            if (templateList.SelectedIndex >= 0)
            {
                if (lootTableLoaded && itemsLoaded && groupsLoaded)
                {
                    Simulator.MakeLoot(lootTemplates[templateList.SelectedIndex], itemGroups, fullItems);

                    simList.Items.Clear();

                    foreach (var item in Simulator.getLootInfo())
                    {
                        simList.Items.Add(item);
                    }
                }              
          
            }

            
        }

        private void simList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string output = "";
            if (simList.Items.Count > 0)
            {
                foreach (var str in simList.Items)
                {
                    output += str + "\r\n";
                }

                Clipboard.SetText(output);
            }         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Item item;
            if (isFiltered)
            {
                item = filteredItems[itemList.SelectedIndex];
            }
            else
            {
                item = fullItems[itemList.SelectedIndex];
            }
            if (itemList.SelectedIndex >= 0)
            {
                MessageBox.Show("Debbuging: '" + item.getName() + "'");

                string debug = "// Debug -- global item list\r\n{\r\n";
                foreach (char c in item.getName())
                {
                    debug += string.Format("\tint({0}) char('{1}')", (int)c, c) + "\r\n";
                }
                debug += string.Format("\t len({0}) indexInGroup({1})\r\n", item.getName().Length, "-NaN-");
                debug += "}";
                Clipboard.SetText(debug);
            }
            
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
           

            if (lootGroupList.SelectedIndex >= 0 && itemsLoaded)
            {
                lootGroupItems.Items.Clear();

                int index = lootGroupList.SelectedIndex;
                int count = itemGroups[index].Items.Count();
                int fixd = 0;

                string output = "/// generated by Pol-Item-lootgroups\r\nfixes_in_" + itemGroups[index].Name + "\r\n{\r\n";

                List<string> toBeDeleted = new List<string>();
                
                for (int i = 0; i <= count -1; i++)
                {
                    var result = hexToStr(itemGroups[index].Items[i]);
                    if (result != null)
                    {
                        lootGroupItems.Items.Add(result);
                        if (itemGroups[index].Items[i] != result)
                        {
                            output += string.Format("\t{0} => {1}\r\n", itemGroups[index].Items[i], result);
                            fixd++;
                        }
                        itemGroups[index].Items[i] = result;
                    }
                    else
                    {
                        output += string.Format("\t{0} => {1}\r\n", itemGroups[index].Items[i], "Deleted");
                        toBeDeleted.Add(itemGroups[index].Items[i]);
                        MessageBox.Show("Did not find item '" + itemGroups[index].Items[i] + "', ignoring it!");
                    }                   
                }

                output += "}";

                if(fixd > 0)
                {
                    if (MessageBox.Show("Fixed " + fixd.ToString() + " errors! Do you want fixed to be copied to clipboard?", "Info", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Clipboard.SetText(output);
                    }
                }
                else
                {
                    MessageBox.Show("No errors found!");
                }

                if (toBeDeleted.Count > 0)
                {
                    foreach (var item in toBeDeleted)
                    {
                        itemGroups[index].Items.Remove(item);
                    }
                }
            }           
        }

        private string hexToStr(string hex)
        {
            foreach(var item in fullItems)
            {
                if (item.getObjNumberHex().ToLower() == hex.ToLower())
                {
                    return item.getName();
                }

                if (item.getName().Trim() == hex)
                {
                    return item.getName();
                }

                if (item.getName().Trim().ToLower() == hex.ToLower())
                {
                    return item.getName();
                }
            }
            return null;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*
            string output = "/// generated by Pol-Item-lootgroups\r\nAllowedMagicItems\r\n{\r\n";

            foreach(MagicAllowed.MagicAllow item in MagicAllowed.AllowedMagicItems)
            {
                output += string.Format("\t{0} => {1}\r\n", item.Name, item.MagicType);
            }     

            output += "}";

            Clipboard.SetText(output);
            */

            int min = Dice.getRandom(1, 100);
            int max = Dice.getRandom(1, 100);

            for (int i = 1; i <= 10000; i++)
            {
                int val = Dice.getRandom(1, 100);

                if (val < min)
                {
                    min = val;
                }

                if (val > max)
                {
                    max = val;
                }
            }

            MessageBox.Show(string.Format("Min: {0} Max: {1}", min, max));
        }

        private void exportTemplate_Click(object sender, EventArgs e)
        {
            string fileData = Parser.exportLootTemplates(lootTemplates);

            System.IO.Stream myStream;
            SaveFileDialog saveFileDiag = new SaveFileDialog();
            saveFileDiag.Filter = "npc_lootgroups.cfg file|*.cfg";

            if (saveFileDiag.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDiag.OpenFile()) != null)
                {
                    byte[] data = Encoding.UTF8.GetBytes(fileData);
                    myStream.Write(data, 0, data.Length);
                    myStream.Close();
                }
            }
        }


        private void selectNewItemAfterDelete(int selectedIndex, ref ListBox listbox)
        {
            if (listbox.Items.Count > 1)
            {

                int newIndex = selectedIndex - 1;

                if (selectedIndex <= 0)
                {
                    newIndex = selectedIndex;
                }

                listbox.SelectedIndex = newIndex;
            }
            else if (listbox.Items.Count == 1)
            {
                MessageBox.Show("got here");
                listbox.SelectedIndex = 0;
            }
        }
    }
}
