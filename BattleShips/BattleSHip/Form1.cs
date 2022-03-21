using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleSHip
{
    public partial class Form1 : Form
    {
        PictureBox temp = new PictureBox();
        Animations anim = new Animations();
        public const int mapsize = 11;
        public const int boxsize = 40;
        public string alpha = "ABCDEFGHIJ";
        Dictionary<int, int> countShips = new Dictionary<int, int>();
        public int[,] map = new int[mapsize, mapsize];
        public int[,] botmap = new int[mapsize, mapsize];
        public Button[,] mybuttons = new Button[mapsize, mapsize];
        public Button[,] botbuttons = new Button[mapsize, mapsize];
        public static bool enabled = false;
        public ComboBox Kind_of_ship = new ComboBox();
        public ComboBox Position = new ComboBox();
        public ComboBox Choose_dif = new ComboBox();
        public TextBox text = new TextBox();
        private Player player;
        private Bot bot;
        public Form1()
        {
            InitializeComponent();
            this.Text = "BATTLESHIP";
            Init();
        }
        public void Init()
        {
            enabled = false;
            CreateForm();
            player = new Player(ref map, ref botmap, ref mybuttons, ref botbuttons);
            bot = new Bot(ref map, ref botmap, ref mybuttons, ref botbuttons, ref player);
            bot.Trigger_arrangement();
        }
        public void CreateForm()
        {
            temp.Location = new Point(400, mapsize * boxsize + 100);
            temp.Size = new System.Drawing.Size(30, 30);
            temp.BackColor = Color.Black;
            this.Controls.Add(temp);


            this.Width = mapsize * 2 * boxsize + 150;
            this.Height = mapsize * boxsize + 300;
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    map[i, j] = 0;
                    Button button = new Button();
                    button.Location = new Point(j * boxsize, i * boxsize);
                    button.Size = new Size(boxsize, boxsize);
                    if (i == 0 || j == 0)
                    {
                        button.BackColor = Color.Purple;
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                        if (i == 0 && j > 0)
                            button.Text = alpha[j - 1].ToString();
                    }
                    else
                    {
                        mybuttons[i, j] = button;
                        button.Click += new EventHandler(Print_My_Ships);
                    }
                    this.Controls.Add(button);
                }
            }
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    botmap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(500 + j * boxsize, i * boxsize);
                    button.Size = new Size(boxsize, boxsize);
                    if (i == 0 || j == 0)
                    {
                        button.BackColor = Color.Purple;
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                        if (i == 0 && j > 0)
                            button.Text = alpha[j - 1].ToString();
                    }
                    else
                    {
                        botbuttons[i, j] = button;
                        button.Click += new EventHandler(Change_of_queue);
                    }
                    this.Controls.Add(button);
                }
            }
            string[] Dif = { "Легко", "Сложно" };
            Choose_dif.Font = new Font("Consolas", 11, FontStyle.Regular);
            Choose_dif.Location = new Point(700, mapsize * boxsize + 145);
            for (int i = 0; i < Dif.Length; i++)
                Choose_dif.Items.Add(Dif[i]);
            Choose_dif.SelectedItem = "Легко";
            Choose_dif.ItemHeight = 70;
            Position.Width = 140;
            this.Controls.Add(Choose_dif);

            string[] len = { "4", "3", "2", "1" };
            Kind_of_ship.Font = new Font("Consolas", 11, FontStyle.Regular);
            Kind_of_ship.Location = new Point(100, mapsize * boxsize + 165);
            for (int i = 0; i < len.Length; i++)
                Kind_of_ship.Items.Add(len[i]);
            Kind_of_ship.SelectedItem = "4";
            Kind_of_ship.ItemHeight = 70;
            Kind_of_ship.Width = 140;
            this.Controls.Add(Kind_of_ship);

            string[] Pos = { "Вертикально", "Горизонтально" };
            Position.Font = new Font("Consolas", 11, FontStyle.Regular);
            Position.Location = new Point(250, mapsize * boxsize + 165);
            for (int i = 0; i < Pos.Length; i++)
                Position.Items.Add(Pos[i]);
            Position.SelectedItem = "Горизонтально";
            Position.ItemHeight = 70;
            Position.Width = 140;
            this.Controls.Add(Position);

            Label txt = new Label();
            txt.Font = new Font("Consolas", 13, FontStyle.Regular);
            txt.Text = "Расставьтсвои корабли";
            txt.Location = new Point(mapsize * boxsize / 2 - 150, mapsize * boxsize + 100);
            this.Controls.Add(txt);

            Label map1 = new Label();
            map1.Font = new Font("Consolas", 11, FontStyle.Regular);
            map1.Text = "Твоя карта";
            map1.Location = new Point(mapsize * boxsize / 2 - 50, mapsize * boxsize + 20);
            this.Controls.Add(map1);

            Label map2 = new Label();
            map2.Font = new Font("Consolas", 11, FontStyle.Regular);
            map2.Text = "Карта противника";
            map2.Location = new Point(400 + mapsize * boxsize / 2, mapsize * boxsize + 20);
            this.Controls.Add(map2);

            Button start = new Button();
            start.Font = new Font("Consolas", 13, FontStyle.Regular);
            start.Text = "Начать";
            start.Height = 50;
            start.Width = 100;
            start.Click += new EventHandler(CheckBegin);
            start.Location = new Point(402, mapsize * boxsize + 20);
            this.Controls.Add(start);
        }
        public void CheckBegin(object sender, EventArgs e)
        {
            int all;

            all = 0;
            foreach (int sum in player.Num_of_ships.Values)
                all += sum;
            if (all == 0)
                enabled = true;
            else
            {
                enabled = false;
                MessageBox.Show("Расставьте все свои корабли");
            }
        }
        public void Print_My_Ships(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int box1 = Convert.ToInt32(Kind_of_ship.SelectedItem);
            string box2 = Convert.ToString(Position.SelectedItem);
            if (enabled != false)
            {
                if (box1 == 0 || box2 == "")
                {
                    if (box1 == 0)
                        MessageBox.Show("Выберите длину корабля");
                    if (box1 == 0 && box2 == "")
                        MessageBox.Show("Выберите корабль который хотите поставить");
                    if (box2 == "")
                        MessageBox.Show("Выберите как должен стоять корабль");
                }
            }
            else
                player.The_main_arrangement(box2 == "Горизонтально", box1, button);
        }
        public bool GameContinion()
        {
            if (bot.Getter_of_alives() == 0)
            {
                MessageBox.Show("Вы победили");
                enabled = false;
            }
            if (player.Getter_of_alives() == 0)
            {
                MessageBox.Show("Победил бот");
                enabled = false;
            }
            return enabled;
        }
        public void Change_of_queue(object sender, EventArgs e)
        {
            string chosenBot = Convert.ToString(Choose_dif.SelectedItem);
            Button pressedButton = sender as Button;
            bool playerTurn = Shoot(pressedButton);

            if (GameContinion())
            {
                if (!playerTurn)
                {
                    if (chosenBot == "Легко")
                        bot.BotShoot(false);
                    if (chosenBot == "Сложно")
                        bot.BotShoot(true);
                }
            }
        }
        private async void Print(Button pressedButton)
        {
            foreach (Image img in anim.frame.ToList())
            {
                pressedButton.Image = img;
                await Task.Delay(100);
            }
        }
        public bool Shoot(Button pressedbutton)
        {
            anim.GettingImages();
            bool goal = false;
            if (enabled == true)
            {
                int x = ((pressedbutton.Location.X - 500) / boxsize);
                int y = pressedbutton.Location.Y / boxsize;
                if (botmap[y, x] == 1)
                {
                    Print(botbuttons[y, x]);
                    bot.Getter_of_Hit(y, x);
                    botmap[y, x] = 2;
                    botbuttons[y, x].Enabled = false;
                    goal = true;
                }
                if (botmap[y, x] != 1 && botmap[y, x] != -2 && botmap[y, x] != 2)
                {
                    pressedbutton.BackColor = Color.Aquamarine;
                    botmap[y, x] = -2;
                    botbuttons[y, x].Enabled = false;
                    goal = false;
                }
            }
            else
                MessageBox.Show("Начните игру");
            if (goal)
            {
                bool checker = bot.Check_all_ships();
                if (!checker)
                {
                    if (bot.Getter_of_alives() <= 9)
                    {
                        Print_Deads();
                    }
                }
            }
            if (!goal)
            {
                bool checker = player.Check_all_ships();
                if (!checker)
                {
                    if (player.Getter_of_alives() == 9)
                        bot.Print_Deads();
                    else if (player.Getter_of_alives() < 9)
                    {
                        bot.Print_Deads();
                    }
                }
            }
            return (goal);
        }
        private void Print_Deads()
        {
            List<List<TPoint>> list = bot.Dead_list();
            //MessageBox.Show("AHAAHA deadlist");
            foreach (List<TPoint> l in list)
            {
                TPoint first = l[0];
                TPoint last = l[l.Count - 1];
                first = new TPoint(new Point(first.point.X - 1, first.point.Y - 1));
                last = new TPoint(new Point(last.point.X + 1, last.point.Y + 1));
                for (int i = first.point.Y; i <= last.point.Y; i++)
                {
                    for (int j = first.point.X; j <= last.point.X; j++)
                    {
                        if (i > 0 && j > 0 && i < 11 && j < 11 && botmap[i, j] != 2)
                        {
                            botbuttons[i, j].BackColor = Color.Aquamarine;
                            botbuttons[i, j].Enabled = false;
                        }
                    }
                }
            }
        }
    }
}