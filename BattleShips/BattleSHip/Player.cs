using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleSHip
{ 
    class Player
    {
        public int[,] map = new int[Form1.mapsize, Form1.mapsize];
        public int[,] botmap = new int[Form1.mapsize, Form1.mapsize];
        private List<Ship> ships = new List<Ship>();
        public Button[,] mybuttons = new Button[Form1.mapsize, Form1.mapsize];
        public Button[,] botbuttons = new Button[Form1.mapsize, Form1.mapsize];
        public Dictionary<int, int> Num_of_ships = new Dictionary<int, int>();
        private int counter;
        public Player(ref int[,] map, ref int[,] botmap, ref Button[,] mybuttons, ref Button[,] botbuttons)
        {
            counter = 10;
            this.map = map;
            this.botmap = botmap;
            this.botbuttons = botbuttons;
            this.mybuttons = mybuttons;
            Num_of_ships.Add(4, 1);
            Num_of_ships.Add(3, 2);
            Num_of_ships.Add(2, 3);
            Num_of_ships.Add(1, 4);
        }
        public void The_main_arrangement(bool isHorizontal, int pos, Button temp)
        {
            bool check1;
            if (isHorizontal)
            {
                if (temp.Location.X / Form1.boxsize + pos <= Form1.mapsize)
                    check1 = true;
                else
                    check1 = false;
            }
            else
            {
                if (temp.Location.Y / Form1.boxsize + pos <= Form1.mapsize)
                    check1 = true;
                else
                    check1 = false;
            }
            if (Num_of_ships[pos] > 0)
            {
                if (check1 == true)
                {
                    int dx = 0;
                    int dy = 0;
                    List<Point> points = new List<Point>();
                    for (int i = 0; i < pos; i++)
                    {
                        int x = temp.Location.X / Form1.boxsize + dx;
                        int y = temp.Location.Y / Form1.boxsize + dy;
                        points.Add(new Point(x, y));
                        if (isHorizontal)
                            dx++;
                        else 
                            dy++;
                    }
                    if (check_around(points))
                    {
                        Place_the_ship(points); 
                        Num_of_ships[pos]--;
                    }
                    else
                        MessageBox.Show("Вы поместили корабль рядом с другим кораблём");
                }
                else
                    MessageBox.Show("Вы помещаете корабль вне карты");
            }
            else
                MessageBox.Show("Кораблей с таким количеством палуб больше не осталось");
        }
        public bool check_around(List<Point> temp)
        {
            bool check = true;

            foreach (Point p in temp)
                check = check && !(map[p.Y, p.X] == 1 || map[p.Y, p.X] == -1);
            return check;
        }
        public void Place_the_ship(List<Point> points) // поместить корабль и закрасить окрестности
        {
            Ship ship = new Ship();

            foreach (Point p in points) // помещаю корабль
            {
                TPoint temp = new TPoint();
                ship.Add(temp); 
                map[p.Y, p.X] = 1;
                mybuttons[p.Y, p.X].BackColor = Color.Red;
            }
            ships.Add(ship);
            for (int i = 0; i < Form1.mapsize; i++) // закрашиваю окрестность
            {
                for (int j = 0; j < Form1.mapsize; j++)
                {
                    if (map[i, j] == 1)
                    {
                        for (int m = i - 1; m <= i + 1; m++)
                        {
                            for (int n = j - 1; n <= j + 1; n++)
                            {
                                if ((m < Form1.mapsize) && (n < Form1.mapsize) && map[m, n] != 1)
                                {
                                    map[m, n] = -1;
                                    if (m != 0 && n != 0 && m != 12 && n != 12)
                                        mybuttons[m, n].BackColor = Color.BlueViolet;
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
            foreach(Ship temp in ships)
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
            //MessageBox.Show(alives.ToString());
            return (alives);
        }
        public List<Ship> Getter_of_all_ships()
        {
            return ships;
        }
        public List<List<TPoint>> Dead_list()
        {
            List<List<TPoint>> DeadList= new List<List<TPoint>>();
            foreach (Ship temp in ships)
                if (!temp._isValid())
                    DeadList.Add(temp.Getter());
            return DeadList;
        }
    }
}
