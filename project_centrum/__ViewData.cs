using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;

using Tekla.Structures;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;

namespace project_centrum
{
    class __ViewData
    {
        bool _setView;
        public TSD.ViewBase view;

        public List<_Mark_Beam> markBeams;
        public List<_Mark_PolyBeam> markPolyBeams;
        public List<_Mark_ContourPlate> markContourPlates;
        public List<_Mark_SingleRebar> markSingleRebars;
        public List<_Mark_RebarGroup> markRebarBases;

        public List<TSD.Line> lines;
        public List<_StraightDimentionSet> straightDimSets;

        public List<_SectionMark> sectionMarks;
        public List<TSD.DetailMark> detailMarks;

        public List<TSD.TextFile> txtFiles;

        public __ViewData(TSD.ViewBase currentView, bool set)
        {
            _setView = set;
            view = currentView;

            markBeams = new List<_Mark_Beam>();
            markPolyBeams = new List<_Mark_PolyBeam>();
            markContourPlates = new List<_Mark_ContourPlate>();

            markSingleRebars = new List<_Mark_SingleRebar>();
            markRebarBases = new List<_Mark_RebarGroup>();

            straightDimSets = new List<_StraightDimentionSet>();

            sectionMarks = new List<_SectionMark>();
            detailMarks = new List<TSD.DetailMark>();

            lines = new List<TSD.Line>();
            txtFiles = new List<TSD.TextFile>();
        }

        public void addOneObject(TSD.DrawingObject dro)
        {
            if (dro is TSD.Mark)
            {
                TSD.Mark current = dro as TSD.Mark;
                markHandler(current);
            }

            else if (dro is TSD.StraightDimensionSet)
            {
                TSD.StraightDimensionSet current = dro as TSD.StraightDimensionSet;
                _StraightDimentionSet temp = new _StraightDimentionSet(current);
                straightDimSets.Add(temp);
            }

            else if (dro is TSD.SectionMark)
            {
                TSD.SectionMark current = dro as TSD.SectionMark;
                _SectionMark temp = new _SectionMark(current);
                sectionMarks.Add(temp);
            }

            else if (dro is TSD.DetailMark)
            {
                TSD.DetailMark current = dro as TSD.DetailMark;
                detailMarks.Add(current);
            }

            else if (dro is TSD.Line)
            {
                TSD.Line current = dro as TSD.Line;
                lines.Add(current);
            }

            else if (dro is TSD.TextFile)
            {
                TSD.TextFile current = dro as TSD.TextFile;
                txtFiles.Add(current);
            }
        }

        private void markHandler(TSD.Mark currentMark)
        {
            if (currentMark.Attributes.Content.Count > 0)
            {
                System.Type[] Types = new System.Type[1];
                Types.SetValue(typeof(TSD.ModelObject), 0);
                TSD.DrawingObjectEnumerator markObjects = currentMark.GetRelatedObjects(Types);

                foreach (TSD.ModelObject currentDO in markObjects)
                {
                    TSM.Model myModel = new TSM.Model();
                    TSM.ModelObject modelObject = myModel.SelectModelObject(currentDO.ModelIdentifier);

                    if (modelObject != null)
                    {
                        if (modelObject is TSM.Beam)
                        {
                            TSM.Beam currentMO = modelObject as TSM.Beam;
                            markBeams.Add(new _Mark_Beam(currentMark, currentMO, currentDO, this.view));
                        }
                        else if (modelObject is TSM.PolyBeam)
                        {
                            TSM.PolyBeam currentMO = modelObject as TSM.PolyBeam;
                            markPolyBeams.Add(new _Mark_PolyBeam(currentMark, currentMO, currentDO, this.view));
                        }
                        else if (modelObject is TSM.ContourPlate)
                        {
                            TSM.ContourPlate currentMO = modelObject as TSM.ContourPlate;
                            markContourPlates.Add(new _Mark_ContourPlate(currentMark, currentMO, currentDO, this.view));
                        }
                        else if (modelObject is TSM.SingleRebar)
                        {
                            TSM.SingleRebar currentMO = modelObject as TSM.SingleRebar;
                            markSingleRebars.Add(new _Mark_SingleRebar(currentMark, currentMO, currentDO, this.view));
                        }
                        else if (modelObject is TSM.RebarGroup)
                        {
                            TSM.BaseRebarGroup currentMO = modelObject as TSM.RebarGroup;
                            markRebarBases.Add(new _Mark_RebarGroup(currentMark, currentMO, currentDO, this.view));
                        }
                    }
                }
            }
        }
    }
}

