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
    class TeklaGetter
    {
        public static __DrawingData getAllData()
        {
            __DrawingData drawing = new __DrawingData();
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.ContainerView sheet = drawingHandler.GetActiveDrawing().GetSheet();
                drawing.setSheet(sheet);
                TSD.DrawingObjectEnumerator views = sheet.GetAllViews();
                drawing.setViews(views);
                TSD.DrawingObjectEnumerator allObjects = sheet.GetAllObjects();
                drawing.populate(allObjects);
            }
            else
            {
                throw new DivideByZeroException();
            }

            return drawing;
        }

        public static void getPoint1()
        {
            __DrawingData drawing = new __DrawingData();
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.UI.Picker picker = drawingHandler.GetPicker();
                T3D.Point point = null;
                TSD.ViewBase vv = null;


                picker.PickPoint("PICK", out point, out vv);
                Form1._form.add_text(Environment.NewLine + point.X.ToString() + " " + point.Y.ToString());
                UserProperties.setTag1(point);
            }
        }

        public static void getPoint2()
        {
            __DrawingData drawing = new __DrawingData();
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.UI.Picker picker = drawingHandler.GetPicker();
                T3D.Point point = null;
                TSD.ViewBase vv = null;


                picker.PickPoint("PICK", out point, out vv);
                Form1._form.add_text(Environment.NewLine + point.X.ToString() + " " + point.Y.ToString());
                UserProperties.setTag2(point);
            }
        }

        public static __DrawingData getSelectedData()
        {
            __DrawingData drawing = new __DrawingData();
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.ContainerView sheet = drawingHandler.GetActiveDrawing().GetSheet();
                drawing.setSheet(sheet);

                TSD.DrawingObjectEnumerator selectedObjects = drawingHandler.GetDrawingObjectSelector().GetSelected();
                drawing.setSelectedViews(selectedObjects);
                drawing.populateSelected(selectedObjects);
            }
            else
            {
                throw new DivideByZeroException();
            }

            return drawing;
        }

        public static __DrawingData getEmptyData()
        {
            __DrawingData drawing = new __DrawingData();
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.ContainerView sheet = drawingHandler.GetActiveDrawing().GetSheet();
                drawing.setSheet(sheet);
                TSD.DrawingObjectEnumerator views = sheet.GetAllViews();
                drawing.setViews(views);
            }
            else
            {
                throw new DivideByZeroException();
            }

            return drawing;
        }
    }
}