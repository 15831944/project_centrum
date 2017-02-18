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
    public class _Mark_BoltGroup : _Mark
    {
        public _Mark_BoltGroup(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject dr, TSD.ViewBase vv) : base(mark, part, dr, vv)
        {

        }

        public override void tryPredict<T>(List<T> others)
        {

        }

        public override T3D.Vector getDirection()
        {
            return new T3D.Vector(0, 0, 0);
        }

        public override T3D.Vector getDirectionOther()
        {
            return new T3D.Vector(0, 0, 10);
        }
        
        public override bool checkModelObjects(_Mark other)
        {
            TSM.BoltGroup part1 = _part as TSM.BoltGroup;
            TSM.BoltGroup part2 = other._part as TSM.BoltGroup;

            T3D.Point p1start = __GeometryOperations.factor1Point(part1.FirstPosition, _view as TSD.View);
            T3D.Point p1end = __GeometryOperations.factor1Point(part1.SecondPosition, _view as TSD.View);
            T3D.Point p2start = __GeometryOperations.factor1Point(part2.FirstPosition, other._view as TSD.View);
            T3D.Point p2end = __GeometryOperations.factor1Point(part2.SecondPosition, other._view as TSD.View);

            if (!__GeometryOperations.compare2Points(p1start, p2start) )
            {
                return false;
            }
            if (!__GeometryOperations.compare2Points(p1end, p2end) )
            {
                return false;
            }

            return true;
        }

    }
}

