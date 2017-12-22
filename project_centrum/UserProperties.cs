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
        public static bool _dwg;

        public static bool _red;

        public static double _dR;

        public static bool _predict;

        public static T3D.Point viewInputPoint = null;
        public static T3D.Point viewOutputPoint = null;

        public static T3D.Point sheetInputPoint = null;
        public static T3D.Point sheetOutputPoint = null;

        public static void set(bool view, bool mark, 
                               bool section, bool detail, bool line, bool dim,
                               bool txt, bool dwg,
                               bool red, 
                               double deg)
        {
            _view = view;
            _mark = mark;

            _section = section;
            _detail = detail;
            _dim = dim;
            _line = line;

            _txt = txt;
            _dwg = dwg;

            _red = red;

            _predict = false;

            _dR = deg;
        }

        public static void setInputPoints(T3D.Point start, T3D.Point sheet)
        {
            viewInputPoint = start;
            sheetInputPoint = sheet;
        }

        public static void setOutputPoints(T3D.Point end, T3D.Point sheet)
        {
            viewOutputPoint = end;
            sheetOutputPoint = sheet;
        }
    }
}

