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

        public _Mark(TSD.Mark mark, TSM.ModelObject part, TSD.ModelObject DRpart)
        {
            _mark = mark;
            _part = part;
            _DRpart = DRpart;
        }


        public abstract bool checkModelObjects(_Mark name);


        public void reCreateMark(_Mark input)
        {
            _mark.Delete();

            TSD.Mark newMark = new TSD.Mark(_DRpart);

            if (input._mark.Placing is TSD.AlongLinePlacing)
            {
                TSD.AlongLinePlacing attr = input._mark.Placing as TSD.AlongLinePlacing;
                T3D.Point start = __Transformster.Transform(attr.StartPoint);
                T3D.Point end = __Transformster.Transform(attr.EndPoint);
                TSD.AlongLinePlacing newPlacing = new TSD.AlongLinePlacing(start, end);
                newMark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.BaseLinePlacing)
            {
                TSD.BaseLinePlacing attr = input._mark.Placing as TSD.BaseLinePlacing;
                T3D.Point start = __Transformster.Transform(attr.StartPoint);
                T3D.Point end = __Transformster.Transform(attr.EndPoint);
                TSD.BaseLinePlacing newPlacing = new TSD.BaseLinePlacing(start, end);
                newMark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.LeaderLinePlacing)
            {
                TSD.LeaderLinePlacing attr = input._mark.Placing as TSD.LeaderLinePlacing;
                T3D.Point start = __Transformster.Transform(attr.StartPoint);
                TSD.LeaderLinePlacing newPlacing = new TSD.LeaderLinePlacing(start);
                newMark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.PointPlacing)
            {
                TSD.PointPlacing newPlacing = new TSD.PointPlacing();
                newMark.Placing = newPlacing;
            }

            newMark.Attributes = input._mark.Attributes;
            newMark.Insert();

            newMark.InsertionPoint = __Transformster.Transform(input._mark.InsertionPoint); 
            newMark.Modify();

        }


        public bool compare2Points(T3D.Point p1, T3D.Point p2)
        {
            Form1._form.add_text("DUH" + p1.X.ToString() + " " + p1.Y.ToString());

            Form1._form.add_text("BEFORE: " + p2.X.ToString() + " " + p2.Y.ToString());
            
            p2 = __Transformster.Transform(p2);

            Form1._form.add_text("AFTER: " + p2.X.ToString() + " " + p2.Y.ToString());

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

                p2 = __Transformster.Transform(p2);

                if (! compare2Points(p1, p2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
