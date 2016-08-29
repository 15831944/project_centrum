using System;
using System.Collections;
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
    static class __GeometryOperations
    {
        public static T3D.Point applyGlobalOffset(T3D.Point input)
        {
            if (UserProperties.a != null && UserProperties.b != null)
            {
                T3D.Point a = UserProperties.a;
                T3D.Point b = UserProperties.b;

                double cs = Math.Cos(Math.PI * UserProperties._dR / 180);
                double sn = Math.Sin(Math.PI * UserProperties._dR / 180);

                double dX = b.X - a.X;
                double dY = b.Y - a.Y;

                double iX = input.X - a.X;
                double iY = input.Y - a.Y;

                double X = (iX * cs) - (iY * sn) + a.X + dX;
                double Y = (iX * sn) + (iY * cs) + a.Y + dY;

                T3D.Point tr = new T3D.Point(X, Y, 0);

                return tr;
            }

            else
            {
                return input;
            }
        }

        public static bool areOnSameLine(T3D.Point A, T3D.Point B, T3D.Point C)
        {
            bool line = Math.Abs(A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)) < 1;
            return line;
        }

        public static T3D.Vector getDirectionVector(T3D.Point start, T3D.Point end)
        {
            double dX = end.X - start.X;
            double dY = end.Y - start.Y;
            double dZ = end.Z - start.Z;

            double dT = Math.Pow(Math.Pow(dX, 2) + Math.Pow(dY, 2) + Math.Pow(dZ, 2), 0.5);
            
            dX = dX / dT;
            dY = dY / dT;
            dZ = dZ / dT;

            T3D.Vector vector = new T3D.Vector(dX, dY, dZ);

            return vector;
        }

        public static double getLength(T3D.Point start, T3D.Point end)
        {
            double dX = end.X - start.X;
            double dY = end.Y - start.Y;

            double L = Math.Pow(Math.Pow(dX, 2) + Math.Pow(dY, 2), 0.5);
            return L;
        }

        public static T3D.Point getLocalOffset(T3D.Point start1, T3D.Point start2)
        {
            double X = start1.X - start2.X;
            double Y = start1.Y - start2.Y;

            T3D.Point tr = new T3D.Point(X, Y, 0);

            return tr;
        }

        public static T3D.Point applyLocalOffset(T3D.Point output, T3D.Point offset)
        {
            double dX = output.X - offset.X;
            double dY = output.Y - offset.Y;

            T3D.Point tr = new T3D.Point(dX, dY, 0);

            return tr;
        }

        public static double getAlfa(T3D.Point start1, T3D.Point end1, T3D.Point start2, T3D.Point end2)
        {
            double alfa = getLength(start1, end1);
            double bravo = getLength(start2, end2);

            if (alfa < 0.1) return 1;
            if (bravo < 0.1) return 1;

            double multi = bravo / alfa;

            return multi;
        }

        public static T3D.Point getPlacingOffset(T3D.Point output, T3D.Point placing, double alfa)
        {
            double dX = (placing.X - output.X);

            double dY = (placing.Y - output.Y);

            if (Math.Abs(dX / 100) > dY)
            {
                dX = dX * alfa;
            }

            if (Math.Abs(dY / 100) > dX)
            {
                dY = dY * alfa;
            }


            T3D.Point tr = new T3D.Point(dX, dY, 0);
            
            return tr;
        }

        public static T3D.Point factor1Point(T3D.Point pp, TSD.View vv)
        {
            T3D.Matrix convMatrix = T3D.MatrixFactory.ToCoordinateSystem(vv.DisplayCoordinateSystem);
            return convMatrix.Transform(pp);
        }

        public static ArrayList factorPointArray(ArrayList pps, TSD.View vv)
        {
            ArrayList factored = new ArrayList();

            T3D.Matrix convMatrix = T3D.MatrixFactory.ToCoordinateSystem(vv.DisplayCoordinateSystem);

            foreach (T3D.Point pp in pps)
            {
                T3D.Point fp = convMatrix.Transform(pp);
                factored.Add(fp);
            }

            return factored;
        }

        public static bool compare2Points(T3D.Point p1, T3D.Point p2)
        {
            p2 = __GeometryOperations.applyGlobalOffset(p2);

            if (Math.Abs(p1.X - p2.X) > 2.0)
            {
                return false;
            }
            if (Math.Abs(p1.Y - p2.Y) > 2.0)
            {
                return false;
            }

            return true;
        }

        public static bool comparePointArray(ArrayList points1, ArrayList points2)
        {
            if (points1.Count != points2.Count)
            {
                return false;
            }

            if (points1.Count < 1)
            {
                return false;
            }

            for (int i = 0; i < points1.Count; i++)
            {
                T3D.Point p1 = points1[i] as T3D.Point;
                T3D.Point p2 = points2[i] as T3D.Point;

                if (!compare2Points(p1, p2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
