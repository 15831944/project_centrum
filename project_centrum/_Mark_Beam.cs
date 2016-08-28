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

        public override T3D.Vector getDirection()
        {
            T3D.Point start = __GeometryOperations.factor1Point((_part as TSM.Beam).StartPoint, _view as TSD.View);
            T3D.Point end = __GeometryOperations.factor1Point((_part as TSM.Beam).EndPoint, _view as TSD.View);

            T3D.Vector vector = __GeometryOperations.getDirectionVector(start, end);

            return vector;
        }

        public override void tryPredict<T>(List<T> others)
        {
            T match = null;

            List<T> sameLineOthers = findSameLineMark(others);
            match = findClosestMark(sameLineOthers);

            if (match == null)
            {
                match = findClosestMark(others);
            }

            if (match != null)
            {
                this.reCreateMarkSmarts(match);
            }
        }

        private List<T> findSameLineMark<T>(List<T> others) where T : _Mark
        {
            List<T> sameLine = new List<T>();

            foreach (T other in others)
            {
                T3D.Point inputStart = __GeometryOperations.factor1Point((other._part as TSM.Beam).StartPoint, other._view as TSD.View);
                inputStart = __GeometryOperations.applyGlobalOffset(inputStart);
                T3D.Point outputStart = __GeometryOperations.factor1Point((this._part as TSM.Beam).StartPoint, this._view as TSD.View);
                T3D.Point outputEnd = __GeometryOperations.factor1Point((this._part as TSM.Beam).EndPoint, this._view as TSD.View);

                bool same = __GeometryOperations.areOnSameLine(outputStart, outputEnd, inputStart);
                
                if (same)
                {
                    sameLine.Add(other);
                }

            }

            return sameLine;
        }

        private T findClosestMark<T>(List<T> others) where T : _Mark
        {
            T match = null;
            double min = double.MaxValue;

            foreach (T other in others)
            {
                T3D.Point inputStart = __GeometryOperations.factor1Point((other._part as TSM.Beam).StartPoint, other._view as TSD.View);
                inputStart = __GeometryOperations.applyGlobalOffset(inputStart);
                T3D.Point inputEnd = __GeometryOperations.factor1Point((other._part as TSM.Beam).EndPoint, other._view as TSD.View);
                inputEnd = __GeometryOperations.applyGlobalOffset(inputStart);

                T3D.Point outputStart = __GeometryOperations.factor1Point((this._part as TSM.Beam).StartPoint, this._view as TSD.View);
                T3D.Point outputEnd = __GeometryOperations.factor1Point((this._part as TSM.Beam).EndPoint, this._view as TSD.View);

                double dist = 0;
                dist += __GeometryOperations.getLength(inputStart, outputStart);
                dist += __GeometryOperations.getLength(inputEnd, outputEnd);

                if (dist < min)
                {
                    match = other;
                    min = dist;
                }
            }

            return match;
        }

        private void reCreateMarkSmarts(_Mark other)
        {
            _mark.Attributes = other._mark.Attributes;
            _mark.Attributes.Frame.Color = TSD.DrawingColors.Green;

            T3D.Point inputStart = __GeometryOperations.factor1Point((other._part as TSM.Beam).StartPoint, other._view as TSD.View);
            inputStart = __GeometryOperations.applyGlobalOffset(inputStart);
            T3D.Point inputEnd = __GeometryOperations.factor1Point((other._part as TSM.Beam).EndPoint, other._view as TSD.View);
            inputEnd = __GeometryOperations.applyGlobalOffset(inputEnd);

            T3D.Point outputStart = __GeometryOperations.factor1Point((this._part as TSM.Beam).StartPoint, this._view as TSD.View);
            T3D.Point outputEnd = __GeometryOperations.factor1Point((this._part as TSM.Beam).EndPoint, this._view as TSD.View);

            T3D.Point markOffset = __GeometryOperations.getLocalOffset(inputStart, outputStart);
            double alfa = __GeometryOperations.getAlfa(inputStart, inputEnd, outputStart, outputEnd);

            T3D.Point insertion = __GeometryOperations.applyGlobalOffset(other._mark.InsertionPoint);
            insertion = __GeometryOperations.applyLocalOffset(insertion, markOffset);

            if (other._mark.Placing is TSD.LeaderLinePlacing)
            {
                TSD.LeaderLinePlacing attr = other._mark.Placing as TSD.LeaderLinePlacing;

                T3D.Point start = __GeometryOperations.applyGlobalOffset(attr.StartPoint);
                start = __GeometryOperations.applyLocalOffset(start, markOffset);
                T3D.Point placingOffset = __GeometryOperations.getPlacingOffset(start, outputStart, alfa);
                T3D.Point insertionOffset = __GeometryOperations.getLocalOffset(start, insertion);
                start = __GeometryOperations.applyLocalOffset(outputStart, placingOffset);
                insertion = __GeometryOperations.applyLocalOffset(start, insertionOffset);

                TSD.LeaderLinePlacing newPlacing = new TSD.LeaderLinePlacing(start);
                _mark.Placing = newPlacing;
            }

            else if (other._mark.Placing is TSD.AlongLinePlacing)
            {
                TSD.AlongLinePlacing attr = other._mark.Placing as TSD.AlongLinePlacing;

                T3D.Point start = __GeometryOperations.applyGlobalOffset(attr.StartPoint);
                start = __GeometryOperations.applyLocalOffset(start, markOffset);
                T3D.Point placingOffset = __GeometryOperations.getPlacingOffset(outputStart, start, alfa);
                T3D.Point insertionOffset = __GeometryOperations.getLocalOffset(start, insertion);
                start = __GeometryOperations.applyLocalOffset(start, placingOffset);
                insertion = __GeometryOperations.applyLocalOffset(start, insertionOffset);

                T3D.Point end = __GeometryOperations.applyGlobalOffset(attr.EndPoint);
                end = __GeometryOperations.applyLocalOffset(end, markOffset);
                end = __GeometryOperations.applyLocalOffset(end, placingOffset);
                
                TSD.AlongLinePlacing newPlacing = new TSD.AlongLinePlacing(start, end);
                _mark.Placing = newPlacing;
            }

            else if (other._mark.Placing is TSD.BaseLinePlacing)
            {
                //TSD.BaseLinePlacing attr = input._mark.Placing as TSD.BaseLinePlacing;

                //T3D.Point start = __GeometryOperations.applyGlobalOffset(attr.StartPoint);
                //start = __GeometryOperations.applyLocalOffset(start, markOffset);
                //T3D.Point placingOffset = __GeometryOperations.getPlacingOffset(startPointRefactor, start, alfa);
                //T3D.Point insertionOffset = __GeometryOperations.getLocalOffset(start, insertion);
                //start = __GeometryOperations.applyLocalOffset(start, placingOffset);
                //insertion = __GeometryOperations.applyLocalOffset(start, insertionOffset);

                //T3D.Point end = __GeometryOperations.applyGlobalOffset(attr.EndPoint);
                //end = __GeometryOperations.applyLocalOffset(end, markOffset);
                //end = __GeometryOperations.applyLocalOffset(end, placingOffset);

                //TSD.BaseLinePlacing newPlacing = new TSD.BaseLinePlacing(start, end);
                //_mark.Placing = newPlacing;
            }

            else if (other._mark.Placing is TSD.PointPlacing)
            {
                TSD.PointPlacing newPlacing = new TSD.PointPlacing();
                _mark.Placing = newPlacing;
                
            }

            _mark.InsertionPoint = insertion;

            _mark.Modify();
            _mark.Modify();
        }

        public override bool checkModelObjects(_Mark other)
        {
            TSM.Beam part1 = _part as TSM.Beam;
            TSM.Beam part2 = other._part as TSM.Beam;

            T3D.Point p1start = __GeometryOperations.factor1Point(part1.StartPoint, _view as TSD.View);
            T3D.Point p1end = __GeometryOperations.factor1Point(part1.EndPoint, _view as TSD.View);
            T3D.Point p2start = __GeometryOperations.factor1Point(part2.StartPoint, other._view as TSD.View);
            T3D.Point p2end = __GeometryOperations.factor1Point(part2.EndPoint, other._view as TSD.View);

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

