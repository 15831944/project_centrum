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
        public _Mark_Beam(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject dr, TSD.ViewBase vv) : base(mark, part, dr, vv)
        {

        }

        public override void tryPredict(_Mark other)
        {

        }

        public override T3D.Vector getDirection()
        {
            T3D.Point start = (_part as TSM.Beam).StartPoint;
            T3D.Point end = (_part as TSM.Beam).EndPoint;

            double dX = end.X - start.X;
            double dY = end.Y - start.Y;
            double dZ = end.Z - start.Z;

            double dT = Math.Abs(dX) + Math.Abs(dY) + Math.Abs(dZ);

            dX = dX / dT;
            dY = dY / dT;
            dZ = dZ / dT;

            T3D.Vector vector = new T3D.Vector(dX, dY, dZ);

            return vector;
        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.Beam part1 = _part as TSM.Beam;
            TSM.Beam part2 = other._part as TSM.Beam;

            T3D.Point p1start = factor1Point(part1.StartPoint, _view as TSD.View);
            T3D.Point p1end = factor1Point(part1.EndPoint, _view as TSD.View);
            T3D.Point p2start = factor1Point(part2.StartPoint, other._view as TSD.View);
            T3D.Point p2end = factor1Point(part2.EndPoint, other._view as TSD.View);

            if (! compare2Points(p1start, p2start) )
            {
                return false;
            }
            if (! compare2Points(p1end, p2end) )
            {
                return false;
            }

            return true;
        }

    }
}
