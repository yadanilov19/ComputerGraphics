using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Points;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Logic
{
    public static class CreateMapPerspetiveGeometryPlot
    {
        static int n = 4; //размерность матриц
        public static List<Points.Point2D> CentralProjection(double H, double W, Point4D C, List<Points.Point4D> Points)
        {
            List<Point2D> result;
            CheckC(C);
            Equals_T_and_C(C, Points.First(x => x.Name == "T"));
            InspactionAngle(C, Points.First(x => x.Name == "T"));

            Calculate(H,W, C,Points, CreateRz(C) * CreateRx(C) * CreateMx() * CreateCz(C) * CreatePz() * CreateT(H,W), out result);

            OutFromDislay(result.First(x => x.Name == "T"), H, W);
            return result;
        }

        private static void Calculate(double H, double W, Point4D C, List<Point4D> Points,Matrix<Double> A,  out List<Point2D> result)
        {
            var t = Points.First(x => x.Name == "T");//точка пространства

            result = new List<Point2D>();

            foreach (Point4D p in Points)
            {
                Matrix<double> point = p.getVector();
                p.setVector(point * A);
                p.Norm();
                if (p.Name == "X" || p.Name == "Y" || p.Name == "Z")
                {
                    var O = result[0];
                    var b = new Point2D() { X = (p.X - O.X) * (W < H ? W : H) + O.X, Y = (p.Y - O.Y) * (W < H ? W : H) + O.Y, Name = p.Name };
                    result.Add(b);
                }
                else
                    result.Add(new Point2D() { X = p.X, Y = p.Y, Name = p.Name });
            }
        }

        private static void OutFromDislay(Point2D t, double h, double w)
        {
            if (t.X < 0 || t.Y > h || t.Y < 0 || t.X > w)
                throw new Exception("Точка находится за границами экрана");
        }

        private static void InspactionAngle(Point4D c, Point4D t)
        {
            var T = c.X * (t.X - c.X) + c.Y * (t.Y - c.Y) + c.Z * (t.Z - c.Z);
            if (T >= 0)
                throw new Exception("Проецирующие лучи пересекают проецирующую плоскость");
        }

        private static void Equals_T_and_C(Point4D c, Point4D t)
        {
            if (c == t)
                throw new Exception("Точка T совпадает с точкой C");
        }


        public static List<Points.Point2D> OrtProjection(double H, double W, Point4D C, List<Points.Point4D> Points)
        {
            List<Point2D> result;
            CheckC(C);
            Calculate(H, W, C, Points, CreateRz(C)* CreateRx(C) * CreateMx() * CreatePz() * CreateT(H,W), out result);

            return result;
        }

        private static void CheckC(Point4D C)
        {
            if (!(C.X > 0 || C.Y > 0 || C.Z > 0))
            {          
                throw new Exception("C == O");
            }
        }

        private static Matrix<double> CreateT(double h, double w)
        {
            var tmp = Matrix<double>.Build.DenseIdentity(n);
            tmp[3, 0] = w / 2; 
            tmp[3, 1] = h / 2;
            return tmp;
        }

        private static Matrix<double> CreateCz(Point4D C)
        {
            var tmp = Matrix<double>.Build.DenseIdentity(n);
            tmp[2, 3] = -1 / Math.Sqrt(C.Z * C.Z + C.X * C.X + C.Y * C.Y);
            return tmp;
        }

        private static Matrix<double> CreatePz()
        {
            var tmp = Matrix<double>.Build.DenseIdentity(n);
            tmp[2, 2] = 0;
            return tmp;
        }

        private static Matrix<double> CreateMx()
        {
            var tmp = Matrix<double>.Build.DenseIdentity(n);
            tmp[0, 0] = -1;
            return tmp;
        }

        private static Matrix<double> CreateRx(Point4D C)
        {
            var tmp = Matrix<double>.Build.DenseIdentity(n);
            tmp[1, 1] = C.Z / Math.Sqrt(C.X * C.X + C.Y * C.Y + C.Z * C.Z);
            tmp[2, 2] = C.Z / Math.Sqrt(C.X * C.X + C.Y * C.Y + C.Z * C.Z);
            tmp[2, 1] = -Math.Sqrt(C.X * C.X + C.Y * C.Y) / Math.Sqrt(C.X * C.X + C.Y * C.Y + C.Z * C.Z);
            tmp[1, 2] = Math.Sqrt(C.X * C.X + C.Y * C.Y) / Math.Sqrt(C.X * C.X + C.Y * C.Y + C.Z * C.Z);
            return tmp;
        }

        private static Matrix<double> CreateRz(Point4D C)
        {
            var tmp = Matrix<double>.Build.DenseIdentity(n);
            if(C.X > 0 || C.Y > 0)
            {
                tmp[0, 0] = C.Y / Math.Sqrt(C.X * C.X + C.Y * C.Y);
                tmp[1, 1] = C.Y / Math.Sqrt(C.X * C.X + C.Y * C.Y);
                tmp[1, 0] = -C.X / Math.Sqrt(C.X * C.X + C.Y * C.Y);
                tmp[0, 1] =  C.X / Math.Sqrt(C.X * C.X + C.Y * C.Y);
            }

            return tmp;
        }
    }
}
