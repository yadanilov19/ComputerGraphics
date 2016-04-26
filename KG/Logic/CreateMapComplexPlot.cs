using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class CreateMapComplexPlot
    {
        /// <summary>
        /// Возвращает проекции точки для построения комплексного чертежа
        /// </summary>
        /// <param name="H">Высота области чертежа</param>
        /// <param name="W">Ширина области чертежа</param>
        /// <param name="Points">Точки пространства, которые нужно отобразить на комплексном чертеже</param>
        /// <returns></returns>
        public static List<List<Points.Point2D>> getPointsOnWindow(double H, double W, List<Points._Point> Points)
        {
            List<List<Points.Point2D>> result = new List<List<Logic.Points.Point2D>>()
            {
                new List<Logic.Points.Point2D>()
                {
                    new Logic.Points.Point2D() { X =  W/2, Y = H/2, Name = "O"},
                    new Logic.Points.Point2D() { X =  0, Y = H/2, Name = "X"},
                    new Logic.Points.Point2D() { X =  W, Y = H/2, Name = "-X"},
                    new Logic.Points.Point2D() { X =  W/2, Y = H, Name = "-Z"},
                    new Logic.Points.Point2D() { X =  W/2, Y = 0, Name = "Z"},
                }
            };

            foreach(Points._Point p in Points)
            {
                var point = (Points.Point4D)p;
                if (point.Equals(null))
                    return null;

                var _X = point.X;
                var _Y = point.Y;
                var _Z = point.Z;

                result.Add(new List<Points.Point2D>() {
                    new Logic.Points.Point2D() { X =  W/2 - _X, Y = H/2 + _Y, Name = point.Name + "1"},
                    new Logic.Points.Point2D() { X =  W/2 + _Y, Y = H/2 - _Z, Name = point.Name + "3"},
                    new Logic.Points.Point2D() { X =  W/2 - _X, Y = H/2 - _Z, Name = point.Name + "2"},
                    new Logic.Points.Point2D() { X =  W/2 + _Y, Y = H/2, Name = point.Name + "y1"},
                    new Logic.Points.Point2D() { X =  W/2, Y = H/2 + _Y, Name = point.Name + "y2"},
                    new Logic.Points.Point2D() { X =  W/2 - _X, Y = H/2, Name = point.Name + "x"},
                    new Logic.Points.Point2D() { X =  W/2, Y = H/2 - _Z, Name = point.Name + "z"}
                });
            }

            return result;
        }
    }
}
