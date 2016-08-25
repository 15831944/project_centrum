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
            List<TSD.ViewBase> inputKeys = new List<TSD.ViewBase>(input.data.Keys);
            List<TSD.ViewBase> outputKeys = new List<TSD.ViewBase>(output.data.Keys);
            Dictionary<TSD.ViewBase, TSD.ViewBase> closest = __MatchMaker.viewFinder(inputKeys, outputKeys);

            foreach (TSD.ViewBase inView in closest.Keys)
            {
                TSD.ViewBase outView = closest[inView];
                __CopyViewHandler.redraw(input.data[inView], output.data[outView]);
            }
        }
    }
}
