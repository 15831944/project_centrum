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
    class __MatchMaker
    {
        public static Dictionary<TSD.TextFile, TSD.TextFile> txtFinder(List<TSD.TextFile> input, List<TSD.TextFile> output)
        {
            List<TSD.TextFile> tempInput = new List<TSD.TextFile>(input);
            List<TSD.TextFile> tempOutput = new List<TSD.TextFile>(output);

            Dictionary<TSD.TextFile, TSD.TextFile> closest = new Dictionary<Tekla.Structures.Drawing.TextFile, Tekla.Structures.Drawing.TextFile>();

            while (tempInput.Count > 0 && tempOutput.Count > 0)
            {
                double min = double.MaxValue;
                TSD.TextFile minIn = null;
                TSD.TextFile minOut = null;

                foreach (TSD.TextFile aa in tempInput)
                {
                    foreach (TSD.TextFile bb in tempOutput)
                    {
                        double X = aa.InsertionPoint.X - bb.InsertionPoint.X;
                        double Y = aa.InsertionPoint.Y - bb.InsertionPoint.Y;
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

        public static Dictionary<T, T> matchMarks<T>(List<T> input, List<T> output, out List<T> notFound) where T : _Mark
        {
            List<T> tempInput = new List<T>(input);
            List<T> tempOutput = new List<T>(output);
            notFound = new List<T>();
            Dictionary<T, T> matches = new Dictionary<T, T>();

            foreach (T outputMark in tempOutput)
            {
                bool found = false;

                foreach (T inputMark in tempInput)
                {
                    bool areSame = outputMark.checkModelObjects(inputMark);

                    if (areSame)
                    {
                        matches[inputMark] = outputMark;
                        tempInput.Remove(inputMark);
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    notFound.Add(outputMark);
                }
            }

            return matches;
        }
    }
}
