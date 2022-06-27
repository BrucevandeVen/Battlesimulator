using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BattleSimulator
{
    public partial class BattleSimulatorForm : Form
    {
        // Fields
        private Speler speler1; // Create left player
        private Speler speler2; // Create Right player

        private Random random; // Create Random
        private int randomSide;

        private Label roundCounter;
        private int roundCount = 1;

        private bool lButtonClick;
        private bool rButtonClick;

        // ctor
        public BattleSimulatorForm()
        {
            InitializeComponent();
        }

        // EventHandlers
        private void BattleSimulatorForm_Load(object sender, EventArgs e)
        {
            RulesButton();
            LPBEventhandlerCreator();
            RPBEventhandlerCreator();
            UIHandler();
        }

        // Left Player
        private void LAttackButton_Click(object sender, EventArgs e)
        {
            lButtonClick = true;

            int damage1 = speler1.DealDamage(); // Create damage int
            speler2.TakeDamage(damage1);

            RProgressBar.Value = speler2.Hitpoints;
            RHealth.Text = Convert.ToString(speler2.Hitpoints);

            speler2.GainArmor(damage1);
            RArmorInt.Text = Convert.ToString(speler2.Armor);

            BattleDialogText(speler1.Naam + " did " + Convert.ToString(damage1) + " damage");

            AttackPictureBox(damage1);

            TurnCount(randomSide);

            LAttackButton.Visible = false; // It's the other players turn
            RAttackButton.Visible = true;
            LPotionButton.Visible = false;
            RPotionButton.Visible = true;

            if (speler2.Hitpoints == 0)
            {
                EndGame(speler2, damage1, RPicture);
            }
            if (Convert.ToInt32(RArmorInt.Text) == 0)
            {
                RArmorPB.Visible = false;
                RArmorInt.Visible = false;
            }
            else
            {
                RArmorPB.Visible = true;
                RArmorInt.Visible = true;
            }
        }

        private void LPotionButton_Click(object sender, EventArgs e)
        {
            lButtonClick = true;

            int heal = speler1.DrinkPotion(); // Create heal int
            speler1.Heal(heal);

            LProgressBar.Value = speler1.Hitpoints;
            LHealth.Text = Convert.ToString(speler1.Hitpoints);

            BattleDialogText(speler1.Naam + " healed with " + Convert.ToString(heal) + " points");

            TurnCount(randomSide);

            LAttackButton.Visible = false;
            RAttackButton.Visible = true;
            LPotionButton.Visible = false;
            RPotionButton.Visible = true;
            AttackPB.Visible = false; // there was no damage done, so this is irrelevant
        }

        private void LPictureBoxClick(object sender, EventArgs e)
        {
            LPicture.Image = ((PictureBox)sender).Image; // Left clicked image = LPicture image (is still invisible at this moment)
            Picture1.Visible = false; // Disables all small picture boxes
            Picture2.Visible = false;
            Picture3.Visible = false;
            Picture4.Visible = false;

            LPBClick.Image = ((PictureBox)sender).Image; // Set clicked image to LPBClick PictureBox
            LPBClick.Visible = true;
        }

        // Right Player
        private void RAttackButton_Click(object sender, EventArgs e)
        {
            rButtonClick = true;

            int damage2 = speler2.DealDamage();
            speler1.TakeDamage(damage2);

            LProgressBar.Value = speler1.Hitpoints;
            LHealth.Text = Convert.ToString(speler1.Hitpoints);

            speler1.GainArmor(damage2);
            LArmorInt.Text = Convert.ToString(speler1.Armor);

            BattleDialogText(speler2.Naam + " did " + Convert.ToString(damage2) + " damage");

            AttackPictureBox(damage2);

            TurnCount(randomSide);

            LAttackButton.Visible = true;
            RAttackButton.Visible = false;
            LPotionButton.Visible = true;
            RPotionButton.Visible = false;

            if (speler1.Hitpoints == 0)
            {
                EndGame(speler1, damage2, LPicture);
            }
            if (Convert.ToInt32(LArmorInt.Text) == 0)
            {
                LArmorPB.Visible = false;
                LArmorInt.Visible = false;
            }
            else
            {
                LArmorPB.Visible = true;
                LArmorInt.Visible = true;
            }
        }

        private void RPotionButton_Click(object sender, EventArgs e)
        {
            rButtonClick = true;

            int heal = speler2.DrinkPotion();
            speler2.Heal(heal);

            RProgressBar.Value = speler2.Hitpoints;
            RHealth.Text = Convert.ToString(speler2.Hitpoints);

            BattleDialogText(speler2.Naam + " healed with " + Convert.ToString(heal) + " points");

            TurnCount(randomSide);

            LAttackButton.Visible = true;
            RAttackButton.Visible = false;
            LPotionButton.Visible = true;
            RPotionButton.Visible = false;
            AttackPB.Visible = false;
        }

        private void RPictureBoxClick(object sender, EventArgs e)
        {
            RPicture.Image = ((PictureBox)sender).Image;
            Picture5.Visible = false;
            Picture6.Visible = false;
            Picture7.Visible = false;
            Picture8.Visible = false;

            RPBClick.Image = ((PictureBox)sender).Image;
            RPBClick.Visible = true;
        }

        // Other Buttons
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (WhiteSpaceDetected(LTextBox.Text) == false && LTextBox.Text != "") // Check if textbox is not empty or only contains " "
            {
                speler1 = new Speler(100, LTextBox.Text); 
            }
            else
            {
                speler1 = new Speler(100);
            }

            if (WhiteSpaceDetected(RTextBox.Text) == false && RTextBox.Text != "")
            {
                speler2 = new Speler(100, RTextBox.Text);
            }
            else
            {
                speler2 = new Speler(100);
            }

            RandomPlayer(RandomSide());
            DesignControl();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void RulesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ATTACK:"
                + Environment.NewLine +
                "Random Damage between 0 - 30"
                + Environment.NewLine +
                Environment.NewLine +
                "POTION:"
                + Environment.NewLine +
                "Random Healing Power between 10 - 20"
                + Environment.NewLine +
                Environment.NewLine +
                "ARMOR:"
                + Environment.NewLine +
                "5 - Damage taken between 20 - 25"
                + Environment.NewLine +
                "10 - Damage taken <= 10 & Health <= 30"
                + Environment.NewLine +
                "13 - Health = 13 or Damage taken = 13"
                + Environment.NewLine +
                "50 - Health = 1 & Armor = 0"
                + Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "Bruce van de Ven", "Rules");
        }

        private void HighScoreButton_Click(object sender, EventArgs e)
        {
            if (speler1.Hitpoints == 0)
            {
                MessageBox.Show(HighscoreReaderWriter(speler2), "Highscore");
            }
            if (speler2.Hitpoints == 0)
            {
                MessageBox.Show(HighscoreReaderWriter(speler1), "Highscore");
            }
        }

        // methodes
        private void DesignControl() // Initiate when StartButton is clicked, this creates the battle scene (and makes start/end screen invisible)
        {
            StartButton.Visible = false;

            LNaamLabel.Visible = true;
            RNaamLabel.Visible = true;
            LNaamLabel.Text = speler1.Naam;
            RNaamLabel.Text = speler2.Naam;

            LProgressBar.Visible = true;
            RProgressBar.Visible = true;
            LProgressBar.Value = 100;
            RProgressBar.Value = 100;

            LTextBox.Visible = false;
            RTextBox.Visible = false;

            LPicture.Visible = true;
            RPicture.Visible = true;

            BattleDialog1.Text = "";
            BattleDialog1.Visible = true;
            BattleDialog2.Text = "";
            BattleDialog2.Visible = true;
            BDL.Visible = true;

            Player1PB.Visible = false;
            Player2PB.Visible = false;
            ChoosePlayerPB.Visible = false;

            LHealth.Text = Convert.ToString(speler1.Hitpoints);
            RHealth.Text = Convert.ToString(speler2.Hitpoints);
            LHealth.Visible = true;
            RHealth.Visible = true;

            Picture1.Visible = false;
            Picture2.Visible = false;
            Picture3.Visible = false;
            Picture4.Visible = false;
            Picture5.Visible = false;
            Picture6.Visible = false;
            Picture7.Visible = false;
            Picture8.Visible = false;

            LPBClick.Visible = false;
            RPBClick.Visible = false;

            TransparentLabelOnPictureBox(LArmorPB, LArmorInt);
            TransparentLabelOnPictureBox(RArmorPB, RArmorInt);

            Size size = new Size(100, 100);
            VSPB.Size = size;
            VSPB.Location = new Point(240, 60);
            VSPB.Visible = true;

            CreateRoundCounterLabel();
        }

        private void RulesButton() // Create RulesButton (UpLeft Coner)
        {
            PictureBox rulesButton = new PictureBox();

            rulesButton.Location = new Point(9, 9);
            rulesButton.AutoSize = true;
            rulesButton.SizeMode = PictureBoxSizeMode.Zoom;
            rulesButton.BackColor = Color.Transparent;
            rulesButton.Image = Properties.Resources.rules_icon;
            rulesButton.Size = new Size(50, 48);
            rulesButton.Cursor = Cursors.Hand;

            this.Controls.Add(rulesButton);

            rulesButton.Click += new EventHandler(this.RulesButton_Click);
        }

        private bool WhiteSpaceDetected(string text) // Check if Textbox does not only contain whitespaces
        {
            foreach (char c in text) // Check per charachter in text
            {
                if (char.IsWhiteSpace(c) == false)  // Check if charachter is " "
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void AttackPictureBox(int damage) // Critical Hit, Perfect, Too Bad PictureBoxes
        {
            AttackPB.Visible = false;

            if (damage >= 25 && damage < 30)
            {
                AttackPB.Visible = true;
                AttackPB.Image = Properties.Resources.CriticalHit; // Sets image
            }
            if (damage == 0)
            {
                AttackPB.Visible = true;
                AttackPB.Image = Properties.Resources.TooBadCropped;
            }
            if (damage == 30)
            {
                AttackPB.Visible = true;
                AttackPB.Image = Properties.Resources.Perfect;
            }
        }

        private int RandomSide()
        {
            random = new Random();
            randomSide = random.Next(0, 2);
            return randomSide; // returns 0 or 1
        }

        private void RandomPlayer(int random)
        {
            if (random == 0) // Player 1 turn
            {
                LAttackButton.Visible = true;
                LPotionButton.Visible = true;
            }
            else if (random == 1) // Player 2 turn
            {
                RAttackButton.Visible = true;
                RPotionButton.Visible = true;
            }
        }

        private void TurnCount(int random)
        {
            if (random == 0) // If Player 1 starts, roundcounter is only updated when Player 2 did something
            {
                if (rButtonClick == true) 
                {
                    UpdateRoundCounter();
                }
                rButtonClick = false; // Makes sure to not repeat if statement after 1 click
            }
            else if (random == 1) // If Player 2 starts, roundcounter is only updated when Player 1 did something
            {
                if (lButtonClick == true)
                {
                    UpdateRoundCounter();
                }
                lButtonClick = false; // Makes sure to not repeat if statement after 1 click
            }
        }

        private void TransparentLabelOnPictureBox(PictureBox pictureBox, Label label) // Sets Label transparent with a PictureBox as background
        {
            var lPos = this.PointToScreen(label.Location);
            lPos = pictureBox.PointToClient(lPos);
            label.Parent = pictureBox;
            label.Location = lPos;
            label.BackColor = Color.Transparent;
        }

        private void LPBEventhandlerCreator() // When picture is clicked, an eventhandler is created
        {
            Picture1.Click += LPictureBoxClick;
            Picture2.Click += LPictureBoxClick;
            Picture3.Click += LPictureBoxClick;
            Picture4.Click += LPictureBoxClick;
        }

        private void RPBEventhandlerCreator() // When picture is clicked, an eventhandler is created
        {
            Picture5.Click += RPictureBoxClick;
            Picture6.Click += RPictureBoxClick;
            Picture7.Click += RPictureBoxClick;
            Picture8.Click += RPictureBoxClick;
        }

        private void TextboxMaxLength(int length) // Set textbox lengths
        {
            LTextBox.MaxLength = length;
            RTextBox.MaxLength = length;
        }

        private void UIHandler()
        {
            TextboxMaxLength(10); // Set max textbox text length, so it won't fall off the screen when too long
            MaximizeBox = false; // Disable fullscreen option
            FormBorderStyle = FormBorderStyle.FixedDialog; // Fix Form to be in the center of the screen
        }

        private void EndGame(Speler dodeSpeler, int finalHit, PictureBox pictureBox) // Initiate after finalHit
        {
            BattleDialog1.Text = dodeSpeler.Naam + " is DEAD! " + finalHit + " damage done";
            pictureBox.Image = Properties.Resources.Death; // Change deadplayer to dead picture
            
            LAttackButton.Visible = false; // Change UI for end screen
            RAttackButton.Visible = false;
            LPotionButton.Visible = false;
            RPotionButton.Visible = false;
            AttackPB.Visible = false;
            RProgressBar.Visible = false;
            LProgressBar.Visible = false;
            RHealth.Visible = false;
            LHealth.Visible = false;
            LArmorPB.Visible = false;
            RArmorPB.Visible = false;
            VSPB.Visible = false;

            RAttackButton.Enabled = false; // Glitches out sometimes if not disabled
            RPotionButton.Enabled = false;
            LAttackButton.Enabled = false;
            LPotionButton.Enabled = false;

            Size sizePicture = new Size(200, 200); // Set new size for pictures in end screen
            LPicture.Size = sizePicture;
            RPicture.Size = sizePicture;

            LPicture.Location = new Point(30, 120); // Set new Locations for Pictures & Labels to fit screen nicely
            LNaamLabel.Location = new Point(20, 70);
            RPicture.Location = new Point(350, 120);
            RNaamLabel.Location = new Point(340, 70);

            LNaamLabel.Font = new Font("Impact", 30, FontStyle.Italic); // Only fontsize is changed here, to fit with the theme
            RNaamLabel.Font = new Font("Impact", 30, FontStyle.Italic);

            roundCounter.Location = new Point(300, 15); // Set new Turncount location

            HighScoreButton(); // create HighscoreButton
        }

        private void HighScoreButton() // Create highscore button
        {
            PictureBox highScoreButton = new PictureBox(); // Create new PictureBox

            highScoreButton.Location = new Point(50, 5); // Set Location
            highScoreButton.AutoSize = true; 
            highScoreButton.SizeMode = PictureBoxSizeMode.Zoom; // Make Image fit to Box (without stretching)
            highScoreButton.BackColor = Color.Transparent;
            highScoreButton.Image = Properties.Resources.Highscore; // Get Image from Resources
            highScoreButton.Size = new Size(275, 80); 
            highScoreButton.Cursor = Cursors.Hand; // Changes mouse cursor to hand when hovering over the button

            this.Controls.Add(highScoreButton); // Add to Controls
            highScoreButton.Click += new EventHandler(this.HighScoreButton_Click); // Create eventhandler when picturebox is clicked
        }

        private void BattleDialogText(string text)
        {
            if (BattleDialog1.Text.Contains(" did ") || BattleDialog1.Text.Contains(" points")) // if battelog contains something
            {
                BattleDialog2.Text = BattleDialog1.Text; // set battlelog 1 text to battlelog 2 text (creates scrolling effect)
                BattleDialog1.Text = text; // set new line, for now empty, batllelog 1 text
            }
            else // if there is no battlelog text yet
            {
                BattleDialog1.Text = text; 
            }
        }

        private void CreateRoundCounterLabel() // Create turncount label in code
        {
            roundCounter = new Label(); // create new label
            roundCounter.Location = new Point(225, 15); // set location
            roundCounter.Font = new Font("Arial", 30); // set font & font size
            roundCounter.AutoSize = true; // Autosize to font size
            roundCounter.Text = "Turn: " + roundCount; // set text
            roundCounter.ForeColor = Color.White; // color
            roundCounter.BackColor = Color.Transparent; // background color transparent, so it blends in with background
            this.Controls.Add(roundCounter); // add label to controls
        }

        private void UpdateRoundCounter() // Place after each attack or heal, will update turncount +1
        {
            roundCount++; //roundCount +1
            roundCounter.Text = "Turn: " + roundCount;
        }

        private string HighscoreReaderWriter(Speler NewWinner) // Create new or update existing highscore files
        {
            string highScoreFilePath = @"D:\Highscore.txt";
            FileInfo sI = new FileInfo(highScoreFilePath);

            int newHighScore = roundCount; // This games score

            if (sI.Exists && sI.Length > 0) // Check if files exist/empty
            {
                StreamReader streamReaderScore = new StreamReader(highScoreFilePath); // Read last highscore
                string text = streamReaderScore.ReadLine(); // String all text inside file
                string[] oldScoreWinner = text.Split(' '); // Put character(s) without ' ' in array, first string in array = [0], second string = [1]
                int oldHighScore = int.Parse(oldScoreWinner[0]); // int old score, always first in array
                string oldWinner = Convert.ToString(oldScoreWinner[1]); // string old winner name, always second in array
                streamReaderScore.Close(); // if not closed, code will not continue

                if (newHighScore < oldHighScore) // Check if new score < last score
                {
                    StreamWriter streamWriterScore = new StreamWriter(highScoreFilePath); // Write new Highscore
                    streamWriterScore.Write(newHighScore + " " + NewWinner.Naam); // Space is necessery for splitting score and winner
                    streamWriterScore.Close();

                    return NewWinner.Naam + ": " + newHighScore.ToString(); // Return new Highscore
                }
                else // New highscore was not better then old one
                {
                    return oldWinner + ": " + oldHighScore.ToString(); // Return old Highscore
                }
            }
            else // If file does not exist, create new file with new score
            {
                StreamWriter streamWriterScore = new StreamWriter(highScoreFilePath);
                streamWriterScore.Write(newHighScore + " " + NewWinner.Naam);
                streamWriterScore.Close();

                return NewWinner.Naam + ": " + newHighScore.ToString(); // Return current score
            }
        }
    }
}