using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;

namespace project_centrum
{
    public static class UserProperties
    {
        public static bool _view;
        public static bool _mark;
        public static bool _section;
        public static bool _detail;
        public static bool _dim;
        public static bool _line;
        public static bool _txt;
        public static bool _red;
        public static bool _predict;

        public static double _dR;

        public static T3D.Point a = null;
        public static T3D.Point b = null;

        public static void set(bool view, bool mark, bool txt,
                               bool section, bool detail, bool dim, bool line,
                               bool red, bool predict, double deg)
        {
            _view = view;
            _mark = mark;
            _section = section;
            _detail = detail;
            _dim = dim;
            _line = line;
            _txt = txt;
            _red = red;
            _predict = predict;

            _dR = deg;
        }

        public static void setTag1(T3D.Point start)
        {
            a = start;
        }

        public static void setTag2(T3D.Point end)
        {
            b = end;
        }
    }
}

