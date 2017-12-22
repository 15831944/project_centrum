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

                try
                {
                    getPoint(drawing);
                }
                catch
                {
                    drawing.setSheet(sheet);
                }
                

                List<Type> types = new List<Type>();

                if (UserProperties._mark) types.Add(typeof(TSD.Mark));
                if (UserProperties._section) types.Add(typeof(TSD.SectionMark));
                if (UserProperties._detail) types.Add(typeof(TSD.DetailMark));
                if (UserProperties._line) types.Add(typeof(TSD.Line));
                if (UserProperties._dim) types.Add(typeof(TSD.StraightDimensionSet));
                if (UserProperties._txt) types.Add(typeof(TSD.TextFile));
                if (UserProperties._dwg) types.Add(typeof(TSD.DwgObject));

                if (types.Count != 0)
                {
                    System.Type[] Types = new System.Type[types.Count];

                    for (int i = 0; i < types.Count; i++)
                    {
                        Types.SetValue(types[i], i);
                    }

                    TSD.DrawingObjectEnumerator allObjects = sheet.GetAllObjects(Types);
                    drawing.populate(allObjects);
                }

            }
        
            return drawing;
        }


        public static __DrawingData getSelectedData()
        {
            __DrawingData drawing = new __DrawingData();

            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.ContainerView sheet = drawingHandler.GetActiveDrawing().GetSheet();

                try
                {
                    getPoint(drawing);
                }
                catch
                {
                    drawing.setSheet(sheet);
                }

                TSD.DrawingObjectEnumerator selectedObjects = drawingHandler.GetDrawingObjectSelector().GetSelected();
                drawing.populate(selectedObjects);
            }

            return drawing;
        }


        private static void getPoint(__DrawingData drawing)
        {
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                TSD.ContainerView sheet = drawingHandler.GetActiveDrawing().GetSheet();

                TSD.UI.Picker picker = drawingHandler.GetPicker();
                T3D.Point viewPoint = null;
                TSD.ViewBase selectedView = null;

                MainForm._form.add_text("Select origin point in drawing view");
                picker.PickPoint("Pick one point", out viewPoint, out selectedView);
                T3D.Point sheetPoint = TSD.Tools.DrawingCoordinateConverter.Convert(selectedView, sheet, viewPoint);

                if (selectedView != null)
                {
                    if (selectedView is TSD.View)
                    {
                        drawing.setView(selectedView as TSD.View);
                    }
                }

                drawing.setSheet(sheet);
                drawing.setPoints(viewPoint, sheetPoint);
            }
        }

    }
}