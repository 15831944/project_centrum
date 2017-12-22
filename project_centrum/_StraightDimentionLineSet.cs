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
    class _StraightDimentionSet
    {
        public TSD.StraightDimensionSet _set;
        public TSD.StraightDimension _first;
        public TSD.PointList _points;


        public _StraightDimentionSet(TSD.StraightDimensionSet obj)
        {
            _set = obj as TSD.StraightDimensionSet;
            _points = new TSD.PointList();
            getDimSetPoints();
        }


        internal void getDimSetPoints()
        {
            TSD.DrawingObjectEnumerator dima = _set.GetObjects();

            while (dima.MoveNext())
            {
                TSD.StraightDimension line = dima.Current as TSD.StraightDimension;

                if (_first == null)
                {
                    _first = line;
                }

                addUniquePoints(line.StartPoint);
                addUniquePoints(line.EndPoint);
            }
        }


        private void addUniquePoints(T3D.Point point)
        {
            if (! _points.Contains(point))
            {
                _points.Add(point);
            }
        }

    }
}
