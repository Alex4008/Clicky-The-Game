using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Threading;

namespace Thingy
{
    public partial class Clicky : Form
    {
        //Init cost for upgrades
        public double extraClickCost = 100;
        public double dullCost = 15;
        public double pointyCost = 100;
        public double pointierCost = 1100;
        public double sharpCost = 12000;
        public double sharperCost = 130000;
        public double sharpestCost = 1400000;

        //Init amount of upgrades (will change for loaded saves)
        public int extraClickAmount = 0;
        public int dullAmount = 0;
        public int pointyAmount = 0;
        public int pointierAmount = 0;
        public int sharpAmount = 0;
        public int sharperAmount = 0;
        public int sharpestAmount = 0;

        public double tokensPerSec; //All upgrades calculated together. Total tokens per sec. Calculated in "ScreenRefresh"
        public double userTokens = 0; //Current Balance of users tokens
        public double userScore = 0; //Complete history of users tokens

        public Boolean shutdown = false;
        public Boolean noSave = false;
        string path = "C:\\Users/Public/Documents/ClickyTheGame-Savefile.txt"; //Path to save the file

        System.Timers.Timer scaleableTimer = new System.Timers.Timer(); //Scaleable timer
        System.Timers.Timer oneSecondTimer = new System.Timers.Timer(); //One sec timer
        public double speed; //Used for the timer. Calculates spee of the scaleable timer.

        public int punishmentCounter;

        public int prestigeLevel = 0;
        public double prestigeMultipier = 0;
        public double prestigeMultipierDecimal = 0;
        public int prestigeCost = 1000000;
        public int timerSec = 0;
        public int timerMin = 0;
        public int timerHour = 0;
        public int timerSecDisplay = 0;
        public int timerMinDisplay = 0;
        public int timerHourDisplay = 0;

        public int timerSecPrestige = 0;
        public int timerMinPrestige = 0;
        public int timerHourPrestige = 0;
        public int timerSecPrestigeDisplay = 0;
        public int timerMinPrestigeDisplay = 0;
        public int timerHourPrestigeDisplay = 0;

        public Boolean prestigeInProcess = false;
        public int clicksInASec = 0; //Auto clicker prevention
        public double prestigeCalcNumberTotal;
        public Boolean prestigeConfirm = false;
        public Boolean resetConfirm = false;

        public double tokensPerSecWithoutBonus;

        public int prestigeConfirmCounter = 0;
        public int resetConfirmCounter = 0;

        public int dullUpgradeLv = 0;
        public int pointyUpgradeLv = 0;
        public int pointierUpgradeLv = 0;
        public int sharpUpgradeLv = 0;
        public int sharperUpgradeLv = 0;
        public int sharpestUpgradeLv = 0;

        public double dullMult = 0;
        public double pointyMult = 0;
        public double pointierMult = 0;
        public double sharpMult = 0;
        public double sharperMult = 0;
        public double sharpestMult = 0;

        public double dullTPS = 0;
        public double pointyTPS = 0;
        public double pointierTPS = 0;
        public double sharpTPS = 0;
        public double sharperTPS = 0;
        public double sharpestTPS = 0;


        public void UpdateUpgrades()
        {
            if (dullUpgradeLv == 3)
            {
                dullMult = (0.25 + 0.5 + 1) + 1;
            }
            else if (dullUpgradeLv == 2)
            {
                dullMult = (0.25 + 0.5) + 1;
            }
            else if (dullUpgradeLv == 1)
            {
                dullMult = (0.25) + 1;
            }
            else
            {
                dullMult = 1;
            }

            if (pointyUpgradeLv == 3)
            {
                pointyMult = (0.25 + 0.5 + 1) + 1;
            }
            else if (pointyUpgradeLv == 2)
            {
                pointyMult = (0.25 + 0.5) + 1;
            }
            else if (pointyUpgradeLv == 1)
            {
                pointyMult = (0.25) + 1;
            }
            else
            {
                pointyMult = 1;
            }

            if (pointierUpgradeLv == 3)
            {
                pointierMult = (0.25 + 0.5 + 1) + 1;
            }
            else if (pointierUpgradeLv == 2)
            {
                pointierMult = (0.25 + 0.5) + 1;
            }
            else if (pointierUpgradeLv == 1)
            {
                pointierMult = (0.25) + 1;
            }
            else
            {
                pointierMult = 1;
            }

            if (sharpUpgradeLv == 3)
            {
                sharpMult = (0.25 + 0.5 + 1) + 1;
            }
            else if (sharpUpgradeLv == 2)
            {
                sharpMult = (0.25 + 0.5) + 1;
            }
            else if (sharpUpgradeLv == 1)
            {
                sharpMult = (0.25) + 1;
            }
            else
            {
                sharpMult = 1;
            }

            if (sharperUpgradeLv == 3)
            {
                sharperMult = (0.25 + 0.5 + 1) + 1;
            }
            else if (sharperUpgradeLv == 2)
            {
                sharperMult = (0.25 + 0.5) + 1;
            }
            else if (sharperUpgradeLv == 1)
            {
                sharperMult = (0.25) + 1;
            }
            else
            {
                sharperMult = 1;
            }

            if (sharpestUpgradeLv == 3)
            {
                sharpestMult = (0.25 + 0.5 + 1) + 1;
            }
            else if (sharpestUpgradeLv == 2)
            {
                sharpestMult = (0.25 + 0.5) + 1;
            }
            else if (sharpestUpgradeLv == 1)
            {
                sharpestMult = (0.25) + 1;
            }
            else
            {
                sharpestMult = 1;
            }
        }

        public string FormatNumber(double numberToFormat)
        {
            string formatedNumber;

            formatedNumber = String.Format("{0:n}", numberToFormat);

            return formatedNumber;
        }

