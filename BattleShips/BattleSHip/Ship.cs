using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleSHip
{
    class Ship
    {
        List<TPoint> ship;
        private bool isValid;
        public Ship()
        {
            ship = new List<TPoint>();
            isValid = true;
        }
        public List<TPoint> Getter()
        {
            return ship;
        }
        public void Add(TPoint temp)
        {
            ship.Add(temp);
        }
        public void Getter_of_Hit(int y, int x)
        {
            foreach (TPoint temp in ship)
            {
                if (temp.point.Y == y && temp.point.X == x)
                {
                    temp.setValid(false);
                }
            }
        }
        public void Checker_for_one_ship()
        {
            int i = 0;
            foreach (TPoint temp in ship)
                if (temp.getValid() == true)
                    i++;
            if (i != 0)
                isValid = true;
            else
                isValid = false;
        }
        public bool _isValid()
        {
            return (isValid);
        }
    }
}
