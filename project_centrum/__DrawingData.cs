using System;
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
    class __DrawingData
    {
        public Dictionary<TSD.ViewBase, __ViewData> data;

        public T3D.Point viewPoint = null;
        public T3D.Point sheetPoint = null;

        public __DrawingData()
        {
            data = new Dictionary<TSD.ViewBase, __ViewData>();
        }

        internal void setSheet(TSD.ContainerView sheet)
        {
            data[sheet] = new __ViewData(sheet);
        }

        internal void setView(TSD.View view)
        {
            if (view != null)
            {
                data[view] = new __ViewData(view);
            }            
        }

        internal void setPoints(T3D.Point vp, T3D.Point sp)
        {
            viewPoint = vp;
            sheetPoint = sp;
        }

        //internal void setSelectedViews(TSD.DrawingObjectEnumerator all)
        //{
        //    foreach (TSD.DrawingObject one in all)
        //    {
        //        if (one is TSD.ViewBase)
        //        {
        //            bool found = false;

        //            foreach (TSD.ViewBase stored in data.Keys)
        //            {
        //                if (stored.IsSameDatabaseObject(one))
        //                {
        //                    data[stored].addOneObject(one);
        //                    found = true;
        //                    break;
        //                }
        //            }

        //            if (found == false)
        //            {
        //                data[one as TSD.ViewBase] = new __ViewData(one as TSD.ViewBase);
        //            }
        //        }
        //    }

        //    all.Reset();
        //}

        public void populate(TSD.DrawingObjectEnumerator all)
        {
            int i = 0;
            int tot = all.GetSize();
            
            Form1._form.replace_text("Total objects to proccess: " + tot.ToString() );

            foreach (TSD.DrawingObject one in all)
            {
                i++;
                Form1._form.replace_text("Proccessing: " + i.ToString() + " of " + tot.ToString());

                if (one is TSD.Mark || one is TSD.StraightDimensionSet || one is TSD.SectionMark || one is TSD.DetailMark || one is TSD.Line || one is TSD.TextFile || one is TSD.DwgObject)
                {
                    TSD.ViewBase oneView = one.GetView();
                    foreach (TSD.ViewBase stored in data.Keys)
                    {
                        if (stored.IsSameDatabaseObject(oneView))
                        {
                            data[stored].addOneObject(one);
                            break;
                        }
                    }
                }
            }
            Form1._form.add_text(String.Empty);
        }

        //public void populateSelected(TSD.DrawingObjectEnumerator all)
        //{
        //    int i = 0;
        //    int tot = all.GetSize();

        //    Form1._form.replace_text("Total objects to proccess: " + tot.ToString());

        //    foreach (TSD.DrawingObject one in all)
        //    {
        //        i++;
        //        Form1._form.replace_text("Proccessing: " + i.ToString() + " of " + tot.ToString());

        //        if (one is TSD.Mark || one is TSD.StraightDimensionSet || one is TSD.SectionMark || one is TSD.DetailMark || one is TSD.Line || one is TSD.TextFile || one is TSD.DwgObject)
        //        {
        //            bool found = false;
        //            TSD.ViewBase oneView = one.GetView();

        //            foreach (TSD.ViewBase stored in data.Keys)
        //            {
        //                if (stored.IsSameDatabaseObject(oneView))
        //                {
        //                    data[stored].addOneObject(one);
        //                    found = true;
        //                    break;
        //                }
        //            }

        //            if (found == false)
        //            {
        //                data[oneView] = new __ViewData(oneView, false);
        //                data[oneView].addOneObject(one);
        //            }
        //        }
        //    }
        //    Form1._form.add_text(String.Empty);
        //}


        public string countObjects()
        {
            int views = 0;

            int markBeams = 0;
            int markPolyBeams = 0;
            int markContourPlates = 0;
            int markBoltGroup = 0;
            int markSingleRebars = 0;
            int markRebarBases = 0;

            int sectionMarks = 0;
            int detailMarks = 0;
            int lines = 0;
            int straightDimSets = 0;

            int txtFiles = 0;
            int dwgRefs = 0;

            foreach (TSD.ViewBase view in data.Keys)
            {
                if (data[view].view != null) views++;

                markBeams += data[view].markBeams.Count;
                markPolyBeams += data[view].markPolyBeams.Count;
                markContourPlates += data[view].markContourPlates.Count;
                markBoltGroup += data[view].markBoltGroup.Count;
                markSingleRebars += data[view].markSingleRebars.Count;
                markRebarBases += data[view].markRebarBases.Count;

                sectionMarks += data[view].sectionMarks.Count;
                detailMarks += data[view].detailMarks.Count;
                lines += data[view].lines.Count;
                straightDimSets += data[view].straightDimSets.Count;

                txtFiles += data[view].txtFiles.Count;
                dwgRefs += data[view].dwgRefs.Count;
            }

            StringBuilder message = new StringBuilder();
            message.AppendLine("");
            message.AppendLine("**************");
            message.AppendLine("Views: " + (views - 1));
            message.AppendLine("" );
            message.AppendLine("Beam marks: " + markBeams);
            message.AppendLine("PolyBeam marks: " + markPolyBeams);
            message.AppendLine("ContourPlate marks: " + markContourPlates);
            message.AppendLine("BoltGroup marks: " + markBoltGroup);
            message.AppendLine("SingleRebar marks: " + markSingleRebars);
            message.AppendLine("RebarGroup marks: " + markRebarBases);
            message.AppendLine("");
            message.AppendLine("Section marks: " + sectionMarks);
            message.AppendLine("Detail marsk: " + detailMarks);
            message.AppendLine("");
            message.AppendLine("StraightDimentionSets: " + straightDimSets);
            message.AppendLine("");
            message.AppendLine("Lines: " + lines);
            message.AppendLine("TextFiles: " + txtFiles);
            message.AppendLine("DwgRef: " + dwgRefs);
            message.AppendLine("**************");
            return message.ToString();
        }
    }
}
