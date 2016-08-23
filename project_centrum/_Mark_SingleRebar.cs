using System;
using System.Collections;
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
    class _Mark_SingleRebar : _Mark
    {
        public _Mark_SingleRebar(TSD.Mark mark, TSM.ModelObject part) : base(mark, part)
        {

        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.SingleRebar part1 = _part as TSM.SingleRebar;
            TSM.SingleRebar part2 = other._part as TSM.SingleRebar;

            ArrayList points1 = part1.Polygon.Points;
            ArrayList points2 = part2.Polygon.Points;

            if (! comparePointArray(points1, points2))
            {
                return false;
            }

            return true;
        }
        
    }
}