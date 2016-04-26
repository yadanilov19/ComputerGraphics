using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Points
{
    public class Point2D
    {
        private string name;
        double _X, _Y;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return _X;
            }

            set
            {
                _X = value;
            }
        }

        public double Y
        {
            get
            {
                return _Y;
            }

            set
            {
                _Y = value;
            }
        }

        public static bool operator ==(Point2D a, Point2D b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point2D a, Point2D b)
        {
            return !(a == b);
        }

    }
}
