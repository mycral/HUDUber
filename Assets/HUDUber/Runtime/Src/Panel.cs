using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HUDUber
{
    [Serializable]
    public class Panel : Layout
    {
        public Matrix4x4 m_kMatrix;
        public int m_iMatrixIndex;

        public override void Copy(Graphic graphic)
        {
            base.Copy(graphic);
        }
    }
}
