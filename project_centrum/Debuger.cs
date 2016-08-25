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
    class Debuger
    {
        public static void p(string txt)

        {
            Form1._form.add_text(txt);
        }

        public static void ppoint(T3D.Point pp, string txt)

        {
            Form1._form.add_text(txt + " : " + pp.X.ToString("F1") + " ; " + pp.Y.ToString("F1") + " ; " + pp.Z.ToString("F1"));
        }

        public static void pvector(T3D.Vector pp, string txt)
        {
            Form1._form.add_text(txt + " : " + pp.X.ToString("F1") + " ; " + pp.Y.ToString("F1") + " ; " + pp.Z.ToString("F1"));
        }

        public static void pcor(T3D.CoordinateSystem pc, string txt)
        {
            pvector(pc.AxisX, txt + " Axis X");
            pvector(pc.AxisY, txt + " Axis Y");
            ppoint(pc.Origin, txt + " Origin");
        }

        public static void pcv(TSD.ContainerView vv)
        {
            Form1._form.add_text("---  SHEET  ---");
            ppoint(vv.ExtremaCenter, "ExtremaCenter");
            pvector(vv.FrameOrigin, "FrameOrigin");

            Form1._form.add_text("IsSheet : " + vv.IsSheet.ToString());
            ppoint(vv.Origin, "Origin");

            Form1._form.add_text("Height : " + vv.Height.ToString("F1"));
            Form1._form.add_text("Width : " + vv.Width.ToString("F1"));
            Form1._form.add_text("");
        }

        public static void pview(TSD.View vv)
        {
            Form1._form.add_text("---  VIEW  ---");
            pcor(vv.DisplayCoordinateSystem, "DisplayCoordinateSystem");
            pcor(vv.ViewCoordinateSystem, "ViewCoordinateSystem");

            ppoint(vv.ExtremaCenter, "ExtremaCenter");
            pvector(vv.FrameOrigin, "FrameOrigin");

            Form1._form.add_text("IsSheet : " + vv.IsSheet.ToString());
            Form1._form.add_text("Name : " + vv.Name);
            ppoint(vv.Origin, "Origin");

            Form1._form.add_text("Height : " + vv.Height.ToString("F1"));
            Form1._form.add_text("Width : " + vv.Width.ToString("F1"));
            Form1._form.add_text("");
        }


    }
}
