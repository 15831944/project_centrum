using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_centrum
{
    public static class UserProperties
    {
        public static bool _view;
        public static bool _mark;
        public static bool _mark_attr;
        public static bool _section;
        public static bool _detail;
        public static bool _dim;
        public static bool _line;
        public static bool _txt;
        public static bool _red;

        public static void set(bool view, bool mark, bool txt,
                                          bool mark_attr,
                               bool section, bool detail, bool dim, bool line,
                               bool red)
        {
            _view = view;
            _mark = mark;
            _mark_attr = mark_attr;
            _section = section;
            _detail = detail;
            _dim = dim;
            _line = line;
            _txt = txt;
            _red = red;
        }

    }
}

