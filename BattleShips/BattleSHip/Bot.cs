using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleSHip
{
    class Bot
    {
        public int[,] map = new int[Form1.mapsize, Form1.mapsize];
        public int[,] botmap = new int[Form1.mapsize, Form1.mapsize];
        private Random rand = new Random();
        Animations anim = new Animations();
        Player player;
        private Random rdn = new Random();
        private List<Ship> ships = new List<Ship>();
        public Button[,] mybuttons = new Button[Form1.mapsize, Form1.mapsize];
        public Button[,] botbuttons = new Button[Form1.mapsize, Form1.mapsize];
        Dictionary<int, int> Num_of_ships = new Dictionary<int, int>();
        private int counter;
        public Bot(ref int[,] map, ref int[,] botmap, ref Button[,] mybuttons, ref Button[,] botbuttons, ref Player player)
        {
            this.map = map;
            this.botmap = botmap;
            this.botbuttons = botbuttons;
            this.mybuttons = mybuttons;
            Num_of_ships.Add(4, 1);
            Num_of_ships.Add(3, 2);
            Num_of_ships.Add(2, 3);
            Num_of_ships.Add(1, 4);
            counter = 10;
            this.player = player;
        }
        public void The_main_arrangement()
        {
            int pos = 4;

            while (pos > 0)
            {
                int dx = 0;
                int dy = 0;
                if (Num_of_ships[pos] > 0)
                {
                    List<Point> ship = new List<Point>();
                    bool h = isHorizontal();
                    Point point = randomizer(h, pos);
                    for (int k = 0; k < pos; k++)
                    {
                        int x = point.X + dx;
                        int y = point.Y + dy;
                        ship.Add(new Point(x, y));
                        if (h)
                            dx++;
                        else
                            dy++;
                    }
                    if (check_around(ship))
                    {
                        Place_the_ship(ship);
                        Num_of_ships[pos]--;
                        //MessageBox.Show("yatut");
                    }
                }
                else
                    pos--;
            }
        }
        public bool check_around(List<Point> temp)
        {
            bool check = true;

            foreach (Point p in temp)
                check = check && !(map[p.Y, p.X] == 1 || map[p.Y, p.X] == -1);
            return check;
        }

        public bool isHorizontal()
        {
            if (rand.Next(0, 2) == 0)
                return true;
            else
                return false;
        }
        public Point randomizer(bool isHorizontal, int begin)
        {
            Random rand = new Random();
            Point temp = new Point();
            bool possibly = false;
            while (possibly != true)
            {
                temp.X = rand.Next(1, 11);
                temp.Y = rand.Next(1, 11);
                if (isHorizontal)
                {
                    if (botmap[temp.Y, temp.X] != 1 && botmap[temp.Y, temp.X] != -1)
                    {
                        if (temp.X + begin < 11)
                            possibly = true;
                        else
                            possibly = false;
                    }
                }
                else
                {
                    if (botmap[temp.Y, temp.X] != 1 && botmap[temp.Y, temp.X] != -1)
                    {
                        if (temp.Y + begin < 11)
                            possibly = true;
                        else
                            possibly = false;
                    }
                }
            }
            return temp;
        }
        public void Place_the_ship(List<Point> points) // поместить корабль и закрасить окрестности
        {
            Ship ship = new Ship();

            foreach (Point p in points) // корабль = 1
            {
                TPoint temp = new TPoint(p);
                ship.Add(temp);
                botmap[p.Y, p.X] = 1;
                //botbuttons[p.Y, p.X].BackColor = Color.Red;
            }
            ships.Add(ship);
            for (int i = 0; i < Form1.mapsize; i++) // окрестность = -1
            {
                for (int j = 0; j < Form1.mapsize; j++)
                {
                    if (botmap[i, j] == 1)
                    {
                        for (int m = i - 1; m <= i + 1; m++)
                        {
                            for (int n = j - 1; n <= j + 1; n++)
                            {
                                if ((m < Form1.mapsize) && (n < Form1.mapsize) && botmap[m, n] != 1)
                                {
                                    botmap[m, n] = -1;
                                    /*if (m != 0 && n != 0 && m != 12 && n != 12)
                                        botbuttons[m, n].BackColor = Color.BlueViolet;*/
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool Check_all_ships()
        {
            bool checker;
            int alive = 0;
            foreach (Ship temp in ships)
            {
                temp.Checker_for_one_ship();
                if (temp._isValid())
                    alive++;
            }
            if (alive == counter)
                checker = true;
            else
                checker = false;
            counter = alive;
            return (checker);
        }
        public void Getter_of_Hit(int y, int x)
        {
            foreach (Ship temp in ships)
                temp.Getter_of_Hit(y, x);
        }
        public int Getter_of_alives()
        {
            int alives = 0;
            foreach (Ship temp in ships)
            {
                temp.Checker_for_one_ship();
                if (temp._isValid())
                    alives++;
            }
            return (alives);
        }
        public List<List<TPoint>> Dead_list()
        {
            List<List<TPoint>> DeadList = new List<List<TPoint>>();
            foreach (Ship temp in ships)
                if (!temp._isValid())
                    DeadList.Add(temp.Getter());
            return DeadList;
        }
        public void Trigger_arrangement()
        {
            The_main_arrangement();
        }
        private async void Print(Button pressedButton)
        {
            foreach (Image img in anim.frame.ToList())
            {
                pressedButton.Image = img;

                await Task.Delay(100);
            }
        }
        public bool BotShoot(bool smart)
        {
            anim.GettingImages();
            List<Ship> list = player.Getter_of_all_ships();
            List<Ship> listOfShips = new List<Ship>();
            foreach (Ship s in list)
            {
                if (!s._isValid())
                    continue;
                bool flag = true;
                foreach (TPoint p in s.Getter())
                    flag = flag && p.getValid();
                if (!flag)
                    listOfShips.Add(s);
            }
            Random r = new Random();
            double chance = r.NextDouble();
            if (chance > 0.5 && listOfShips.Count >= 1)
            {
                Ship s = listOfShips[r.Next(0, listOfShips.Count - 1)];
                TPoint point = null;
                foreach (TPoint p in s.Getter())
                    if (p.getValid())
                        point = p;
                if (point == null)
                    return Trigger_BotShoot(r.Next(1, Form1.mapsize), r.Next(1, Form1.mapsize), false);
                return Trigger_BotShoot(point.point.X, point.point.Y, true);
            }
            else
                return Trigger_BotShoot(r.Next(1, Form1.mapsize), r.Next(1, Form1.mapsize), false);
        }
        private bool Trigger_BotShoot(int x, int y, bool smart)
        {
            Random rand = new Random();
            bool goal = false;
            if (Form1.enabled)
            {
                while (map[y, x] == 2 || map[y, x] == -2 || mybuttons[y, x].BackColor == Color.Aquamarine)
                {
                    x = rand.Next(1, 11);
                    y = rand.Next(1, 11);
                }
                if (map[y, x] == 1)
                {
                    player.Getter_of_Hit(y, x);
                    Print(mybuttons[y, x]);
                    map[y, x] = 2;
                    mybuttons[y, x].BackColor = Color.Black;
                    goal = true;
                }
                else
                {
                    goal = false;
                    mybuttons[y, x].BackColor = Color.Aquamarine;
                }
                if (goal)
                {
                    if (smart)
                        BotShoot(true);
                    else
                        BotShoot(false);
                }
            }
            return (goal);
        }
        public void Print_Deads()
        {
            List<List<TPoint>> list = player.Dead_list();
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
                        if (i > 0 && j > 0 && i < 11 && j < 11 && map[i, j] != 2)
                        {
                            mybuttons[i, j].BackColor = Color.Aquamarine;
                            mybuttons[i, j].Enabled = false;
                        }

                    }
                }
            }
        }
    }
}