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
                }

                if (UserProperties._txt)
                {
                    repositionTextFile(input.txtFiles, output.txtFiles);
                }

                if (UserProperties._dim)
                {
                    createDimentionLineSets(input.straightDimSets, output.straightDimSets, output.view);
                }

                if (UserProperties._line)
                {
                    createLines(input.lines, output.lines, output.view);
                }

            }
        }

        private static void repositionView(TSD.ViewBase input, TSD.ViewBase output)
        {
            output.Origin = input.Origin;
            output.Modify();
        }

        private static void handleMarks<T>(List<T> input, List<T> output) where T : _Mark
        {
            List<T> notFound = new List<T>();
            Dictionary<T, T> matches = __MatchMaker.matchMarks(input, output, out notFound);

            if (UserProperties._mark)
            {
                repositionMarks<T>(matches);
            }

            if (UserProperties._mark_attr)
            {
                changeMarkAttrs<T>(matches);
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
                matches[inputKey].changeMarkLocation(inputKey);
            }
        }

        private static void changeMarkAttrs<T>(Dictionary<T, T> matches) where T : _Mark
        {
            foreach (T inputKey in matches.Keys)
            {
                matches[inputKey].changeMarkAttributes(inputKey);
            }
        }

        private static void setNotFoundRed<T>(List<T> notFound) where T : _Mark
        {
            foreach (T output in notFound)
            {
                output.objectNotFound();
            }
        }

        private static void createSectionMarks(List<_SectionMark> input, List<_SectionMark> output, TSD.View outputView)
        {
            foreach (_SectionMark inputSection in input)
            {
                TSD.SectionMark outputSectionMark = new TSD.SectionMark(outputView, inputSection._obj.LeftPoint, inputSection._obj.RightPoint);
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
                TSD.DetailMark outputDetailMark = new TSD.DetailMark(outputView, inputDetail.CenterPoint, inputDetail.BoundaryPoint, inputDetail.LabelPoint);
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
                closest[key].Attributes.Scaling = TSD.ScalingOptions.NoScaling; //HARDCODE
                closest[key].Size = key.Size;
                closest[key].Modify();
            }
        }

        private static void createDimentionLineSets(List<_StraightDimentionSet> input, List<_StraightDimentionSet> output, TSD.ViewBase outputView)
        {
            foreach (_StraightDimentionSet inputDimSet in input)
            {
                TSD.StraightDimensionSetHandler sds = new TSD.StraightDimensionSetHandler();
                TSD.StraightDimensionSet outputDimSet = sds.CreateDimensionSet(outputView, inputDimSet._points, inputDimSet._first.UpDirection, inputDimSet._set.Distance, inputDimSet._set.Attributes);
                outputDimSet.Distance = inputDimSet._set.Distance;
                outputDimSet.Modify();
            }
        }

        private static void createLines(List<TSD.Line> input, List<TSD.Line> output, TSD.ViewBase outputView)
        {
            foreach (TSD.Line inputLine in input)
            {
                TSD.Line outputLine = new TSD.Line(outputView, inputLine.StartPoint, inputLine.EndPoint, inputLine.Attributes);
                outputLine.Attributes = inputLine.Attributes;
                outputLine.Insert();
            }
        }
    }
}

