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
            Dictionary<TSD.ViewBase, TSD.ViewBase> closest = viewFinder(inputKeys, outputKeys);

            foreach (TSD.ViewBase inView in closest.Keys)
            {
                TSD.ViewBase outView = closest[inView];
                __CopyViewHandler.redraw(input.data[inView], output.data[outView]);
            }
        }

        public static Dictionary<TSD.ViewBase, TSD.ViewBase> viewFinder(List<TSD.ViewBase> input, List<TSD.ViewBase> output)
        {
            List<TSD.ViewBase> tempInput = new List<TSD.ViewBase>(input);
            List<TSD.ViewBase> tempOutput = new List<TSD.ViewBase>(output);

            Dictionary<TSD.ViewBase, TSD.ViewBase> closest = new Dictionary<TSD.ViewBase, TSD.ViewBase>();

            while (tempInput.Count > 0 && tempOutput.Count > 0)
            {

                double min = double.MaxValue;
                TSD.ViewBase minIn = null;
                TSD.ViewBase minOut = null;

                foreach (TSD.ViewBase aa in tempInput)
                {
                    foreach (TSD.ViewBase bb in tempOutput)
                    {
                        double X = aa.Origin.X - bb.Origin.X;
                        double Y = aa.Origin.Y - bb.Origin.Y;
                        double dist = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

                        if (dist < min)
                        {
                            min = dist;
                            minIn = aa;
                            minOut = bb;
                        }
                    }
                }

                if (minIn != null && minOut != null)
                {
                    closest[minIn] = minOut;
                    tempInput.Remove(minIn);
                    tempOutput.Remove(minOut);
                }
                else
                {
                    break;
                }

            }

            return closest;
        }
    }
}
