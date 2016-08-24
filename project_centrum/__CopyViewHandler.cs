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
                        if (input.view != null && output.view != null)
                        {
                            repositionView(input.view, output.view);
                        }
                    }

                    if (UserProperties._mark || UserProperties._mark_attr || UserProperties._red)
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

                //else if (output.view is TSD.ContainerView)
                //{
                //    if (input.view != null && output.view != null)
                //    {
                //        printView(input.view, output.view);
                //    }
                //}

                if (UserProperties._txt)
                {
                    repositionTextFile(input.txtFiles, output.txtFiles);
                }

            }
        }

        private static void repositionView(TSD.ViewBase input, TSD.ViewBase output)
        {
            //Form1._form.add_text("I1: " + input.Origin.X + ", " + input.Origin.Y);
            //Form1._form.add_text("I2: " + input.FrameOrigin.X + ", " + input.FrameOrigin.Y);
            //Form1._form.add_text("");
            //Form1._form.add_text("O1: " + output.Origin.X + ", " + output.Origin.Y);
            //Form1._form.add_text("O2: " + output.FrameOrigin.X + ", " + output.FrameOrigin.Y);


            output.Origin = input.Origin;
            output.Modify();
        }

        //private static void printView(TSD.ViewBase input, TSD.ViewBase output)
        //{
        //    Form1._form.add_text("CI1: " + input.Origin.X + ", " + input.Origin.Y);
        //    Form1._form.add_text("CI2: " + input.FrameOrigin.X + ", " + input.FrameOrigin.Y);
        //    Form1._form.add_text("");
        //    Form1._form.add_text("CO1: " + output.Origin.X + ", " + output.Origin.Y);
        //    Form1._form.add_text("CO2: " + output.FrameOrigin.X + ", " + output.FrameOrigin.Y);
        //}

        private static void handleMarks<T>(List<T> input, List<T> output) where T : _Mark
        {
            List<T> notFound = new List<T>();
            Dictionary<T, T> matches = __MatchMaker.matchMarks(input, output, out notFound);

            if (UserProperties._mark)
            {
                repositionMarks<T>(matches);
            }

            if (UserProperties._red)
            {
                setNotFoundRed<T>(notFound);
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
                output._mark.Attributes.Frame.Color = TSD.DrawingColors.Red;
                output._mark.Attributes.Frame.Type = TSD.FrameTypes.Rectangular;
                output._mark.Modify();
            }
        }

        private static void createSectionMarks(List<_SectionMark> input, List<_SectionMark> output, TSD.View outputView)
        {
            foreach (_SectionMark inputSection in input)
            {
                T3D.Point leftPoint = __Transformster.Transform(inputSection._obj.LeftPoint);
                T3D.Point rightPoint = __Transformster.Transform(inputSection._obj.RightPoint);

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
                T3D.Point centerPoint = __Transformster.Transform(inputDetail.CenterPoint);
                T3D.Point boundaryPoint = __Transformster.Transform(inputDetail.BoundaryPoint);
                T3D.Point labelPoint = __Transformster.Transform(inputDetail.LabelPoint);

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
                closest[key].Modify();
                closest[key].Attributes.Scaling = TSD.ScalingOptions.NoScaling; //HARDCODE
                closest[key].Modify();
            }
        }

        private static void createDimentionLineSets(List<_StraightDimentionSet> input, List<_StraightDimentionSet> output, TSD.ViewBase outputView)
        {
            foreach (_StraightDimentionSet inputDimSet in input)
            {
                TSD.PointList outputPoints = new TSD.PointList();
                foreach (T3D.Point ip in inputDimSet._points)
                {
                    T3D.Point op = __Transformster.Transform(ip);
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
                T3D.Point startPoint = __Transformster.Transform(inputLine.StartPoint);
                T3D.Point endPoint = __Transformster.Transform(inputLine.EndPoint);

                TSD.Line outputLine = new TSD.Line(outputView, startPoint, endPoint, inputLine.Attributes);
                outputLine.Attributes = inputLine.Attributes;
                outputLine.Insert();
            }
        }
    }
}

