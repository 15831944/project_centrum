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

        public _Mark(TSD.Mark mark, TSM.ModelObject part)
        {
            _mark = mark;
            _part = part;
        }

        public abstract bool checkModelObjects(_Mark name);

        public void changeMarkAttributes(_Mark input)
        {
            _mark.Attributes = input._mark.Attributes;
            _mark.Modify();
        }

        public void changeMarkLocation(_Mark input)
        {
            _mark.Placing = input._mark.Placing;
            _mark.Modify();
            _mark.InsertionPoint.X = input._mark.InsertionPoint.X;
            _mark.InsertionPoint.Y = input._mark.InsertionPoint.Y;
            _mark.Modify();
        }

        public void objectNotFound()
        {
            _mark.Attributes.Frame.Color = TSD.DrawingColors.Red;
            _mark.Attributes.Frame.Type = TSD.FrameTypes.Rectangular;
            _mark.Modify();
        }

        public bool compare2Points(T3D.Point p1, T3D.Point p2)
        {
            if (Math.Abs(p1.X - p2.X) > 1.0)
            {
                return false;
            }
            if (Math.Abs(p1.Y - p2.Y) > 1.0)
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

                if (! compare2Points(p1, p2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
