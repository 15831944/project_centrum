using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Tekla.Structures;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;

namespace project_centrum
{
    class _Mark_RebarGroup : _Mark
    {
        public _Mark_RebarGroup(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject dr, TSD.ViewBase vv) : base(mark, part, dr, vv)
        {

        }

        public override void tryPredict<T>(List<T> others)
        {

        }

        public override T3D.Vector getDirection()
        {
            return new T3D.Vector(0, 0, 0);
        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.RebarGroup part1 = _part as TSM.RebarGroup;
            TSM.RebarGroup part2 = other._part as TSM.RebarGroup;

            T3D.Point p1start = __GeometryOperations.factor1Point(part1.StartPoint, _view as TSD.View);
            T3D.Point p1end = __GeometryOperations.factor1Point(part1.EndPoint, _view as TSD.View);
            T3D.Point p2start = __GeometryOperations.factor1Point(part2.StartPoint, other._view as TSD.View);
            T3D.Point p2end = __GeometryOperations.factor1Point(part2.EndPoint, other._view as TSD.View);

            if (!__GeometryOperations.compare2Points(p1start, p2start) )
            {
                return false;
            }
            if (!__GeometryOperations.compare2Points(p1end, p2end) )
            {
                return false;
            }

            ArrayList polygon1 = part1.Polygons;
            ArrayList polygon2 = part2.Polygons;

            if (polygon1.Count != polygon2.Count)
            {
                return false;
            }

            for (int i = 0; i < polygon1.Count; i++)
            {
                TSM.Polygon poly1 = polygon1[i] as TSM.Polygon;
                TSM.Polygon poly2 = polygon2[i] as TSM.Polygon;

                ArrayList points1 = poly1.Points;
                ArrayList points2 = poly2.Points;

                ArrayList transformed1 = __GeometryOperations.factorPointArray(points1, _view as TSD.View);
                ArrayList transformed2 = __GeometryOperations.factorPointArray(points2, other._view as TSD.View);

                if (!__GeometryOperations.comparePointArray(transformed1, transformed2))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
