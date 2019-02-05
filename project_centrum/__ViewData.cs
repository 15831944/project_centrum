﻿using System;
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
    class __ViewBaseData
    {
        public TSD.ViewBase view;

        public List<_Mark_Beam> markBeams;
        public List<_Mark_PolyBeam> markPolyBeams;
        public List<_Mark_ContourPlate> markContourPlates;
        public List<_Mark_BoltGroup> markBoltGroup;        
        public List<_Mark_SingleRebar> markSingleRebars;
        public List<_Mark_RebarGroup> markRebarBases;

        public List<_SectionMark> sectionMarks;
        public List<TSD.DetailMark> detailMarks;
        public List<TSD.Text> txt;
        public List<_StraightDimentionSet> straightDimSets;

        public List<TSD.Arc> arcs;
        public List<TSD.Line> lines;
        public List<TSD.Polyline> polylines;
        public List<TSD.Circle> circles;
        public List<TSD.Cloud> clouds;
        public List<TSD.Rectangle> rectangles;
        public List<TSD.Polygon> polygons;

        public List<TSD.TextFile> txtFiles;
        public List<TSD.DwgObject> dwgRefs;

        public __ViewBaseData(TSD.ViewBase currentView)
        {
            view = currentView;

            markBeams = new List<_Mark_Beam>();
            markPolyBeams = new List<_Mark_PolyBeam>();
            markContourPlates = new List<_Mark_ContourPlate>();
            markBoltGroup = new List<_Mark_BoltGroup>();
            markSingleRebars = new List<_Mark_SingleRebar>();
            markRebarBases = new List<_Mark_RebarGroup>();
            
            sectionMarks = new List<_SectionMark>();
            detailMarks = new List<TSD.DetailMark>();
            txt = new List<TSD.Text>();
            straightDimSets = new List<_StraightDimentionSet>();

            arcs = new List<TSD.Arc>();
            lines = new List<TSD.Line>();
            polylines = new List<TSD.Polyline>();
            circles = new List<TSD.Circle>();
            clouds = new List<TSD.Cloud>();
            rectangles = new List<TSD.Rectangle>();
            polygons = new List<TSD.Polygon>();

            txtFiles = new List<TSD.TextFile>();
            dwgRefs = new List<TSD.DwgObject>();
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

            else if (dro is TSD.Text)
            {
                TSD.Text current = dro as TSD.Text;
                txt.Add(current);
            }

            else if (dro is TSD.Arc)
            {
                TSD.Arc current = dro as TSD.Arc;
                arcs.Add(current);
            }

            else if (dro is TSD.Line)
            {
                TSD.Line current = dro as TSD.Line;
                lines.Add(current);
            }

            else if (dro is TSD.Polyline)
            {
                TSD.Polyline current = dro as TSD.Polyline;
                polylines.Add(current);
            }

            else if (dro is TSD.Circle)
            {
                TSD.Circle current = dro as TSD.Circle;
                circles.Add(current);
            }

            else if (dro is TSD.Cloud)
            {
                TSD.Cloud current = dro as TSD.Cloud;
                clouds.Add(current);
            }
                       
            else if (dro is TSD.Rectangle)
            {
                TSD.Rectangle current = dro as TSD.Rectangle;
                rectangles.Add(current);
            }
                       
            else if (dro is TSD.Polygon)
            {
                TSD.Polygon current = dro as TSD.Polygon;
                polygons.Add(current);
            }

            else if (dro is TSD.TextFile)
            {
                TSD.TextFile current = dro as TSD.TextFile;
                txtFiles.Add(current);
            }

            else if (dro is TSD.DwgObject)
            {
                TSD.DwgObject current = dro as TSD.DwgObject;
                dwgRefs.Add(current);
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
                        else if (modelObject is TSM.BoltGroup)
                        {
                            TSM.BoltGroup currentMO = modelObject as TSM.BoltGroup;
                            markBoltGroup.Add(new _Mark_BoltGroup(currentMark, currentMO, currentDO, this.view));
                        }
                    }
                }
            }
        }

    }
}

