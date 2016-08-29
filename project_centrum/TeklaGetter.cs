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

                int i = 0;
                if (UserProperties._mark) i++;
                if (UserProperties._dim) i++;
                if (UserProperties._section) i++;
                if (UserProperties._detail) i++;
                if (UserProperties._line) i++;
                if (UserProperties._txt) i++;

                System.Type[] Types = new System.Type[i];

                i = 0;

                if (UserProperties._mark)
                {
                    Types.SetValue(typeof(TSD.Mark), i);
                    i++;
                }

                if (UserProperties._dim)
                {
                    Types.SetValue(typeof(TSD.StraightDimensionSet), i);
                    i++;
                }
                if (UserProperties._section)
                {
                    Types.SetValue(typeof(TSD.SectionMark), i);
                    i++;
                }

                if (UserProperties._detail)
                {
                    Types.SetValue(typeof(TSD.DetailMark), i);
                    i++;
                }

                if (UserProperties._line)
                {
                    Types.SetValue(typeof(TSD.Line), i);
                    i++;
                }

                if (UserProperties._txt)
                {
                    Types.SetValue(typeof(TSD.TextFile), i);
                    i++;
                }

                TSD.DrawingObjectEnumerator allObjects = sheet.GetAllObjects(Types);
                drawing.populate(allObjects);
            }
            else
            {
                throw new DivideByZeroException();
            }

            return drawing;
        }

        public static void getPoint(Action<T3D.Point> setter)
        {
            __DrawingData drawing = new __DrawingData();
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.UI.Picker picker = drawingHandler.GetPicker();
                T3D.Point point = null;
                TSD.ViewBase vv = null;

                Form1._form.add_text("Select origin point in drawing view");
                picker.PickPoint("Pick one point", out point, out vv);
                setter(point);
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
    }
}