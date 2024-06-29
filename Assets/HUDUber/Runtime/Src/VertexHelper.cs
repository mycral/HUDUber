using System;
using System.Collections.Generic;
using UnityEngine;

namespace HUDUber
{
    public static class VertexHelper
    {
        public static Vector2 s_iTextureSize = new Vector2(1024,1024);

        public static void ReleaseStaticNativeDatas()
        {
            foreach(var mb in sMeshBuffers)
            {
                mb.Dispose();
            }
            sMeshBuffers.Clear();
        }

        #region MeshBuffPool
        public static Stack<MeshBuffer> sMeshBuffers = new Stack<MeshBuffer>();

        public static MeshBuffer GetMeshBuffer()
        {
            if(sMeshBuffers.Count == 0)
            {
                return new MeshBuffer(1024);
            }
            return sMeshBuffers.Pop();
        }

        public static AutoRelease GetAutoReleaseMeshBuffer()
        {
            return new AutoRelease() { target = GetMeshBuffer() };
        }

        public struct AutoRelease : IDisposable
        {
            public MeshBuffer target;

            public void Dispose()
            {
                ReleaseMeshBuffer(target);
            }
        }

        public static void ReleaseMeshBuffer(MeshBuffer meshBuffer)
        {
            meshBuffer.Reset();
            sMeshBuffers.Push(meshBuffer);
        }
        #endregion

        #region List<T> pool
        public static Stack<List<int>> sListIntPool = new Stack<List<int>>();
        public static List<int> GetListInt()
        {
            if(sListIntPool.Count == 0)
            {
                return new List<int>(1024);
            }
            return sListIntPool.Pop();
        }

        public static void ReleaseListInt(List<int> list)
        {
            list.Clear();
            sListIntPool.Push(list);
        }
        #endregion



        public static void FillMesh(this MeshBuffer meshBuffer, Mesh mesh)
        {
            mesh.SetVertices(meshBuffer.m_kVertexList.AsArray(), 0, meshBuffer.m_kVertexList.Length);
            mesh.SetUVs(0,meshBuffer.m_kUVList.AsArray(), 0, meshBuffer.m_kUVList.Length);
            mesh.SetColors(meshBuffer.m_kColorList.AsArray(), 0, meshBuffer.m_kColorList.Length);
            int vertexCount = meshBuffer.m_kVertexList.Length;
            var intBuffer = GetListInt();
            for(int i =0;i < vertexCount; i += 4)
            {
                intBuffer.Add(i);
                intBuffer.Add(i+1);
                intBuffer.Add(i+3);

                intBuffer.Add(i + 1);
                intBuffer.Add(i + 2);
                intBuffer.Add(i + 3);
            }
            mesh.SetTriangles(intBuffer, 0);
            ReleaseListInt(intBuffer);
        }

    }
}