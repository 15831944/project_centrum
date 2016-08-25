using System;
using System.Collections;
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
    class _SectionMark
    {
        public TSD.SectionMark _obj;
        public TSD.TextElement _txt;

        public _SectionMark(TSD.SectionMark obj)
        {
            _obj = obj;

            IEnumerator content = obj.Attributes.TagsAttributes.TagA2.TagContent.GetEnumerator();

            while (content.MoveNext())
            {
                if (content.Current is TSD.PropertyElement)
                {
                    TSD.PropertyElement tag = content.Current as TSD.PropertyElement;
                    _txt = new TSD.TextElement(tag.GetUnformattedString(), tag.Font);
                }
            }
        }
    }
}
