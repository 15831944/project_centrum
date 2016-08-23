using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;

namespace project_centrum
{
    static class __Transformster
    {
        public static T3D.Point Transform(T3D.Point input)
        {
            if (UserProperties.a != null && UserProperties.b != null)
            {
                T3D.Point a = UserProperties.a;
                T3D.Point b = UserProperties.b;

                double dX = b.X - a.X;
                double dY = b.Y - a.Y;

                double cs = Math.Cos(Math.PI * UserProperties._dR / 180);
                double sn = Math.Sin(Math.PI * UserProperties._dR / 180);

                double iX = input.X - a.X;
                double iY = input.Y - a.Y;

                double X = (iX * cs) - (iY * sn) + dX + a.X;
                double Y = (iX * sn) + (iY * cs) + dY + a.Y;
                double Z = input.Z;

                T3D.Point tr = new T3D.Point(X, Y, Z);

                return tr;
            }

            else
            {
                return input;
            }
        }
    }
}
