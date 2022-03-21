using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSHip
{
    internal class TPoint
    {
        public Point point;
        bool isValid;

        public TPoint()
        {
            isValid = true;
        }
        public TPoint(Point point)
        {
            this.point = point;
            isValid = true;
        }
        public bool getValid()
        {
            return (isValid);
        }
        public void setValid(bool flag)
        {
            isValid = flag;
        }
    }
}
