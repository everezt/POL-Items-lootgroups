using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pol_Items_lootgroups.Classes
{
    static class Dice
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);

        public static bool isValidDiceString(string str)
        {
            int x;

            if (str == null)
            {
                return false;
            }

            if (int.TryParse(str, out x))
            {
                return true;
            }
            else
            {
                if (str.ToLower().Contains('d'))
                {
                    // determine if there is only one d in string
                    int dCount = 0;

                    foreach (char chr in str.ToLower())
                    {
                        if (chr == 'd')
                        {
                            dCount++;
                        }
                    }

                    if (dCount > 1)
                    {
                        // we have too many 'd' chars in string return false
                        return false;
                    }

                    string[] par = str.ToLower().Split(new string[] { "d" }, StringSplitOptions.RemoveEmptyEntries);

                    if (par.Count() == 2)
                    {
                        int y;
                        if (int.TryParse(par[0], out y))
                        {
                            if(int.TryParse(par[1], out y))
                            {
                                return true;
                            }
                            else
                            {

                                if (par[1].Contains("+"))
                                {
                                    string[] extPar = par[1].Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);

                                    if (extPar.Count() == 2)
                                    {
                                        if (int.TryParse(extPar[0], out y) && int.TryParse(extPar[1], out y))
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }

                                if (par[1].Contains("-"))
                                {
                                    string[] extPar = par[1].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

                                    if (extPar.Count() == 2)
                                    {
                                        if (int.TryParse(extPar[0], out y) && int.TryParse(extPar[1], out y))
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }

                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }                   
                }
                else
                {
                    return false;
                }
            }
        }

        public static int[] parseDiceString(string dieString)
        {

            int dices = 0,
                min = 0,
                max = 0,
                level = 0,
                damage = 0,
                sides = 0;

            if (int.TryParse(dieString, out level))
            {
                dices = 1;
                min = level;
                max = level;
                damage = 0;
                sides = 0;
                return new int[] { dices, min, max, damage, sides };
            }

            string[] para = dieString.ToLower().Split(new string[] { "d" }, StringSplitOptions.RemoveEmptyEntries);

            dices = int.Parse(para[0]);

            if (!int.TryParse(para[1], out level))
            {
                if (para[1].Contains("+"))
                {
                    string[] extPara = para[1].Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
                    level = int.Parse(extPara[0]);
                    sides = level;
                    damage = int.Parse(extPara[1]);
                    min = (dices + int.Parse(extPara[1]));
                    max = ((dices * level) + int.Parse(extPara[1]));
                    return new int[] { dices, min, max, damage, sides };
                }
                else
                {
                    string[] extPara = para[1].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    level = int.Parse(extPara[0]);
                    sides = level;
                    damage = int.Parse(extPara[1]);
                    damage = damage - (damage * 2);
                    min = (dices - int.Parse(extPara[1]));
                    max = ((dices * level) - int.Parse(extPara[1]));
                    return new int[] { dices, min, max, damage, sides };
                }

            }
            else
            {
                min = dices;
                max = dices * level;
                sides = level;

                
                return new int[] { dices, min, max, damage, sides };
            }
        }

        public static string generateDiceString(int min, int max)
        {

            int extra = 0;
            int piles = 0;
            int tempPiles = 0;
            int tempExtra = 0;

            int dices = 0;

            if(min == 0 && max == 0)
            {
                return "Setting value to NULL";
            }

            if (min == max)
            {
                return min.ToString();
            }

            if (max < min)
            {
                return "Max can't be smaller than Min";
            }

            if (min == 0)
            {
                dices = 1;
                piles = max + 1;
                extra = 1;

                return String.Format("{0}d{1}-{2}", dices, piles, extra);
            }

            // Im limiting dices to 16, since i don't want to have 800 or so iterations on POL dice.roll
            for (int i = 1; i <= 16; i++)
            {
                tempExtra = (min - i);

                tempPiles = (max - tempExtra) / i;

                if (((tempPiles * i) + tempExtra) == max)
                {
                    dices = i;
                    piles = tempPiles;
                    extra = tempExtra;
                }

            }        

            return String.Format( "{0}d{1}{2}", dices, piles, (extra != 0) ? (extra < 0) ? "-" + Math.Abs(extra)  : "+" + Math.Abs(extra) : "");
             
        }

       

        public static int getRandom(int min, int max)
        {           
            return rnd.Next(min, max + 1);
        }

        public static int Roll(string diceString)
        {
            
            int[] result = Dice.parseDiceString(diceString);
            int dices = result[0];
            int min = result[1];
            int max = result[2];
            int damage = result[3];
            int sides = result[4];

            /*
            if (dieString == "5d2-4")
            {
                MessageBox.Show(string.Format("dices({0}) min({1}) max({2}) damage({3}) sides({4})", dices, min, max, damage, sides));
            }
            */


            if (min == max)
            {
                return min;
            }

            int totalSum = 0;
            
            for(int i = 1; i <= dices; i++)
            {
                totalSum += getRandom(1, sides);
            }

            totalSum += damage;

            return totalSum;
        }


    }
}
