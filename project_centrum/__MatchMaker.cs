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
        public static Dictionary<T, T> matchMarks<T>(List<T> input, List<T> output, out List<T> notFound, out List<T> notFoundInput) where T : _Mark
        {
            List<T> tempInput = new List<T>(input);
            List<T> tempOutput = new List<T>(output);
            notFound = new List<T>();
            notFoundInput = new List<T>();
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

            notFoundInput = tempInput;

            return matches;
        }

    }
}
