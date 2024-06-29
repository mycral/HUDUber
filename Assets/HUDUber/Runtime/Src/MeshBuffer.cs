using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity.Collections;

namespace HUDUber
{
    public class MeshBuffer : IDisposable
    {
        public NativeList<Vector3> m_kVertexList;
        public NativeList<Vector4> m_kUVList;
        public NativeList<Color> m_kColorList;

        private bool m_bDisposed = false;

        public MeshBuffer(int vertexCapticy)
        {
            m_kVertexList = new NativeList<Vector3>(vertexCapticy, Unity.Collections.Allocator.Persistent);
            m_kUVList = new NativeList<Vector4>(vertexCapticy, Unity.Collections.Allocator.Persistent);
            m_kColorList = new NativeList<Color>(vertexCapticy, Unity.Collections.Allocator.Persistent);
        }

        public void Reset()
        {
            m_kVertexList.Clear();
            m_kUVList.Clear();
            m_kColorList.Clear();
        }

        public void AddVertex(Vector3 vertex, Vector4 uv1, Color color)
        {
            m_kVertexList.Add(vertex);
            m_kUVList.Add(uv1);
            m_kColorList.Add(color);
        }

        public void Combine(MeshBuffer meshBuffer)
        {
            m_kVertexList.AddRange(meshBuffer.m_kVertexList);
            m_kUVList.AddRange(meshBuffer.m_kUVList);
            m_kColorList.AddRange(meshBuffer.m_kColorList);
        }

        public void Dispose()
        {
            if (m_bDisposed)
            {
                return;
            }
            m_kVertexList.Dispose();
            m_kUVList.Dispose();
            m_kColorList.Dispose();
            m_bDisposed = true;
        }
        ~MeshBuffer()
        {
            Dispose();
        }
    }
}
