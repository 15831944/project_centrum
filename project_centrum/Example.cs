using Tekla.Structures.Geometry3d;
using Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

class Example
{
    public void InsertMarksForAllPartsInDrawing()
    {
        TSM.Model MyModel = new TSM.Model();
        DrawingHandler MyDrawingHandler = new DrawingHandler();
        if(MyModel.GetConnectionStatus())
        {
            if(MyDrawingHandler.GetConnectionStatus())
            {
                Drawing CurrentDrawing = MyDrawingHandler.GetActiveDrawing(); //
                DrawingObjectEnumerator allParts = CurrentDrawing.GetSheet().GetAllObjects(typeof(Part)); //
                while(allParts.MoveNext())
                {
                    ModelObject modelObject = (ModelObject)allParts.Current;

                    Point PartMiddleStart = null, PartMiddleEnd = null, PartCenterPoint = null;
                    GetPartPoints(MyModel, modelObject.GetView(), modelObject, out PartMiddleStart, out PartMiddleEnd, out PartCenterPoint);

                    Mark Mark = new Mark(modelObject);
                    Mark.Attributes.Content.Clear();
                    Mark.Attributes.Content.Add(new TextElement("My Mark"));
                    Mark.Placing = new AlongLinePlacing(PartMiddleStart, PartMiddleEnd);
                    Mark.InsertionPoint = PartCenterPoint;
                    Mark.Insert();
                }
            }
        }
    }

    private void GetPartPoints(TSM.Model MyModel, ViewBase PartView, ModelObject modelObject, out Point PartMiddleStart, out Point PartMiddleEnd, out Point PartCenterPoint)
    {
        TSM.ModelObject modelPart = GetModelObjectFromDrawingModelObject(MyModel, modelObject);
        GetModelObjectStartAndEndPoint(modelPart, (View)PartView, out PartMiddleStart, out PartMiddleEnd);
        PartCenterPoint = GetInsertionPoint(PartMiddleStart, PartMiddleEnd);
    }

    private TSM.ModelObject GetModelObjectFromDrawingModelObject(TSM.Model MyModel, ModelObject PartOfMark)
    {
        TSM.ModelObject modelObject = MyModel.SelectModelObject(PartOfMark.ModelIdentifier);

        TSM.Part modelPart = (TSM.Part)modelObject;

        return modelPart;
    }

    private void GetModelObjectStartAndEndPoint(TSM.ModelObject modelObject, View PartView, out Point PartStartPoint, out Point PartEndPoint)
    {
        TSM.Part modelPart = (TSM.Part)modelObject;

        PartStartPoint = modelPart.GetSolid().MinimumPoint;
        PartEndPoint = modelPart.GetSolid().MaximumPoint;

        Matrix convMatrix = MatrixFactory.ToCoordinateSystem(PartView.DisplayCoordinateSystem);
        PartStartPoint = convMatrix.Transform(PartStartPoint);
        PartEndPoint = convMatrix.Transform(PartEndPoint);
    }

    private Point GetInsertionPoint(Point PartStartPoint, Point PartEndPoint)
    {
        Point MinPoint = PartStartPoint;
        Point MaxPoint = PartEndPoint;
        Point InsertionPoint = new Point((MaxPoint.X + MinPoint.X) * 0.5, (MaxPoint.Y + MinPoint.Y) * 0.5, (MaxPoint.Z + MinPoint.Z) * 0.5);
        InsertionPoint.Z = 0;
        return InsertionPoint;
    }
}
 