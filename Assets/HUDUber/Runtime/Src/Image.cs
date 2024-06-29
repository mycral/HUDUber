using System;
using System.Collections.Generic;
using UnityEngine;

namespace HUDUber
{
    [Serializable]
    public class Image : Graphic
    {
        public Sprite MainSprite;


        public override void Copy(Graphic graphic)
        {
            base.Copy(graphic);
            var image = graphic as Image;
            MainSprite = image.MainSprite;
        }

        public override void BuildMesh(MeshBuffer meshBuffer)
        {
            var rect = MainSprite.UvRect;
            meshBuffer.AddVertex(m_kRect.position + new Vector2(0, m_kRect.height),
                new Vector4(0,0, rect.x, rect.y + rect.height), 
                m_kColor);
            meshBuffer.AddVertex(m_kRect.position + new Vector2(m_kRect.width, m_kRect.height),
                new Vector4(0, 0, rect.x + rect.width, rect.y + rect.height),
                m_kColor);
            meshBuffer.AddVertex(m_kRect.position + new Vector2(m_kRect.width, 0),
                new Vector4(0, 0, rect.x + rect.width, rect.y),
                m_kColor);
            meshBuffer.AddVertex(m_kRect.position,
                new Vector4(0, 0, rect.x, rect.y),
                m_kColor);
        }
    }
}
