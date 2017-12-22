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
        public abstract void tryPredict<T>(List<T> others) where T : _Mark;
        public abstract T3D.Vector getDirection();
        public abstract T3D.Vector getDirectionOther();


        public void reCreateMark(_Mark input)
        {
            _mark.Attributes = input._mark.Attributes;
            _mark.Modify();

            if (input._mark.Placing is TSD.LeaderLinePlacing)
            {
                TSD.LeaderLinePlacing attr = input._mark.Placing as TSD.LeaderLinePlacing;
                T3D.Point start = __GeometryOperations.applyGlobalOffset(attr.StartPoint);
                TSD.LeaderLinePlacing newPlacing = new TSD.LeaderLinePlacing(start);
                _mark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.AlongLinePlacing)
            {
                TSD.AlongLinePlacing attr = input._mark.Placing as TSD.AlongLinePlacing;
                T3D.Point start = __GeometryOperations.applyGlobalOffset(attr.StartPoint);
                T3D.Point end = __GeometryOperations.applyGlobalOffset(attr.EndPoint);
                TSD.AlongLinePlacing newPlacing = new TSD.AlongLinePlacing(start, end);
                _mark.Placing = newPlacing;
            }

            else if (input._mark.Placing is TSD.BaseLinePlacing)
            {
                //TSD.BaseLinePlacing attr = input._mark.Placing as TSD.BaseLinePlacing;

                //T3D.Point start = __GeometryOperations.applyGlobalOffset(attr.StartPoint);
                //T3D.Point end = __GeometryOperations.applyGlobalOffset(attr.EndPoint);

                //TSD.BaseLinePlacing newPlacing = new TSD.BaseLinePlacing(start, end);
                //_mark.Placing = newPlacing;
            }
            
            else if (input._mark.Placing is TSD.PointPlacing)
            {
                TSD.PointPlacing newPlacing = new TSD.PointPlacing();
                _mark.Placing = newPlacing;
            }
            
            _mark.InsertionPoint = __GeometryOperations.applyGlobalOffset(input._mark.InsertionPoint);
            _mark.Modify();
            _mark.Modify();
        }

        public void applyRedBorder()
        {
            _mark.Attributes.Frame.Color = TSD.DrawingColors.Red;
            _mark.Attributes.Frame.Type = TSD.FrameTypes.Rectangular;
            _mark.Modify();
        }

    }
}
