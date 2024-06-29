using System;
using System.Collections.Generic;
using UnityEngine;

namespace HUDUber
{
    public class Graphic
    {
        public Rect m_kRect;
        public Color m_kColor = Color.white;
        
        public Vector2 GetSize() => m_kRect.size;

        public virtual Vector2 RebuildSize()
        {
            return new Vector2(m_kRect.width, m_kRect.height);
        }

        public virtual void BuildMesh(MeshBuffer meshBuffer)
        {
            meshBuffer.AddVertex(m_kRect.position + new Vector2(0,m_kRect.height), UnityEngine.Vector4.zero, m_kColor);
            meshBuffer.AddVertex(m_kRect.position + new Vector2(m_kRect.width, m_kRect.height), UnityEngine.Vector4.zero, m_kColor);
            meshBuffer.AddVertex(m_kRect.position + new Vector2(m_kRect.width, 0), UnityEngine.Vector4.zero, m_kColor);
            meshBuffer.AddVertex(m_kRect.position, UnityEngine.Vector4.zero, m_kColor);
        }

        public virtual void Copy(Graphic graphic)
        {
            m_kRect = graphic.m_kRect;
            m_kColor = graphic.m_kColor;
        }
    }
}
