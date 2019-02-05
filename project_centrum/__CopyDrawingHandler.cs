using System;
using System.Collections;
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
    class __CopyDrawingHandler
    {
        public static void main(__DrawingData input, __DrawingData output)
        {
            __GeometryOperations.setInputPoints(input.viewPoint, input.sheetPoint);
            __GeometryOperations.setOutputPoints(output.viewPoint, output.sheetPoint);

            if (input.sheet != null && output.sheet != null)
            {
                redraw(input.data[input.sheet], output.data[output.sheet]);
            }

            if (input.view != null && output.view != null)
            {
                redraw(input.data[input.view], output.data[output.view]);
            }
        }


        public static void redraw(__ViewBaseData input, __ViewBaseData output)
        {
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                if (output.view is TSD.View)
                {
                    if (UserProperties._view)
                    {
                        if (input.view != null && output.view != null)
                        {
                            repositionViewWithOffset(input.view as TSD.View, output.view as TSD.View);
                        }
                    }

                    if (UserProperties._mark || UserProperties._red || UserProperties._predict)
                    {
                        handleMarks<_Mark_Beam>(input.markBeams, output.markBeams);
                        handleMarks<_Mark_PolyBeam>(input.markPolyBeams, output.markPolyBeams);
                        handleMarks<_Mark_ContourPlate>(input.markContourPlates, output.markContourPlates);

                        handleMarks<_Mark_BoltGroup>(input.markBoltGroup, output.markBoltGroup);

                        handleMarks<_Mark_SingleRebar>(input.markSingleRebars, output.markSingleRebars);
                        handleMarks<_Mark_RebarGroup>(input.markRebarBases, output.markRebarBases);
                    }

                    if (UserProperties._section)
                    {
                        createSectionMarks(input.sectionMarks, output.sectionMarks, output.view as TSD.View);
                    }

                    if (UserProperties._detail)
                    {
                        createDetailMarks(input.detailMarks, output.detailMarks, output.view as TSD.View);
                    }

                    if (UserProperties._txt)
                    {
                        createText(input.txt, output.txt, input, output);
                    }

                    if (UserProperties._dim)
                    {
                        createDimentionLineSets(input.straightDimSets, output.straightDimSets, output.view);
                    }

                    if (UserProperties._line)
                    {
                        //createArcs(input.arcs, output.arcs, input.view, output.view); // PUUDUB PIISAV INFO KAARE TAASLOOMISEKS
                        createLines(input.lines, output.lines, input.view, output.view);
                        createPolylines(input.polylines, output.polylines, input.view, output.view);

                        createCircles(input.circles, output.circles, input.view, output.view);
                        createClouds(input.clouds, output.clouds, input.view, output.view);
                        createRectangles(input.rectangles, output.rectangles, input.view, output.view);
                        createPolygons(input.polygons, output.polygons, input.view, output.view);
                    }
                }
                else
                {
                    if (UserProperties._line)
                    {
                        createLinesNoOffset(input.lines, output.lines, input.view, output.view);
                    }
                }
                
                if (UserProperties._txtfile)
                {
                    repositionTextFile(input.txtFiles, output.txtFiles, input, output);
                }

                if (UserProperties._dwg)
                {
                    repositionDwgFile(input.dwgRefs, output.dwgRefs, input, output);
                }

                output.view.Modify();
            }
        }


        private static void repositionViewWithOffset(TSD.View input, TSD.View output)
        {
            T3D.Point minus = __GeometryOperations.getLocalOffset(__GeometryOperations.sheetOutputPoint, __GeometryOperations.sheetInputPoint);
            T3D.Point localOffset = __GeometryOperations.applyLocalOffset(output.Origin, minus);
            output.Origin = localOffset;
        }


        private static void handleMarks<T>(List<T> input, List<T> output) where T : _Mark
        {
            List<T> notFound = new List<T>();
            List<T> notFoundInput = new List<T>();
            Dictionary<T, T> matches = __MatchMaker.matchMarks(input, output, out notFound, out notFoundInput);

            if (UserProperties._mark)
            {
                repositionMarks<T>(matches);
            }

            if (UserProperties._red)
            {
                setNotFoundRed<T>(notFound);
            }

            if (UserProperties._predict)
            {
                tryPredictMarks<T>(notFound, input);
            }

            if (notFound.Count > 0 || notFoundInput.Count > 0)
            {
                Debuger.p("");
                Debuger.p("[MARKS] Output not found: " + notFound.Count.ToString());
                Debuger.p("[MARKS] Input not found: " + notFoundInput.Count.ToString());
                Debuger.p("");
            }

        }


        private static void repositionMarks<T>(Dictionary<T, T> matches) where T : _Mark
        {
            foreach (T inputKey in matches.Keys)
            {
                matches[inputKey].reCreateMark(inputKey);
            }
        }


        private static void setNotFoundRed<T>(List<T> notFound) where T : _Mark
        {
            foreach (T output in notFound)
            {
                output.applyRedBorder();
            }
        }


        private static void tryPredictMarks<T>(List<T> notFound, List<T> input) where T : _Mark
        {
            foreach (T output in notFound)
            {
                T3D.Vector outputvector = output.getDirection();
                List<T> sameDirectionMarks = input.Where(x => x.getDirectionOther() == outputvector).ToList();
                output.tryPredict<T>(sameDirectionMarks);
            }
        }


        private static void createSectionMarks(List<_SectionMark> input, List<_SectionMark> output, TSD.View outputView)
        {
            foreach (_SectionMark inputSection in input)
            {
                T3D.Point leftPoint = __GeometryOperations.applyGlobalOffset(inputSection._obj.LeftPoint);
                T3D.Point rightPoint = __GeometryOperations.applyGlobalOffset(inputSection._obj.RightPoint);

                bool found = false;
                foreach (_SectionMark outputSectionMark in output)
                {
                    if (outputSectionMark._obj.LeftPoint == leftPoint &&
                        outputSectionMark._obj.RightPoint == rightPoint)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.SectionMark outputSectionMark = new TSD.SectionMark(outputView, leftPoint, rightPoint);
                    outputSectionMark.Attributes = inputSection._obj.Attributes;

                    //if (inputSection._txt != null)
                    //{
                    //    outputSectionMark.Attributes.TagsAttributes.TagA2.TagContent.Clear();
                    //    outputSectionMark.Attributes.TagsAttributes.TagA2.TagContent.Add(inputSection._txt);
                    //    outputSectionMark.Modify();
                    //}

                    outputSectionMark.Insert();
                }
            }
        }


        private static void createDetailMarks(List<TSD.DetailMark> input, List<TSD.DetailMark> output, TSD.View outputView)
        {
            foreach (TSD.DetailMark inputDetail in input)
            {
                T3D.Point centerPoint = __GeometryOperations.applyGlobalOffset(inputDetail.CenterPoint);
                T3D.Point boundaryPoint = __GeometryOperations.applyGlobalOffset(inputDetail.BoundaryPoint);
                T3D.Point labelPoint = __GeometryOperations.applyGlobalOffset(inputDetail.LabelPoint);

                bool found = false;
                foreach (TSD.DetailMark outputDetailMark in output)
                {
                    if (outputDetailMark.CenterPoint == centerPoint &&
                        outputDetailMark.BoundaryPoint == boundaryPoint &&
                        outputDetailMark.LabelPoint == labelPoint)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.DetailMark outputDetailMark = new TSD.DetailMark(outputView, centerPoint, boundaryPoint, labelPoint);
                    outputDetailMark.Attributes = inputDetail.Attributes;
                    outputDetailMark.Insert();
                }
            }
        }


        private static void repositionTextFile(List<TSD.TextFile> input, List<TSD.TextFile> output, __ViewBaseData inputData, __ViewBaseData outputData)
        {
            foreach (TSD.TextFile inputTextFile in input)
            {
                bool found = false;

                foreach (TSD.TextFile outputTextFile in output)
                {
                    if (inputTextFile.FileName == outputTextFile.FileName)
                    {
                        outputTextFile.InsertionPoint = inputTextFile.InsertionPoint;
                        outputTextFile.Attributes = inputTextFile.Attributes;
                        outputTextFile.Size = inputTextFile.Size;
                        outputTextFile.Modify();
                        Debuger.p("TextFile -> No Scaling");

                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.TextFile outputTextFile = new TSD.TextFile(outputData.view, inputTextFile.InsertionPoint, inputTextFile.FileName);
                    outputTextFile.Attributes = inputTextFile.Attributes;
                    outputTextFile.FileName = inputTextFile.FileName;
                    outputTextFile.Insert();
                }
            }
        }


        private static void repositionDwgFile(List<TSD.DwgObject> input, List<TSD.DwgObject> output, __ViewBaseData inputData, __ViewBaseData outputData)
        {
            foreach (TSD.DwgObject inputDwgFile in input)
            {
                bool found = false;

                foreach (TSD.DwgObject outputDwgFile in output)
                {
                    if (inputDwgFile.FileName == outputDwgFile.FileName)
                    {
                        outputDwgFile.InsertionPoint = inputDwgFile.InsertionPoint;
                        outputDwgFile.Attributes = inputDwgFile.Attributes;
                        outputDwgFile.Size = inputDwgFile.Size;
                        outputDwgFile.Modify();

                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.DwgObject outputDwgFile = new TSD.DwgObject(outputData.view, inputDwgFile.InsertionPoint, inputDwgFile.FileName);
                    outputDwgFile.Attributes = inputDwgFile.Attributes;
                    outputDwgFile.FileName = inputDwgFile.FileName;
                    outputDwgFile.Insert();
                }
            }
        }


        private static void createText(List<TSD.Text> input, List<TSD.Text> output, __ViewBaseData inputData, __ViewBaseData outputData)
        {
            foreach (TSD.Text inputText in input)
            {
                T3D.Point insertionPoint = __GeometryOperations.applyGlobalOffset(inputText.InsertionPoint);

                bool found = false;

                //foreach (TSD.Text outputText in output)
                //{
                //    if (outputText.InsertionPoint == insertionPoint && outputText.TextString == inputText.TextString)
                //    {
                //        found = true;
                //        break;
                //    }
                //}

                if (found == false)
                {
                    TSD.Text outputText = new TSD.Text(outputData.view, insertionPoint, inputText.TextString);
                    outputText.Placing = inputText.Placing;
                    outputText.Attributes = inputText.Attributes;
                    outputText.Insert();
                }
            }
        }


        private static void createDimentionLineSets(List<_StraightDimentionSet> input, List<_StraightDimentionSet> output, TSD.ViewBase outputView)
        {
            foreach (_StraightDimentionSet inputDimSet in input)
            {
                TSD.PointList outputPoints = new TSD.PointList();
                foreach (T3D.Point ip in inputDimSet._points)
                {
                    T3D.Point op = __GeometryOperations.applyGlobalOffset(ip);
                    outputPoints.Add(op);
                }

                bool found = false;
                _StraightDimentionSet outputDimSetCopy = null;

                foreach (_StraightDimentionSet outputDimSet in output)
                {
                    if (inputDimSet._points.Count == outputDimSet._points.Count)
                    {
                        if (outputDimSet._points.Count != 0)
                        {
                            bool local = true;
                            for (int i = 0; i < outputDimSet._points.Count; i++)
                            {
                                bool same = __GeometryOperations.compare2PointsNullTolerance(outputPoints[i], outputDimSet._points[i]);

                                if (same == false)
                                {
                                    local = false;
                                    break;
                                }
                            }

                            if (local == true)
                            {
                                if (outputDimSet._first.UpDirection == inputDimSet._first.UpDirection)
                                {
                                    found = true;
                                    outputDimSetCopy = outputDimSet;
                                    break;
                                }
                            }
                        }
                    }

                }

                if (found == false)
                {
                    TSD.StraightDimensionSetHandler sds = new TSD.StraightDimensionSetHandler();
                    TSD.StraightDimensionSet outputDimSet = sds.CreateDimensionSet(outputView, outputPoints, inputDimSet._first.UpDirection, inputDimSet._set.Distance, inputDimSet._set.Attributes);
                    outputDimSet.Distance = inputDimSet._set.Distance;
                    outputDimSet.Modify();
                }
                else
                {
                    if (outputDimSetCopy != null)
                    {
                        outputDimSetCopy._set.Distance = inputDimSet._set.Distance;
                        outputDimSetCopy._set.Modify();
                    }
                }
            }
        }


        //private static void createArcs(List<TSD.Arc> input, List<TSD.Arc> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        //{
        //    foreach (TSD.Arc inputArc in input)
        //    {
        //        T3D.Point startPoint = __GeometryOperations.applyGlobalOffset(inputArc.StartPoint);
        //        T3D.Point endPoint = __GeometryOperations.applyGlobalOffset(inputArc.EndPoint);

        //        bool found = false;
        //        foreach (TSD.Arc outputArc in output)
        //        {
        //            if (outputArc.StartPoint == startPoint && outputArc.EndPoint == endPoint)
        //            {
        //                found = true;
        //                break;
        //            }
        //            else if (outputArc.StartPoint == endPoint && outputArc.EndPoint == startPoint)
        //            {
        //                found = true;
        //                break;
        //            }
        //        }

        //        if (found == false)
        //        {
        //            TSD.Arc outputLine = new TSD.Arc(outputView, startPoint, endPoint, inputArc.Radius, inputArc.Attributes);
        //            outputLine.Attributes = inputArc.Attributes;
        //            outputLine.Insert();
        //        }

        //    }
        //}


        private static void createLines(List<TSD.Line> input, List<TSD.Line> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Line inputLine in input)
            {
                T3D.Point startPoint = __GeometryOperations.applyGlobalOffset(inputLine.StartPoint);
                T3D.Point endPoint = __GeometryOperations.applyGlobalOffset(inputLine.EndPoint);

                bool found = false;
                foreach (TSD.Line outputLine in output)
                {
                    if (outputLine.StartPoint == startPoint && outputLine.EndPoint == endPoint)
                    {
                        found = true;
                        break;
                    }
                    else if (outputLine.StartPoint == endPoint && outputLine.EndPoint == startPoint)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.Line outputLine = new TSD.Line(outputView, startPoint, endPoint, inputLine.Attributes);
                    outputLine.Attributes = inputLine.Attributes;
                    outputLine.Insert();
                }

            }
        }


        private static void createPolylines(List<TSD.Polyline> input, List<TSD.Polyline> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Polyline inputPoly in input)
            {
                TSD.PointList pts = new TSD.PointList();

                foreach (T3D.Point pt in inputPoly.Points)
                {
                    T3D.Point temp = __GeometryOperations.applyGlobalOffset(pt);
                    pts.Add(temp);
                }

                bool found = false;
                bool same = true;
                foreach (TSD.Polyline outputPoly in output)
                {
                    if (outputPoly.Points.Count == pts.Count)
                    {
                        if (outputPoly.Points.Count != 0)
                        {
                            for (int i = 0; i < outputPoly.Points.Count; i++)
                            {
                                T3D.Point oldpt = outputPoly.Points[0];
                                T3D.Point newpt = pts[0];

                                if (__GeometryOperations.compare2Points(oldpt, newpt) == false)
                                {
                                    same = false;
                                    break;
                                }
                            }

                            if (same == true)
                            {
                                found = true;
                                break;
                            }

                            for (int i = 0; i < outputPoly.Points.Count; i++)
                            {
                                T3D.Point oldpt = outputPoly.Points[0];
                                T3D.Point newpt = pts[pts.Count - i - 1];

                                if (__GeometryOperations.compare2Points(oldpt, newpt) == false)
                                {
                                    same = false;
                                    break;
                                }
                            }

                            if (same == true)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (found == false)
                {
                    TSD.Polyline outputLine = new TSD.Polyline(outputView, pts, inputPoly.Attributes);
                    outputLine.Attributes = inputPoly.Attributes;
                    outputLine.Insert();
                }
            }
        }


        private static void createCircles(List<TSD.Circle> input, List<TSD.Circle> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Circle inputCircle in input)
            {
                T3D.Point centerPoint = __GeometryOperations.applyGlobalOffset(inputCircle.CenterPoint);

                bool found = false;
                foreach (TSD.Circle outputCircle in output)
                {
                    if (outputCircle.CenterPoint == centerPoint && outputCircle.Radius == inputCircle.Radius)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.Circle outputLine = new TSD.Circle(outputView, centerPoint, inputCircle.Radius, inputCircle.Attributes);
                    outputLine.Attributes = inputCircle.Attributes;
                    outputLine.Insert();
                }

            }
        }


        private static void createClouds(List<TSD.Cloud> input, List<TSD.Cloud> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Cloud inputPoly in input)
            {
                TSD.PointList pts = new TSD.PointList();

                foreach (T3D.Point pt in inputPoly.Points)
                {
                    T3D.Point temp = __GeometryOperations.applyGlobalOffset(pt);
                    pts.Add(temp);
                }

                bool found = false;
                bool same = true;
                foreach (TSD.Cloud outputPoly in output)
                {
                    if (outputPoly.Points.Count == pts.Count)
                    {
                        if (outputPoly.Points.Count != 0)
                        {
                            for (int i = 0; i < outputPoly.Points.Count; i++)
                            {
                                T3D.Point oldpt = outputPoly.Points[0];
                                T3D.Point newpt = pts[0];

                                if (__GeometryOperations.compare2Points(oldpt, newpt) == false)
                                {
                                    same = false;
                                    break;
                                }
                            }

                            if (same == true)
                            {
                                found = true;
                                break;
                            }

                            for (int i = 0; i < outputPoly.Points.Count; i++)
                            {
                                T3D.Point oldpt = outputPoly.Points[0];
                                T3D.Point newpt = pts[pts.Count - i - 1];

                                if (__GeometryOperations.compare2Points(oldpt, newpt) == false)
                                {
                                    same = false;
                                    break;
                                }
                            }

                            if (same == true)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (found == false)
                {
                    TSD.Cloud outputLine = new TSD.Cloud(outputView, pts, inputPoly.Attributes);
                    outputLine.Attributes = inputPoly.Attributes;
                    outputLine.Insert();
                }
            }
        }


        private static void createRectangles(List<TSD.Rectangle> input, List<TSD.Rectangle> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Rectangle inputRectangle in input)
            {
                T3D.Point startPoint = __GeometryOperations.applyGlobalOffset(inputRectangle.StartPoint);
                T3D.Point endPoint = __GeometryOperations.applyGlobalOffset(inputRectangle.EndPoint);

                bool found = false;
                foreach (TSD.Rectangle outputLine in output)
                {
                    if (outputLine.StartPoint == startPoint && outputLine.EndPoint == endPoint)
                    {
                        found = true;
                        break;
                    }
                    else if (outputLine.StartPoint == endPoint && outputLine.EndPoint == startPoint)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.Rectangle outputLine = new TSD.Rectangle(outputView, startPoint, endPoint, inputRectangle.Attributes);
                    outputLine.Attributes = inputRectangle.Attributes;
                    outputLine.Insert();
                }

            }
        }


        private static void createPolygons(List<TSD.Polygon> input, List<TSD.Polygon> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Polygon inputPoly in input)
            {
                TSD.PointList pts = new TSD.PointList();

                foreach (T3D.Point pt in inputPoly.Points)
                {
                    T3D.Point temp = __GeometryOperations.applyGlobalOffset(pt);
                    pts.Add(temp);
                }

                bool found = false;
                bool same = true;
                foreach (TSD.Polygon outputPoly in output)
                {
                    if (outputPoly.Points.Count == pts.Count)
                    {
                        if (outputPoly.Points.Count != 0)
                        {
                            for (int i = 0; i < outputPoly.Points.Count; i++)
                            {
                                T3D.Point oldpt = outputPoly.Points[0];
                                T3D.Point newpt = pts[0];

                                if (__GeometryOperations.compare2Points(oldpt, newpt) == false)
                                {
                                    same = false;
                                    break;
                                }
                            }

                            if (same == true)
                            {
                                found = true;
                                break;
                            }

                            for (int i = 0; i < outputPoly.Points.Count; i++)
                            {
                                T3D.Point oldpt = outputPoly.Points[0];
                                T3D.Point newpt = pts[pts.Count - i - 1];

                                if (__GeometryOperations.compare2Points(oldpt, newpt) == false)
                                {
                                    same = false;
                                    break;
                                }
                            }

                            if (same == true)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (found == false)
                {
                    TSD.Polygon outputLine = new TSD.Polygon(outputView, pts, inputPoly.Attributes);
                    outputLine.Attributes = inputPoly.Attributes;
                    outputLine.Insert();
                }
            }
        }


        private static void createLinesNoOffset(List<TSD.Line> input, List<TSD.Line> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Line inputLine in input)
            {
                T3D.Point startPoint = inputLine.StartPoint;
                T3D.Point endPoint = inputLine.EndPoint;

                bool found = false;
                foreach (TSD.Line outputLine in output)
                {
                    if (outputLine.StartPoint == startPoint && outputLine.EndPoint == endPoint)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    TSD.Line outputLine = new TSD.Line(outputView, startPoint, endPoint, inputLine.Attributes);
                    outputLine.Attributes = inputLine.Attributes;
                    outputLine.Insert();
                }
            }
        }

    }
}