        public void TPSManagement()
        {
            dullTPS = (dullAmount * 0.1) * dullMult;
            pointyTPS = (pointyAmount * 1) * pointyMult;
            pointierTPS = (pointierAmount * 8) * pointierMult;
            sharpTPS = (sharpAmount * 47) * sharpMult;
            sharperTPS = (sharperAmount * 260) * sharperMult;
            sharpestTPS = (sharpestAmount * 1400) * sharpestMult;

            tokensPerSecWithoutBonus = (dullTPS) + (pointyTPS) + (pointierTPS) + (sharpTPS) + (sharperTPS) + (sharpestTPS);

            tokensPerSec = (tokensPerSecWithoutBonus) * (prestigeMultipier); //Calculates total TPS with bonus
        }

        public void DisplayCosts()
        {
            //Displaying Tokens
            if (extraClickCost >= 1000000000000) //Trillion
            {
                double userTokensTrillion = Math.Round(userTokens / 1000000000000, 2);
                lblScore.Text = "Total Tokens: " + userTokensTrillion.ToString() + " Trillion"; //Set the token amount
            }
            else if (userTokens >= 1000000000) //Billion
            {
                double userTokensBillion = Math.Round(userTokens / 1000000000, 2);
                lblScore.Text = "Total Tokens: " + userTokensBillion.ToString() + " Billion"; //Set the token amount
            }
            else if (userTokens >= 1000000) //Million
            {
                double userTokensMillion = Math.Round(userTokens / 1000000, 2);
                lblToken.Text = "Total Tokens: " + userTokensMillion.ToString() + " Million"; //Set the token amount
            }
            else
            {
                lblToken.Text = "Total Tokens: " + String.Format("{0:#,##0}", userTokens); //Set the token amount
            }

            //Extra Clicks Cost
            if (extraClickCost >= 1000000000000)
            {
                double cost = Math.Round(extraClickCost / 1000000000000, 2);
                lblExtraCost.Text = "Extra Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (extraClickCost >= 1000000000)
            {
                double cost = Math.Round(extraClickCost / 1000000000, 2);
                lblExtraCost.Text = "Extra Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (extraClickCost >= 1000000)
            {
                double cost = Math.Round(extraClickCost / 1000000, 2);
                lblExtraCost.Text = "Extra Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblExtraCost.Text = "Extra Click Cost: " + String.Format("{0:#,##0}", extraClickCost); //Set the cost
            }

            //Dull Clicks Cost
            if (dullCost >= 1000000000000)
            {
                double cost = Math.Round(dullCost / 1000000000000, 2);
                lblDullCost.Text = "Dull Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (dullCost >= 1000000000)
            {
                double cost = Math.Round(dullCost / 1000000000, 2);
                lblDullCost.Text = "Dull Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (dullCost >= 1000000)
            {
                double cost = Math.Round(dullCost / 1000000, 2);
                lblDullCost.Text = "Dull Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblDullCost.Text = "Dull Click Cost: " + String.Format("{0:#,##0}", dullCost); //Set the cost
            }

            //Pointy Click Cost
            if (pointyCost >= 1000000000000)
            {
                double cost = Math.Round(pointyCost / 1000000000000, 2);
                lblPointyCost.Text = "Pointy Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (pointyCost >= 1000000000)
            {
                double cost = Math.Round(pointyCost / 1000000000, 2);
                lblPointyCost.Text = "Pointy Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (pointyCost >= 1000000)
            {
                double cost = Math.Round(pointyCost / 1000000, 2);
                lblPointyCost.Text = "Pointy Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblPointyCost.Text = "Pointy Click Cost: " + String.Format("{0:#,##0}", pointyCost); //Set the cost
            }

            //Pointier Click Cost
            if (pointierCost >= 1000000000000)
            {
                double cost = Math.Round(pointierCost / 1000000000000, 2);
                lblPointierCost.Text = "Pointier Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (pointierCost >= 1000000000)
            {
                double cost = Math.Round(pointierCost / 1000000000, 2);
                lblPointierCost.Text = "Pointier Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (pointierCost >= 1000000)
            {
                double cost = Math.Round(pointierCost / 1000000, 2);
                lblPointierCost.Text = "Pointier Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblPointierCost.Text = "Pointier Click Cost: " + String.Format("{0:#,##0}", pointierCost); //Set the cost
            }

            //Sharp Click Cost
            if (sharpCost >= 1000000000000)
            {
                double cost = Math.Round(sharpCost / 1000000000000, 2);
                lblSharpCost.Text = "Sharp Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (sharpCost >= 1000000000)
            {
                double cost = Math.Round(sharpCost / 1000000000, 2);
                lblSharpCost.Text = "Sharp Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (sharpCost >= 1000000)
            {
                double cost = Math.Round(sharpCost / 1000000, 2);
                lblSharpCost.Text = "Sharp Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblSharpCost.Text = "Sharp Click Cost: " + String.Format("{0:#,##0}", sharpCost); //Set the cost
            }

            //Sharper Click Cost
            if (sharperCost >= 1000000000000)
            {
                double cost = Math.Round(sharperCost / 1000000000000, 2);
                lblSharperCost.Text = "Sharper Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (sharperCost >= 1000000000)
            {
                double cost = Math.Round(sharperCost / 1000000000, 2);
                lblSharperCost.Text = "Sharper Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (sharperCost >= 1000000)
            {
                double cost = Math.Round(sharperCost / 1000000, 2);
                lblSharperCost.Text = "Sharper Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblSharperCost.Text = "Sharper Click Cost: " + String.Format("{0:#,##0}", sharperCost); //Set the cost
            }

            //Sharpest Click Cost
            if (sharpestCost >= 1000000000000)
            {
                double cost = Math.Round(sharpestCost / 1000000000000, 2);
                lblSharpestCost.Text = "Sharper Click Cost: " + cost.ToString() + " Trillion"; //Set the cost
            }
            else if (sharpestCost >= 1000000000)
            {
                double cost = Math.Round(sharpestCost / 1000000000, 2);
                lblSharpestCost.Text = "Sharper Click Cost: " + cost.ToString() + " Billion"; //Set the cost
            }
            else if (sharpestCost >= 1000000)
            {
                double cost = Math.Round(sharpestCost / 1000000, 2);
                lblSharpestCost.Text = "Sharper Click Cost: " + cost.ToString() + " Million"; //Set the cost
            }
            else
            {
                lblSharpestCost.Text = "Sharper Click Cost: " + String.Format("{0:#,##0}", sharpestCost); //Set the cost
            }

            //Sets the amount of clicks the user has for each type.
            btnExtraClicks.Text = "Extra Clicks: " + extraClickAmount.ToString();
            btnDull.Text = "Dull Clicks: " + dullAmount.ToString();
            btnPointy.Text = "Pointy Clicks: " + pointyAmount.ToString();
            btnPointier.Text = "Pointier Clicks: " + pointierAmount.ToString();
            btnSharp.Text = "Sharp Clicks: " + sharpAmount.ToString();
            btnSharper.Text = "Sharper Clicks: " + sharperAmount.ToString();
            btnSharpest.Text = "Sharpest Clicks: " + sharpestAmount.ToString();
        }

        public void RefreshScreen()
        {
            //Show prestige box?
            if(prestigeLevel > 0 || sharperAmount > 0)
            {
                prestigeBox.Visible = true;
            }

             //Determine what version of dull upgrade box to show
            if(prestigeLevel >= 2 && dullUpgradeLv == 1 && dullAmount >= 1)
            {
                dullUpgradeBox.Visible = true;
                btnDullUpgradeTwo.Visible = true;
                lblDullUpgradeTwo.Visible = true;
            }
            else if(prestigeLevel >= 3 && dullUpgradeLv >= 2 && dullAmount >= 1)
            {
                dullUpgradeBox.Visible = true;
                btnDullUpgradeTwo.Visible = true;
                lblDullUpgradeTwo.Visible = true;
                btnDullUpgradeThree.Visible = true;
                lblDullUpgradeThree.Visible = true;
            }
            else if (prestigeLevel >= 2 && dullUpgradeLv >=2 && dullAmount >= 1)
            {
                dullUpgradeBox.Visible = true;
                btnDullUpgradeTwo.Visible = true;
                lblDullUpgradeTwo.Visible = true;
            }
            else if(prestigeLevel >= 1 && dullAmount >= 1)
            {
                dullUpgradeBox.Visible = true;
            }

            if (dullAmount > 0 || userTokens >= 100)
            {
                pointyBox.Visible = true;
                //Determine what version of pointy upgrade box to show
                if (prestigeLevel >= 2 && pointyUpgradeLv == 1 && pointyAmount >= 1)
                {
                    pointyUpgradeBox.Visible = true;
                    btnPointyUpgradeTwo.Visible = true;
                    lblPointyUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 3 && pointyUpgradeLv >= 2 && pointyAmount >= 1)
                {
                    pointyUpgradeBox.Visible = true;
                    btnPointyUpgradeTwo.Visible = true;
                    lblPointyUpgradeTwo.Visible = true;
                    btnPointyUpgradeThree.Visible = true;
                    lblPointyUpgradeThree.Visible = true;
                }
                else if (prestigeLevel >= 2 && pointyUpgradeLv >= 2 && pointyAmount >= 1)
                {
                    pointyUpgradeBox.Visible = true;
                    btnPointyUpgradeTwo.Visible = true;
                    lblPointyUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 1 && pointyAmount >= 1)
                {
                    pointyUpgradeBox.Visible = true;
                }
            }

            if(pointyAmount > 0 || userTokens >= 1100)
            {
                pointierBox.Visible = true;
                //Determine what version of pointier upgrade box to show
                if (prestigeLevel >= 2 && pointierUpgradeLv == 1 && pointierAmount >= 1)
                {
                    pointierUpgradeBox.Visible = true;
                    btnPointierUpgradeTwo.Visible = true;
                    lblPointierUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 3 && pointierUpgradeLv >= 2 && pointierAmount >= 1)
                {
                    pointierUpgradeBox.Visible = true;
                    btnPointierUpgradeTwo.Visible = true;
                    lblPointierUpgradeTwo.Visible = true;
                    btnPointierUpgradeThree.Visible = true;
                    lblPointierUpgradeThree.Visible = true;
                }
                else if (prestigeLevel >= 2 && pointierUpgradeLv >= 2 && pointierAmount >= 1)
                {
                    pointierUpgradeBox.Visible = true;
                    btnPointierUpgradeTwo.Visible = true;
                    lblPointierUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 1 && pointierAmount >= 1)
                {
                    pointierUpgradeBox.Visible = true;
                }
            }

            if (pointierAmount > 0 || userTokens >= 12000)
            {
                sharpBox.Visible = true;
                //Determine what version of sharp upgrade box to show
                if (prestigeLevel >= 2 && sharpUpgradeLv == 1 && sharpAmount >= 1)
                {
                    sharpUpgradeBox.Visible = true;
                    btnSharpUpgradeTwo.Visible = true;
                    lblSharpUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 3 && sharpUpgradeLv >= 2 && sharpAmount >= 1)
                {
                    sharpUpgradeBox.Visible = true;
                    btnSharpUpgradeTwo.Visible = true;
                    lblSharpUpgradeTwo.Visible = true;
                    btnSharpUpgradeThree.Visible = true;
                    lblSharpUpgradeThree.Visible = true;
                }
                else if (prestigeLevel >= 2 && sharpUpgradeLv >= 2 && sharpAmount >= 1)
                {
                    sharpUpgradeBox.Visible = true;
                    btnSharpUpgradeTwo.Visible = true;
                    lblSharpUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 1 && sharpAmount >= 1)
                {
                    sharpUpgradeBox.Visible = true;
                }
            }

            if (sharpAmount > 0 || userTokens >= 130000)
            {
                sharperBox.Visible = true;
                //Determine what version of sharper upgrade box to show
                if (prestigeLevel >= 2 && sharperUpgradeLv == 1 && sharperAmount >= 1)
                {
                    sharperUpgradeBox.Visible = true;
                    btnSharperUpgradeTwo.Visible = true;
                    lblSharperUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 3 && sharperUpgradeLv >= 2 && sharperAmount >= 1)
                {
                    sharperUpgradeBox.Visible = true;
                    btnSharperUpgradeTwo.Visible = true;
                    lblSharperUpgradeTwo.Visible = true;
                    btnSharperUpgradeThree.Visible = true;
                    lblSharperUpgradeThree.Visible = true;
                }
                else if (prestigeLevel >= 2 && sharperUpgradeLv >= 2 && sharperAmount >= 1)
                {
                    sharperUpgradeBox.Visible = true;
                    btnSharperUpgradeTwo.Visible = true;
                    lblSharperUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 1 && sharperAmount >= 1)
                {
                    sharperUpgradeBox.Visible = true;
                }
            }

            if (sharperAmount > 0 || userTokens >= 1400000)
            {
                sharpestBox.Visible = true;
                //Determine what version of pointy upgrade box to show
                if (prestigeLevel >= 2 && sharpestUpgradeLv == 1 && sharpestAmount >= 1)
                {
                    sharpestUpgradeBox.Visible = true;
                    btnSharpestUpgradeTwo.Visible = true;
                    lblSharpestUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 3 && sharpestUpgradeLv >= 2 && sharpestAmount >= 1)
                {
                    sharpestUpgradeBox.Visible = true;
                    btnSharpestUpgradeTwo.Visible = true;
                    lblSharpestUpgradeTwo.Visible = true;
                    btnSharpestUpgradeThree.Visible = true;
                    lblSharpestUpgradeThree.Visible = true;
                }
                else if (prestigeLevel >= 2 && sharpestUpgradeLv >= 2 && sharpestAmount >= 1)
                {
                    sharpestUpgradeBox.Visible = true;
                    btnSharpestUpgradeTwo.Visible = true;
                    lblSharpestUpgradeTwo.Visible = true;
                }
                else if (prestigeLevel >= 1 && sharpestAmount >= 1)
                {
                    sharpestUpgradeBox.Visible = true;
                }
            }

            if (extraClickAmount != 0 || userTokens >= extraClickCost)
            {
                extraBox.Visible = true;
            }

            DisplayCosts();

            UpdateUpgrades();
            TPSManagement();

            if (prestigeLevel > 0) //If prestiged
            {
                lblBonusTPS.Visible = true;

                lblTokensPerSecWithoutBonus.Visible = true;
                lblGenWithoutBonus.Visible = true;
                lblTokensPerSecWithoutBonus.Text = String.Format("{0:n}", tokensPerSecWithoutBonus).ToString() + " (Without Prestige Bonus)"; //Sets the Tokens per second label without bonus
                lblTokensPerSec.Text = String.Format("{0:n}", tokensPerSec).ToString() + " (With Prestige Bonus)"; //Sets the Tokens per second label with bonus
                lblTimeSinceLastPrestige.Visible = true; //Set the timer for last prestige
                lblTimeSinceLastPrestigeLabel.Visible = true; //Set the timer for last prestige
            }
            else //If not
            {
                lblBonusTPS.Visible = false;

                lblTokensPerSecWithoutBonus.Text = String.Format("{0:n}", tokensPerSecWithoutBonus).ToString(); //Sets the Tokens per second label
                lblTokensPerSecWithoutBonus.Visible = false;
                lblGenWithoutBonus.Visible = false;
                lblTokensPerSec.Text = tokensPerSec.ToString(); //Sets the Tokens per second label with bonus
                lblTimeSinceLastPrestige.Visible = false; //Set the timer for last prestige
                lblTimeSinceLastPrestigeLabel.Visible = false; //Set the timer for last prestige
            }
            lblTokensPerClick.Text = (extraClickAmount + 1).ToString(); //Sets tokens per click label

            if (userScore >= 1000000000) //Billion
            {
                double userScoreBillion = Math.Round(userScore / 1000000000, 2);

                lblScore.Text = "Total Score: " + userScoreBillion.ToString() + " Billion"; //Sets the score label
            }
            else if (userScore >= 1000000) //Million
            {
                double userScoreMillion = Math.Round(userScore / 1000000, 2);

                lblScore.Text = "Total Score: " + userScoreMillion.ToString() + " Million"; //Sets the score label
            }
            else
            {
                lblScore.Text = "Total Score: " + String.Format("{0:#,##0}", userScore); //Sets the score label
            }

            lblDullTPS.Text = "Dull Click TPS: " + FormatNumber(dullTPS);
            lblPointyTPS.Text = "Pointy Click TPS: " + FormatNumber(pointyTPS);
            lblPointierTPS.Text = "Pointier Click TPS: " + FormatNumber(pointierTPS);
            lblSharpTPS.Text = "Sharp Click TPS: " + FormatNumber(sharpTPS);
            lblSharperTPS.Text = "Sharper Click TPS: " + FormatNumber(sharperTPS);
            lblSharpestTPS.Text = "Sharpest Click TPS: " + FormatNumber(sharpestTPS);

            prestigeMultipier = (prestigeMultipierDecimal + 1); //Calculate the prestige multipler for TPS

            lblPrestige.Text = prestigeCost.ToString(); //Set the cost
            lblPrestigeLevel.Text = "Prestige Level: " + prestigeLevel.ToString(); //Sets the prestige level    

            lblBonusTPS.Text = (prestigeMultipier).ToString() + "x TPS Prestige Bonus"; //Refreshes bonus % label

            //Clock Code.

            //Since last prestige clock
            if (timerSecPrestige > 3600)
            {
                timerMinPrestige = timerSecPrestige / 60;

                timerHourPrestigeDisplay = (timerSecPrestige / 60) / 60;

                timerMinPrestigeDisplay = timerMinPrestige % 60;

                timerSecPrestigeDisplay = (timerSecPrestige % 60);

                lblTimeSinceLastPrestige.Text = timerHourPrestigeDisplay.ToString() + " (hour) " + timerMinPrestigeDisplay.ToString() + " (min) " + timerSecPrestigeDisplay.ToString() + " (sec)"; //Sets the time since last prestige
            }
            else if (timerSecPrestige > 60)
            {
                timerMinPrestige = timerSecPrestige / 60;

                timerMinPrestigeDisplay = timerMinPrestige;

                timerSecPrestigeDisplay = (timerSecPrestige % 60);

                lblTimeSinceLastPrestige.Text = timerMinPrestigeDisplay.ToString() + " (min) " + timerSecPrestigeDisplay.ToString() + " (sec)"; //Sets the time since last prestige
            }
            else
            {
                timerSecPrestigeDisplay = timerSecPrestige;

                lblTimeSinceLastPrestige.Text = timerSecPrestigeDisplay.ToString() + " (sec)"; //Sets the time since last prestige
            }

            //Since start clock
            if (timerSec > 3600)
            {
                timerMin = timerSec / 60;

                timerHourDisplay = (timerSec / 60) / 60;

                timerMinDisplay = timerMin % 60;

                timerSecDisplay = (timerSec % 60);

                lblTimer.Text = timerHourDisplay.ToString() + " (hour) " + timerMinDisplay.ToString() + " (min) " + timerSecDisplay.ToString() + " (sec)"; //Refreshes Timer
            }
            else if(timerSec > 60)
            {
                timerMin = timerSec / 60;

                timerMinDisplay = timerMin;

                timerSecDisplay = (timerSec % 60);

                lblTimer.Text = timerMinDisplay.ToString() + " (min) " + timerSecDisplay.ToString() + " (sec)"; //Refreshes Timer
            }
            else
            {
                timerSecDisplay = timerSec;

                lblTimer.Text = timerSecDisplay.ToString() + " (sec)"; //Refreshes Timer
            }

        }
        public void SaveGame()
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(userTokens);
                sw.WriteLine(userScore);
                sw.WriteLine(extraClickAmount);
                sw.WriteLine(dullAmount);
                sw.WriteLine(pointyAmount);
                sw.WriteLine(pointierAmount);
                sw.WriteLine(sharpAmount);
                sw.WriteLine(sharperAmount);
                sw.WriteLine(sharpestAmount);
                sw.WriteLine(prestigeLevel);
                sw.WriteLine(timerSec);
                sw.WriteLine(timerSecPrestige);
                sw.WriteLine(prestigeMultipierDecimal);
                sw.WriteLine(dullUpgradeLv);
                sw.WriteLine(pointyUpgradeLv);
                sw.WriteLine(pointierUpgradeLv);
                sw.WriteLine(sharpUpgradeLv);
                sw.WriteLine(sharperUpgradeLv);
                sw.WriteLine(sharpestUpgradeLv);
            }
        }
        public void LoadGame()
        {
            using (StreamReader sr = File.OpenText(path))
            {
                int count = 0;
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    if (count == 0) // User tokens
                    {
                        userTokens = Double.Parse(s);
                    }
                    else if (count == 1) // User Score
                    {
                        userScore = Double.Parse(s);
                    }
                    else if (count == 2) // Amount of Extra Clicks
                    {
                        extraClickAmount = Int32.Parse(s);
                    }
                    else if (count == 3) // Amount of Dull Clicks
                    {
                        dullAmount = Int32.Parse(s);
                    }
                    else if (count == 4) // Amount of Pointy Clicks
                    {
                        pointyAmount = Int32.Parse(s);
                    }
                    else if (count == 5) // Amount of Pointier Clicks
                    {
                        pointierAmount = Int32.Parse(s);
                    }
                    else if (count == 6) // Amount of Sharp Clicks
                    {
                        sharpAmount = Int32.Parse(s);
                    }
                    else if (count == 7) // Amount of Sharper Clicks
                    {
                        sharperAmount = Int32.Parse(s);
                    }
                    else if (count == 8) // Amount of Sharpest Clicks
                    {
                        sharpestAmount = Int32.Parse(s);
                    }
                    else if (count == 9) // Prestige level
                    {
                        prestigeLevel = Int32.Parse(s);
                    }
                    else if (count == 10) // Timer in seconds
                    {
                        timerSec = Int32.Parse(s);
                    }
                    else if(count == 11) //Time since last prestige
                    {
                        timerSecPrestige = Int32.Parse(s);
                    }
                    else if (count == 12) //Prestige multiplier
                    {
                        prestigeMultipierDecimal = Double.Parse(s);
                    }
                    else if (count == 13) //Dull upgrade level
                    {
                        dullUpgradeLv = Int32.Parse(s);
                    }
                    else if (count == 14) //Pointy upgrade level
                    {
                        pointyUpgradeLv = Int32.Parse(s);
                    }
                    else if (count == 15) //Pointier upgrade level
                    {
                        pointierUpgradeLv = Int32.Parse(s);
                    }
                    else if (count == 16) //Sharp upgrade level
                    {
                        sharpUpgradeLv = Int32.Parse(s);
                    }
                    else if (count == 17) //Sharper upgrade level
                    {
                        sharperUpgradeLv = Int32.Parse(s);
                    }
                    else if (count == 18) //Sharpest upgrade level
                    {
                        sharpestUpgradeLv = Int32.Parse(s);
                    }
                    count++;
                }
            }
        }

        public double CostCalc(int upgrade, double upgradeCost, double amount)
        {
            int i = 0;
            while (upgrade > i)
            {
                upgradeCost = upgradeCost * amount;
                upgradeCost = Math.Round(upgradeCost);
                i++;
            }
            i = 0;

            return upgradeCost;
        }

        public double PrestigeCostCalc(int upgrade, double upgradeCost)
        {
            double pCostIncrease = 0;

            int i = 0;
            while (upgrade > i)
            {
                pCostIncrease = (((i+3)*2) / 100) + 1;
                upgradeCost = upgradeCost * pCostIncrease;
                upgradeCost = (pCostIncrease);
                i++;
            }
            i = 0;
            return upgradeCost;
        }

        public void VisibilityFalse()
        {
            dullBox.Visible = true;
            pointyBox.Visible = false;
            pointierBox.Visible = false;
            sharpBox.Visible = false;
            sharperBox.Visible = false;
            sharpestBox.Visible = false;
            extraBox.Visible = false;
            prestigeBox.Visible = false;
            lblTokensPerSecWithoutBonus.Visible = false;
            lblGenWithoutBonus.Visible = false;
            lblBonusTPS.Visible = false;

            btnDullUpgradeTwo.Visible = false;
            btnDullUpgradeThree.Visible = false;
            btnPointyUpgradeTwo.Visible = false;
            btnPointyUpgradeThree.Visible = false;
            btnPointierUpgradeTwo.Visible = false;
            btnPointierUpgradeThree.Visible = false;
            btnSharpUpgradeTwo.Visible = false;
            btnSharpUpgradeThree.Visible = false;
            btnSharperUpgradeTwo.Visible = false;
            btnSharperUpgradeThree.Visible = false;
            btnSharpestUpgradeTwo.Visible = false;
            btnSharpestUpgradeThree.Visible = false;

            lblDullUpgradeTwo.Visible = false;
            lblDullUpgradeThree.Visible = false;
            lblPointyUpgradeTwo.Visible = false;
            lblPointyUpgradeThree.Visible = false;
            lblPointierUpgradeTwo.Visible = false;
            lblPointierUpgradeThree.Visible = false;
            lblSharpUpgradeTwo.Visible = false;
            lblSharpUpgradeThree.Visible = false;
            lblSharperUpgradeTwo.Visible = false;
            lblSharperUpgradeThree.Visible = false;
            lblSharpestUpgradeTwo.Visible = false;
            lblSharpestUpgradeThree.Visible = false;

            dullUpgradeBox.Visible = false;
            pointyUpgradeBox.Visible = false;
            pointierUpgradeBox.Visible = false;
            sharpUpgradeBox.Visible = false;
            sharperUpgradeBox.Visible = false;
            sharpestUpgradeBox.Visible = false;

            lblTimeSinceLastPrestige.Visible = false;
            lblTimeSinceLastPrestigeLabel.Visible = false;
        }

        public Clicky()
        {
            
            InitializeComponent();

            VisibilityFalse();

            if (!File.Exists(path))
            {
                MessageBox.Show("Welcome to Clicky - The Game! A New Game is Being Created For You!");
                SaveGame();
            }
            else
            {
                LoadGame();
            }

            //Calculating Cost
            extraClickCost = CostCalc(extraClickAmount, extraClickCost, 1.35);

            dullCost = CostCalc(dullAmount, dullCost, 1.08);
            pointyCost = CostCalc(pointyAmount, pointyCost, 1.15);
            pointierCost = CostCalc(pointierAmount, pointierCost, 1.15);
            sharpCost = CostCalc(sharpAmount, sharpCost, 1.15);
            sharperCost = CostCalc(sharperAmount, sharperCost, 1.15);
            sharpestCost = CostCalc(sharpestAmount, sharpestCost, 1.15);

            double pCostIncrease;
            double i = 0;
            while (prestigeLevel > i)
            {
                pCostIncrease = (((i + 1) * 2) / 100) + 1;
                prestigeCost = (int)(Math.Round(prestigeCost * pCostIncrease));
                i++;
            }
            i = 0;

            RefreshScreen(); //Refresh the screen now that all the values are loaded from save

            //One Second Timer
            oneSecondTimer.Elapsed += new ElapsedEventHandler(OneSecondTimer);
            oneSecondTimer.Interval = 1000; // 1000 ms is one second
            oneSecondTimer.Start();

            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void OneSecondTimer(object source, ElapsedEventArgs e) //One second timer
        {
            if(prestigeInProcess == false)
            {
                if (shutdown == true)
                {
                    this.Close();
                }

                if (noSave == false && shutdown == false)
                {
                    SaveGame();
                }

                if (prestigeLevel > 0)
                {
                    timerSecPrestige++;
                }
                timerSec++;
                RefreshScreen();
                clicksInASec = 0;
                if (tokensPerSec != 0)
                {
                    userTokens = Math.Round((userTokens + tokensPerSec), 1);
                    userScore = Math.Round((userScore + tokensPerSec), 1);
                }

                if(resetConfirm == true)
                {
                    resetConfirmCounter++;
                    if (resetConfirmCounter >= 5)
                    {
                        resetConfirm = false;
                        resetConfirmCounter = 0;
                    }

                }
                else if(prestigeConfirm == true)
                {
                    prestigeConfirmCounter++;
                    if (prestigeConfirmCounter >= 5)
                    {
                        prestigeConfirm = false;
                        prestigeConfirmCounter = 0;
                    }
                }
            }
        }

        public double PrestigeMultCalculator(int upgradeLv, int amount, double prestigeCalc, double mult)
        {
            if (upgradeLv == 0)
            {
                prestigeCalc = (amount * mult);
            }
            else if (upgradeLv == 1)
            {
                prestigeCalc = (amount * mult) * 1.25;
            }
            else if (upgradeLv == 2)
            {
                prestigeCalc = (amount * mult) * 1.5;
            }
            else if (upgradeLv == 3)
            {
                prestigeCalc = (amount * mult) * 2;
            }

            return prestigeCalc;
        }

        //Main Buttons
        private void clickButton_Click(object sender, EventArgs e)
        {
            clicksInASec++;
            int amountToRemove = clicksInASec;
            userTokens = Math.Round((userTokens + 1 + extraClickAmount), 1); //Adds one token for the click. And then adds the extra clicks worth. Also adds the previous amount.
            userScore = Math.Round((userScore + 1 + extraClickAmount), 1); //Same as above, just for the score.
            RefreshScreen();
            if (clicksInASec > 20)
            {
                if (punishmentCounter == 0)
                {
                    MessageBox.Show("Cheating is wrong. Lets work together to prevent it. Ok? :)");
                    punishmentCounter++;
                }
                else
                {
                    MessageBox.Show("Cheating is still wrong. I warned you. You have been deducted " + ((1 + extraClickAmount) * amountToRemove).ToString() + " Tokens");
                    userTokens = Math.Round((userTokens - (1 + extraClickAmount) * amountToRemove), 1);
                    userScore = Math.Round((userScore - (1 + extraClickAmount) * amountToRemove), 1);

                    if(userTokens < 0)
                    {
                        userTokens = 0;
                    }

                    if(userScore < 0)
                    {
                        userScore = 0;
                    }
                    punishmentCounter++;
                }
            }
        }

        private void btnExtraClicks_Click(object sender, EventArgs e)
        {
            if (userTokens >= extraClickCost)
            {
                userTokens = userTokens - Convert.ToInt32(extraClickCost);
                extraClickAmount = extraClickAmount + 1;
                extraClickCost = Math.Round(extraClickCost * 1.35);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase an Extra Click!");
            }
        }

        private void btnUpgradeOne_Click(object sender, EventArgs e)
        {
            if(userTokens >= dullCost) //Enough to purchase?
            {
                userTokens = userTokens - Convert.ToInt32(dullCost); //Subtract cost
                dullAmount = dullAmount + 1; //Adds one to the amount
                dullCost = Math.Round(dullCost * 1.08);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase a Dull Click!");
            }
        }

        private void btnSharpClicks_Click(object sender, EventArgs e)
        {
            if (userTokens >= pointyCost) //Enough to purchase?
            {
                userTokens = userTokens - Convert.ToInt32(pointyCost); //Subtract cost
                pointyAmount = pointyAmount + 1; //Adds one to the amount
                pointyCost = Math.Round(pointyCost * 1.15);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase a Pointy Click!");
            }
        }

        private void btnPointier_Click(object sender, EventArgs e)
        {
            if (userTokens >= pointierCost) //Enough to purchase?
            {
                userTokens = userTokens - Convert.ToInt32(pointierCost); //Subtract cost
                pointierAmount = pointierAmount + 1; //Adds one to the amount
                pointierCost = Math.Round(pointierCost * 1.15);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase a Pointier Click!");
            }
        }

        private void btnSharp_Click(object sender, EventArgs e)
        {
            if (userTokens >= sharpCost) //Enough to purchase?
            {
                userTokens = userTokens - Convert.ToInt32(sharpCost); //Subtract cost
                sharpAmount = sharpAmount + 1; //Adds one to the amount
                sharpCost = Math.Round(sharpCost * 1.15);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase a Sharp Click!");
            }
        }

        private void btnSharper_Click(object sender, EventArgs e)
        {
            if (userTokens >= sharperCost) //Enough to purchase?
            {
                userTokens = userTokens - Convert.ToInt32(sharperCost); //Subtract cost
                sharperAmount = sharperAmount + 1; //Adds one to the amount
                sharperCost = Math.Round(sharperCost * 1.15);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase a Sharper Click!");
            }
        }

        private void btnSharpest_Click(object sender, EventArgs e)
        {
            if (userTokens >= sharpestCost) //Enough to purchase?
            {
                userTokens = userTokens - Convert.ToInt32(sharpestCost); //Subtract cost
                sharpestAmount = sharpestAmount + 1; //Adds one to the amount
                sharpestCost = Math.Round(sharpestCost * 1.15);
                RefreshScreen();
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens to purchase a Sharpest Click!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SaveGame();
            shutdown = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if(resetConfirm == true)
            {
                System.IO.File.Delete(path);
                shutdown = true;
                noSave = true;
            }
            else
            {
                MessageBox.Show("Are you sure you wish to reset the game? This will clear all data. If you are sure, press the Prestige button again within 5 seconds.");
                resetConfirm = true;
            }
        }

        private void btnPrestige_Click(object sender, EventArgs e)
        {
            if(userTokens >= 0)
            {
                double dullPrestigeCalc = 0;
                double pointyPrestigeCalc = 0;
                double pointierPrestigeCalc = 0;
                double sharpPrestigeCalc = 0;
                double sharperPrestigeCalc = 0;
                double sharpestPrestigeCalc = 0;

                dullPrestigeCalc = PrestigeMultCalculator(dullUpgradeLv, dullAmount, dullPrestigeCalc, 0.25);
                pointyPrestigeCalc = PrestigeMultCalculator(pointyUpgradeLv, pointyAmount, pointyPrestigeCalc, 0.50);
                pointierPrestigeCalc = PrestigeMultCalculator(pointierUpgradeLv, pointierAmount, pointierPrestigeCalc, 1);
                sharpPrestigeCalc = PrestigeMultCalculator(sharpUpgradeLv, sharpAmount, sharpPrestigeCalc, 2);
                sharperPrestigeCalc = PrestigeMultCalculator(sharperUpgradeLv, sharperAmount, sharperPrestigeCalc, 3);
                sharpestPrestigeCalc = PrestigeMultCalculator(sharpestUpgradeLv, sharpestAmount, sharpestPrestigeCalc, 5);

                if (prestigeConfirm == true)
                {
                    prestigeInProcess = true;

                    tokensPerSec = 0;

                    prestigeCalcNumberTotal = dullPrestigeCalc + pointyPrestigeCalc + pointierPrestigeCalc + sharpPrestigeCalc + sharperPrestigeCalc + sharpestPrestigeCalc;
                    prestigeMultipierDecimal = Math.Round((prestigeCalcNumberTotal / 1000),2) + prestigeMultipierDecimal;
                    //Reset the costs to the default amount
                    extraClickCost = 100;
                    dullCost = 15;
                    pointyCost = 100;
                    pointierCost = 1100;
                    sharpCost = 12000;
                    sharperCost = 130000;
                    sharpestCost = 1400000;

                    userTokens = 0;
                    extraClickAmount = 0;
                    dullAmount = 0;
                    pointyAmount = 0;
                    pointierAmount = 0;
                    sharpAmount = 0;
                    sharperAmount = 0;
                    sharpestAmount = 0;
                    prestigeLevel = prestigeLevel + 1;
                    timerSecPrestige = 0;

                    dullUpgradeLv = 0;
                    pointyUpgradeLv = 0;
                    pointierUpgradeLv = 0;
                    sharpUpgradeLv = 0;
                    sharperUpgradeLv = 0;
                    sharpestUpgradeLv = 0;

                    SaveGame();

                    prestigeCost = 1000000;
                    double pCostIncrease;
                    double i = 0;
                    while (prestigeLevel > i)
                    {
                        pCostIncrease = (((i + 3) * 2) / 100) + 1;
                        prestigeCost = (int)(Math.Round(prestigeCost * pCostIncrease));
                        i++;
                    }
                    i = 0;

                    VisibilityFalse();
                    RefreshScreen();
                    UpdateUpgrades();

                    prestigeInProcess = false;
                }
                else
                {
                    prestigeCalcNumberTotal = dullPrestigeCalc + pointyPrestigeCalc + pointierPrestigeCalc + sharpPrestigeCalc + sharperPrestigeCalc + sharpestPrestigeCalc;
                    if (prestigeCalcNumberTotal >= 100)
                    {
                        MessageBox.Show("Are you sure you wish to Prestige? This will reset all upgrades and tokens and give you a bonus of " + Math.Round((prestigeCalcNumberTotal / 1000),2).ToString() + ". If you are sure, press the Prestige button again within 5 seconds.");
                        prestigeConfirm = true;
                    }
                    else
                    {
                        MessageBox.Show("Your prestige bonus would be less then 10% if you prestiged now. Because of this, you cannot prestige until you purchase more clicks!");
                    }
                }
            }
            else
            {
                MessageBox.Show("You do not have enough Tokens To Prestige!");
            }
        }

        //Upgrade Buttons

        private void btnDullUpgradeOne_Click(object sender, EventArgs e)
        {
            if(!(dullUpgradeLv >= 1))
            {
                if (userTokens >= 100000)
                {
                    userTokens = userTokens - 100000;
                    dullUpgradeLv = 1;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Dull Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnDullUpgradeTwo_Click(object sender, EventArgs e)
        {
            if(!(dullUpgradeLv >= 2))
            {
                if (userTokens >= 250000)
                {
                    userTokens = userTokens - 250000;
                    dullUpgradeLv = 2;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Dull Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnDullUpgradeThree_Click(object sender, EventArgs e)
        {
            if(!(dullUpgradeLv >= 3))
            {
                if (userTokens >= 500000)
                {
                    userTokens = userTokens - 500000;
                    dullUpgradeLv = 3;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Dull Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnPointyUpgradeOne_Click(object sender, EventArgs e)
        {
            if (!(pointyUpgradeLv >= 1))
            {
                if (userTokens >= 250000)
                {
                    userTokens = userTokens - 250000;
                    pointyUpgradeLv = 1;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Pointy Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnPointyUpgradeTwo_Click(object sender, EventArgs e)
        {
            if (!(pointyUpgradeLv >= 2))
            {
                if (userTokens >= 500000)
                {
                    userTokens = userTokens - 500000;
                    pointyUpgradeLv = 2;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Pointy Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnPointyUpgradeThree_Click(object sender, EventArgs e)
        {
            if (!(pointyUpgradeLv >= 3))
            {
                if (userTokens >= 1000000)
                {
                    userTokens = userTokens - 1000000;
                    pointyUpgradeLv = 3;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Pointy Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnPointierUpgradeOne_Click(object sender, EventArgs e)
        {
            if (!(pointierUpgradeLv >= 1))
            {
                if (userTokens >= 1000000)
                {
                    userTokens = userTokens - 1000000;
                    pointierUpgradeLv = 1;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Pointy Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnPointierUpgradeTwo_Click(object sender, EventArgs e)
        {
            if (!(pointierUpgradeLv >= 2))
            {
                if (userTokens >= 2000000)
                {
                    userTokens = userTokens - 2000000;
                    pointierUpgradeLv = 2;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Pointy Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnPointierUpgradeThree_Click(object sender, EventArgs e)
        {
            if (!(pointierUpgradeLv >= 3))
            {
                if (userTokens >= 3500000)
                {
                    userTokens = userTokens - 3500000;
                    pointierUpgradeLv = 3;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Pointy Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharpUpgradeOne_Click(object sender, EventArgs e)
        {
            if (!(sharpUpgradeLv >= 1))
            {
                if (userTokens >= 5000000)
                {
                    userTokens = userTokens - 5000000;
                    sharpUpgradeLv = 1;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharp Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharpUpgradeTwo_Click(object sender, EventArgs e)
        {
            if (!(sharpUpgradeLv >= 2))
            {
                if (userTokens >= 7500000)
                {
                    userTokens = userTokens - 7500000;
                    sharpUpgradeLv = 2;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharp Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharpUpgradeThree_Click(object sender, EventArgs e)
        {
            if (!(sharpUpgradeLv >= 3))
            {
                if (userTokens >= 10000000)
                {
                    userTokens = userTokens - 10000000;
                    sharpUpgradeLv = 3;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharp Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharperUpgradeOne_Click(object sender, EventArgs e)
        {
            if (!(sharperUpgradeLv >= 1))
            {
                if (userTokens >= 15000000)
                {
                    userTokens = userTokens - 15000000;
                    sharperUpgradeLv = 1;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharper Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharperUpgradeTwo_Click(object sender, EventArgs e)
        {
            if (!(sharperUpgradeLv >= 2))
            {
                if (userTokens >= 17500000)
                {
                    userTokens = userTokens - 17500000;
                    sharperUpgradeLv = 2;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharper Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharperUpgradeThree_Click(object sender, EventArgs e)
        {
            if (!(sharperUpgradeLv >= 3))
            {
                if (userTokens >= 20000000)
                {
                    userTokens = userTokens - 20000000;
                    sharperUpgradeLv = 3;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharper Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharpestUpgradeOne_Click(object sender, EventArgs e)
        {
            if (!(sharpestUpgradeLv >= 1))
            {
                if (userTokens >= 50000000)
                {
                    userTokens = userTokens - 50000000;
                    sharpestUpgradeLv = 1;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharpest Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharpestUpgradeTwo_Click(object sender, EventArgs e)
        {
            if (!(sharpestUpgradeLv >= 2))
            {
                if (userTokens >= 100000000)
                {
                    userTokens = userTokens - 100000000;
                    sharpestUpgradeLv = 2;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharpest Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }

        private void btnSharpestUpgradeThree_Click(object sender, EventArgs e)
        {
            if (!(sharpestUpgradeLv >= 3))
            {
                if (userTokens >= 250000000)
                {
                    userTokens = userTokens - 250000000;
                    sharpestUpgradeLv = 3;
                    UpdateUpgrades();
                    RefreshScreen();
                }
                else
                {
                    MessageBox.Show("You do not have enough Tokens to upgrade your Sharpest Clicks!");
                }
            }
            else
            {
                MessageBox.Show("You already own this upgrade! Since this is a one time upgrade you cannot purchase it again!");
            }
        }
    }
}
