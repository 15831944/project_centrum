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
    public abstract class _Mark
    {
        public TSD.Mark _mark;
        public TSM.ModelObject _part;
        private TSD.ModelObject _DRpart;
        internal TSD.ViewBase _view;

        public _Mark(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject DRpart, TSD.ViewBase view)
        {
            _mark = mark;
            _part = part;
            _DRpart = DRpart;
            _view = view;
        }


        public abstract bool checkModelObjects(_Mark name);

        public abstract void tryPredict(_Mark other);

        public abstract T3D.Vector getDirection();

        public void reCreateMark(_Mark input)
        {
            _mark.Attributes = input._mark.Attributes;

            if (input._mark.Placing is TSD.AlongLinePlacing)
            {
                TSD.AlongLinePlacing attr = input._mark.Placing as TSD.AlongLinePlacing;
                T3D.Point start = __Transformster.Transform2(attr.StartPoint);
                T3D.Point end = __Transformster.Transform2(attr.EndPoint);
                TSD.AlongLinePlacing newPlacing = new TSD.AlongLinePlacing(start, end);
                _mark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.BaseLinePlacing)
            {
                TSD.BaseLinePlacing attr = input._mark.Placing as TSD.BaseLinePlacing;
                T3D.Point start = __Transformster.Transform2(attr.StartPoint);
                T3D.Point end = __Transformster.Transform2(attr.EndPoint);
                TSD.BaseLinePlacing newPlacing = new TSD.BaseLinePlacing(start, end);
                _mark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.LeaderLinePlacing)
            {
                TSD.LeaderLinePlacing attr = input._mark.Placing as TSD.LeaderLinePlacing;
                T3D.Point start = __Transformster.Transform2(attr.StartPoint);
                TSD.LeaderLinePlacing newPlacing = new TSD.LeaderLinePlacing(start);
                _mark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.PointPlacing)
            {
                TSD.PointPlacing newPlacing = new TSD.PointPlacing();
                _mark.Placing = newPlacing;
            }

            _mark.InsertionPoint = __Transformster.Transform2(input._mark.InsertionPoint);
            _mark.Modify();
            _mark.Modify();
        }

        public T3D.Point factor1Point(T3D.Point pp, TSD.View vv)
        {
            T3D.Matrix convMatrix = T3D.MatrixFactory.ToCoordinateSystem(vv.DisplayCoordinateSystem);
            return convMatrix.Transform(pp);
        }

        public ArrayList factorPointArray(ArrayList pps, TSD.View vv)
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

        public bool compare2Points(T3D.Point p1, T3D.Point p2)
        {
            p2 = __Transformster.Transform2(p2);

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

        public bool comparePointArray(ArrayList points1, ArrayList points2)
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
