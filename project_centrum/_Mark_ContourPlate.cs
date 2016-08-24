using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;

namespace project_centrum
{
    public class _Mark_ContourPlate : _Mark
    {
        public _Mark_ContourPlate(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject dr, TSD.ViewBase vv) : base(mark, part, dr, vv)
        {

        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.ContourPlate part1 = _part as TSM.ContourPlate;
            TSM.ContourPlate part2 = other._part as TSM.ContourPlate;

            ArrayList points1 = part1.Contour.ContourPoints;
            ArrayList points2 = part2.Contour.ContourPoints;

            ArrayList transformed1 = factorPointArray(points1, _view as TSD.View);
            ArrayList transformed2 = factorPointArray(points2, other._view as TSD.View);

            if (! comparePointArray(transformed1, transformed2) )
            {
                return false;
            }

            return true;
        }

    }
}