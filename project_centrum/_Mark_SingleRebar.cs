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
    class _Mark_SingleRebar : _Mark
    {
        public _Mark_SingleRebar(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject dr, TSD.ViewBase vv) : base(mark, part, dr, vv)
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
            TSM.SingleRebar part1 = _part as TSM.SingleRebar;
            TSM.SingleRebar part2 = other._part as TSM.SingleRebar;

            ArrayList points1 = part1.Polygon.Points;
            ArrayList points2 = part2.Polygon.Points;

            ArrayList transformed1 = __GeometryOperations.factorPointArray(points1, _view as TSD.View);
            ArrayList transformed2 = __GeometryOperations.factorPointArray(points2, other._view as TSD.View);

            if (! __GeometryOperations.comparePointArray(transformed1, transformed2) )
            {
                return false;
            }

            return true;
        }
        
    }
}