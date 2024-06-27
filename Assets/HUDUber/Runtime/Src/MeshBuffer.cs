using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HUDUber
{
    public class MeshBuffer
    {
        public List<Vector3> m_kVertexList;
        public List<Vector4> m_kUV1List;
        public List<Color> m_kVertexColorList;

        public MeshBuffer(int vertexCapticy)
        {
            m_kVertexList = new List<Vector3>(vertexCapticy);
            m_kUV1List = new List<Vector4>(vertexCapticy);
            m_kVertexColorList = new List<Color>(vertexCapticy);
        }

        public void Reset()
        {
            m_kVertexList.Clear();
            m_kUV1List.Clear();
            m_kVertexColorList.Clear();
        }

        public void AddVertex(Vector3 vertex, Vector4 uv1, Color color)
        {
            m_kVertexList.Add(vertex);
            m_kUV1List.Add(uv1);
            m_kVertexColorList.Add(color);
        }
    }
}
