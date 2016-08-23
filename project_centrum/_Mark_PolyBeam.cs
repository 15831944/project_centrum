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
    public class _Mark_PolyBeam : _Mark
    {
        public _Mark_PolyBeam(TSD.Mark mark, TSM.ModelObject part) : base(mark, part)
        {

        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.PolyBeam part1 = _part as TSM.PolyBeam;
            TSM.PolyBeam part2 = other._part as TSM.PolyBeam;

            ArrayList points1 = part1.Contour.ContourPoints;
            ArrayList points2 = part2.Contour.ContourPoints;

            if (! comparePointArray(points1, points2))
            {
                return false;
            }

            return true;
        }

    }
}