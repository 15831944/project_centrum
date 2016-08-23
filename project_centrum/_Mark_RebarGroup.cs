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
        public _Mark_RebarGroup(TSD.Mark mark, TSM.ModelObject part) : base(mark, part)
        {

        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.RebarGroup part1 = _part as TSM.RebarGroup;
            TSM.RebarGroup part2 = other._part as TSM.RebarGroup;

            if (! compare2Points(part1.StartPoint, part2.StartPoint))
            {
                return false;
            }
            if (! compare2Points(part1.EndPoint, part2.EndPoint))
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

                if (! comparePointArray(points1, points2))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
