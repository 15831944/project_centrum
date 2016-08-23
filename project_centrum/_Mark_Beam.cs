using System;
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
    public class _Mark_Beam : _Mark
    {
        public _Mark_Beam(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject dr) : base(mark, part, dr)
        {

        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.Beam part1 = _part as TSM.Beam;
            TSM.Beam part2 = other._part as TSM.Beam;

            if (! compare2Points(part1.StartPoint, part2.StartPoint) )
            {
                return false;
            }
            if (! compare2Points(part1.EndPoint, part2.EndPoint) )
            {
                return false;
            }

            return true;
        }

    }
}
