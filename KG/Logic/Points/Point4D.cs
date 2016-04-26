using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Logic.Points
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public class Point4D : Point3D, _Point
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        double _W  = 1;
        Matrix<double> Vector;
        string name;
        public double W
        {
            get
            {
                return _W;
            }

            set
            {
                _W = value;
            }
        }

        public void Norm()
        {
            this.X /= W;
            this.Y /= this.W;
            this.Z /= this.W;
            W = 1;
        }

        internal void setVector(Matrix<double> V)
        {
            X = V[0,0];
            Y = V[0,1];
            Z = V[0,2];
            W = V[0,3];
        }

        internal  Matrix<double> getVector()
        {
            return Vector = Matrix<double>.Build.DenseOfRows(new List<List<double>>() { new List<double>() { X, Y, Z, W } });
        }

        public static bool operator ==(Point4D a, Point4D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Point4D a, Point4D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static Point4D Zero()
        {
            return new Point4D();
        }

        public Point4D Clone()
        {
            return new Point4D() { X = X, Y = Y, Z = Z, W = W, Name = Name };
        }
    }
}
