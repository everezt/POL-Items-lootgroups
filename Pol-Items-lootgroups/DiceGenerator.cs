using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pol_Items_lootgroups.Classes
{
    public partial class DiceGenerator : Form
    {
        TextBox passedTextBox;
        Point passedTextBoxLoc;
        bool allowGeneration = false;

        public DiceGenerator(ref TextBox target, Point targetLoc)
        {
            passedTextBox = target;
            passedTextBoxLoc = targetLoc;
            InitializeComponent();
        }

        // make the form non-movable
        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
                        
        }

        private void DiceGenerator_Shown(object sender, EventArgs e)
        {
            this.Location = new Point(passedTextBoxLoc.X - passedTextBox.Width - 23, passedTextBoxLoc.Y - this.Height + 8);

            if (passedTextBox.Text == "")
            {
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                numericUpDown3.Value = 0;

                dieStringLabel.Text = "Gold is set to NULL";
                allowGeneration = true;
            }
            else
            {

                if (Dice.isValidDieString(passedTextBox.Text))
                {
                    try
                    {
                        int[] result = Dice.parseDiceString(passedTextBox.Text);
                        /*
                        if (result[1] < 0)
                        {
                            MessageBox.Show("It's not a valid die string! Minimum can't be lower than 0! Setting it to 0!");
                            result[1] = 0;
                        }
                        */


                        if (result[0] > 16)
                        {
                            MessageBox.Show("This die string will take too many iterations, generating new one!");
                            string res = Dice.generateDieString(result[1], result[2]);
                            passedTextBox.Text = res;
                            result = Dice.parseDiceString(res);
                        }

                        numericUpDown1.Value = result[0];
                        numericUpDown2.Value = result[1];
                        numericUpDown3.Value = result[2];

                        dieStringLabel.Text = "Die string: " + passedTextBox.Text;
                        allowGeneration = true;
                    }
                    catch
                    {
                        MessageBox.Show("Something went wrong while calculating min-max! Generating new die string!");
                        allowGeneration = true;
                        doGeneration();
                    }

                }
                else
                {
                    MessageBox.Show("It's not a valid die string, generating new one!");
                    allowGeneration = true;
                    doGeneration();
                }
            }
            
        }
        
        private void doGeneration()
        {
            if (allowGeneration)
            {
                string result = Dice.generateDieString(int.Parse(numericUpDown2.Value.ToString()), int.Parse(numericUpDown3.Value.ToString()));



                if (Dice.isValidDieString(result))
                {
                    int[] intRes = Dice.parseDiceString(result);

                    numericUpDown1.Value = intRes[0];
                    numericUpDown2.Value = intRes[1];
                    numericUpDown3.Value = intRes[2];

                    passedTextBox.Text = result;

                    dieStringLabel.Text = "Die string: " + result;
                }
                else
                {
                    if (result.Contains("NULL"))
                    {
                        passedTextBox.Text = "";
                    }
                    dieStringLabel.Text = result;
                }
            }
        }

        private void generateBtn_Click(object sender, EventArgs e)
        {
            doGeneration();
        }
    }
}
