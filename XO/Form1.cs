using System;
using System.Threading;
using System.Windows.Forms;

namespace XO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Game game;
        Game.State turn;
        Bot bot1;
        Bot bot2;
        bool play = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Game();
            bot1 = new Bot(Game.State.X);
            bot2 = new Bot(Game.State.O);
            turn = Game.State.X;
            for (int i = 0; i < 1999999; i++)
			{
			    BotMove(bot1, bot2);
                BotMove(bot2, bot1);
			}
            play = false;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Play(int x, int y, object sender)
        {
            XO.Game.State s = game.play(x, y, turn);
            if (s == 0)
            {
                MessageBox.Show("Try Again!");
            }else {
                MarkLabel(x, y);
                if (s == Game.State.Win)
                {
                    MessageBox.Show("I Win!");
                    bot2.FeedBack(0);
                    ResetGame();
                }
                else if (s == Game.State.Draw)
                {
                    MessageBox.Show("Draw!");
                    bot2.FeedBack(s);
                    ResetGame();
                }
                else
                {
                    NextTrun();
                    
                    BotMove(bot2, bot1);
                }
            }
        }

        private void BotMove(Bot bot, Bot Other)
        {
            int[] move = bot.Play(game);
            XO.Game.State s = game.play(move[0], move[1], turn);
            if (s == 0)
            {
                MessageBox.Show("Try Again!");
            }
            else
            {
                MarkLabel(move[0], move[1]);
                if (s == Game.State.Win)
                {
                    Console.WriteLine(bot.ToString() +" Win!");
                    bot.FeedBack(s);
                    if (play)
                        Other.FeedBack(0);
                    ResetGame();
                }
                else if (s == Game.State.Draw)
                {
                    Console.WriteLine("Draw!");
                    bot.FeedBack(s);
                    if(play)
                        Other.FeedBack(s);
                    ResetGame();
                }
                NextTrun();
            }
        }

        private void MarkLabel(int x, int y)
        {
            Label lbl = (Label)tableLayoutPanel1.GetControlFromPosition(x, y);
            lbl.Text = turn.ToString();
        }

        private void ResetGame()
        {
            game = new Game();
            ClearLabel(this);
        }

        public void ClearLabel(Control control)
        {
            if (control is Label)
            {
                Label lbl = (Label)control;
                lbl.Text = String.Empty;

            }
            else
                foreach (Control child in control.Controls)
                {
                    ClearLabel(child);
                }

        }

        private void NextTrun()
        {
            if (turn == Game.State.X)
            {
                turn = Game.State.O;
            }
            else
            {
                turn = Game.State.X;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Play(0, 0, sender);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Play(1, 0, sender);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Play(2, 0, sender);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Play(0, 1, sender);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Play(1, 1, sender);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Play(2, 1, sender);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Play(0, 2, sender);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Play(1, 2, sender);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Play(2, 2, sender);
        }
    }
}
