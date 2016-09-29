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
    class __CopyViewHandler
    {
        public static void redraw(__ViewData input, __ViewData output)
        {
            TSD.DrawingHandler drawingHandler = new TSD.DrawingHandler();

            if (drawingHandler.GetConnectionStatus())
            {
                if (output.view is TSD.View)
                {
                    if (UserProperties._view)
                    {
                        if (input._repView != false && output._repView != false)
                        {
                            if (UserProperties.sheetInputPoint == null || UserProperties.sheetOutputPoint == null)
                            {
                                repositionView(input.view, output.view);
                            }
                            else
                            {
                                repositionViewWithOffset(input.view, output.view);
                            }
                        }
                    }

                    if (UserProperties._mark || UserProperties._red || UserProperties._predict)
                    {
                        handleMarks<_Mark_Beam>(input.markBeams, output.markBeams);
                        handleMarks<_Mark_PolyBeam>(input.markPolyBeams, output.markPolyBeams);
                        handleMarks<_Mark_ContourPlate>(input.markContourPlates, output.markContourPlates);
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

                    if (UserProperties._dim)
                    {
                        createDimentionLineSets(input.straightDimSets, output.straightDimSets, output.view);
                    }

                    if (UserProperties._line)
                    {
                        createLines(input.lines, output.lines, input.view, output.view);
                    }
                }
                else
                {
                    if (UserProperties._line)
                    {
                        createLinesNoOffset(input.lines, output.lines, input.view, output.view);
                    }
                }


                if (UserProperties._txt)
                {
                    repositionTextFile(input.txtFiles, output.txtFiles);
                }

            }
        }

        private static void repositionView(TSD.ViewBase input, TSD.ViewBase output)
        {
            output.Origin = input.Origin;
            output.Modify();
        }

        private static void repositionViewWithOffset(TSD.ViewBase input, TSD.ViewBase output)
        {
            
            T3D.Point minus = __GeometryOperations.getLocalOffset(UserProperties.sheetOutputPoint, UserProperties.sheetInputPoint);
            T3D.Point gg = __GeometryOperations.applyLocalOffset(output.Origin, minus);
            output.Origin = gg;
            output.Modify();
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

                TSD.SectionMark outputSectionMark = new TSD.SectionMark(outputView, leftPoint, rightPoint);
                outputSectionMark.Attributes = inputSection._obj.Attributes;
                outputSectionMark.Insert();

                if (inputSection._txt != null)
                {
                    outputSectionMark.Attributes.TagsAttributes.TagA2.TagContent.Clear();
                    outputSectionMark.Attributes.TagsAttributes.TagA2.TagContent.Add(inputSection._txt);
                    outputSectionMark.Modify();
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

                TSD.DetailMark outputDetailMark = new TSD.DetailMark(outputView, centerPoint, boundaryPoint, labelPoint);
                outputDetailMark.Attributes = inputDetail.Attributes;
                outputDetailMark.Insert();
            }
        }

        private static void repositionTextFile(List<TSD.TextFile> input, List<TSD.TextFile> output)
        {
            Dictionary<TSD.TextFile, TSD.TextFile> closest = __MatchMaker.txtFinder(input, output);

            foreach (TSD.TextFile key in closest.Keys)
            {
                closest[key].InsertionPoint = key.InsertionPoint;
                closest[key].Attributes = key.Attributes;
                closest[key].Size = key.Size;
                //closest[key].Attributes.Scaling = TSD.ScalingOptions.NoScaling; //HARDCODE
                closest[key].Modify();
                Debuger.p("TextFile -> No Scaling");
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

                TSD.StraightDimensionSetHandler sds = new TSD.StraightDimensionSetHandler();
                TSD.StraightDimensionSet outputDimSet = sds.CreateDimensionSet(outputView, outputPoints, inputDimSet._first.UpDirection, inputDimSet._set.Distance, inputDimSet._set.Attributes);
                outputDimSet.Distance = inputDimSet._set.Distance;
                outputDimSet.Modify();
            }
        }

        private static void createLines(List<TSD.Line> input, List<TSD.Line> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Line inputLine in input)
            {
                T3D.Point startPoint = __GeometryOperations.applyGlobalOffset(inputLine.StartPoint);
                T3D.Point endPoint = __GeometryOperations.applyGlobalOffset(inputLine.EndPoint);

                TSD.Line outputLine = new TSD.Line(outputView, startPoint, endPoint, inputLine.Attributes);
                outputLine.Attributes = inputLine.Attributes;
                outputLine.Insert();
            }
        }

        private static void createLinesNoOffset(List<TSD.Line> input, List<TSD.Line> output, TSD.ViewBase inputView, TSD.ViewBase outputView)
        {
            foreach (TSD.Line inputLine in input)
            {
                T3D.Point startPoint = inputLine.StartPoint;
                T3D.Point endPoint = inputLine.EndPoint;

                TSD.Line outputLine = new TSD.Line(outputView, startPoint, endPoint, inputLine.Attributes);
                outputLine.Attributes = inputLine.Attributes;
                outputLine.Insert();
            }
        }
    }
}

