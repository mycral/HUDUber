using System;
using System.Collections.Generic;
using UnityEngine;

namespace HUDUber
{
    [Serializable]
    public class Text : Graphic
    {
        public Font m_kFont;
        
        public int m_iFontSize;
        public FontStyle m_eFontStyle;

        public int m_iRawVertexCount;

        public Vector2 m_kEffectDistance;
        public Color m_kEffectColor = Color.black;
        public Color m_kFontColor = Color.white;

        public Sprite m_kTestSprite;
        public Color m_kSpriteColor = Color.white;

        public bool m_bUseShadow;
        public bool m_bUseOutline;
        [SerializeField]
        private string m_kStrText;
        private bool m_bSizeDirty;
        private string StrText
        {
            get { return m_kStrText; }
            set
            {
                if(m_kStrText == value)
                {
                    return;
                }
                m_kStrText = value;
                m_bSizeDirty = true;
            }
        }



        public override void Copy(Graphic graphic)
        {
            base.Copy(graphic);
            var text = graphic as Text;
            m_kFont = text.m_kFont;
            m_iFontSize = text.m_iFontSize;
            m_eFontStyle = text.m_eFontStyle;
            m_kStrText = text.m_kStrText;
            m_kFontColor = text.m_kFontColor;
            m_kEffectDistance = text.m_kEffectDistance;
            m_kEffectColor = text.m_kEffectColor;
            m_bUseShadow = text.m_bUseShadow;
            m_bUseOutline = text.m_bUseOutline;
            m_bSizeDirty = true;
        }

        public override Vector2 RebuildSize()
        {
            if(!m_bSizeDirty)
            {
                return m_kRect.size;
            }

            m_kFont.RequestCharactersInTexture(m_kStrText, m_iFontSize, m_eFontStyle);

            using var autoSmb = VertexHelper.GetAutoReleaseMeshBuffer();
            var smb = autoSmb.target;
            smb.Reset();
            var vertexList = smb.m_kVertexList;

            string str = m_kStrText;
            Font font = m_kFont;
            Vector2 pos = Vector2.zero;

            for (int i = 0; i < str.Length; i++)
            {
                CharacterInfo ch;
                bool retx = font.GetCharacterInfo(str[i], out ch, m_iFontSize, m_eFontStyle);

                vertexList.Add(pos + new Vector2(ch.minX, ch.maxY));
                vertexList.Add(pos + new Vector2(ch.maxX, ch.maxY));
                vertexList.Add(pos + new Vector2(ch.maxX, ch.minY));
                vertexList.Add(pos + new Vector2(ch.minX, ch.minY));
                pos.x += ch.advance;
            }

            if(vertexList.Length == 0)
            {
                m_kRect.size = Vector2.zero;
            }
            else
            {
                var textBounds = new Bounds(vertexList[0], Vector3.zero);

                for (int i = 1; i < vertexList.Length; i++)
                {
                    textBounds.Encapsulate(vertexList[i]);
                }
                m_kRect.size = textBounds.size;
            }
            
            m_bSizeDirty = false;
            return m_kRect.size;
        }

        public override void BuildMesh(MeshBuffer meshBuffer)
        {
            m_kFont.RequestCharactersInTexture(m_kStrText, m_iFontSize, m_eFontStyle);

            using var autoSMB = VertexHelper.GetAutoReleaseMeshBuffer();
            var smb = autoSMB.target;
            smb.Reset();

            var vertexList = smb.m_kVertexList;
            var uvList = smb.m_kUVList;
            var colorList = smb.m_kColorList;


            string str = m_kStrText;
            Font font = m_kFont;
            Vector3 pos = m_kRect.position;

            for (int i = 0; i < str.Length; i++)
            {
                // Get character rendering information from the font
                CharacterInfo ch;
                bool retx = font.GetCharacterInfo(str[i], out ch, m_iFontSize, m_eFontStyle);

                vertexList.Add(pos + new Vector3(ch.minX, ch.maxY, 0));
                vertexList.Add(pos + new Vector3(ch.maxX, ch.maxY, 0));
                vertexList.Add(pos + new Vector3(ch.maxX, ch.minY, 0));
                vertexList.Add(pos + new Vector3(ch.minX, ch.minY, 0));

                uvList.Add(ch.uvTopLeft);
                uvList.Add(ch.uvTopRight);
                uvList.Add(ch.uvBottomRight);
                uvList.Add(ch.uvBottomLeft);

                colorList.Add(m_kFontColor);
                colorList.Add(m_kFontColor);
                colorList.Add(m_kFontColor);
                colorList.Add(m_kFontColor);

                // Advance character position
                pos += new Vector3(ch.advance, 0, 0);
            }

            if(m_bUseShadow)
            {
                using var shadow1 = VertexHelper.GetAutoReleaseMeshBuffer();
                ApplyShadow(smb, shadow1.target, m_kEffectColor, new Vector2(m_kEffectDistance.x, m_kEffectDistance.y));
                meshBuffer.Combine(shadow1.target);
            }
            else if(m_bUseOutline)
            {
                using var shadow1 = VertexHelper.GetAutoReleaseMeshBuffer();
                using var shadow2 = VertexHelper.GetAutoReleaseMeshBuffer();
                using var shadow3 = VertexHelper.GetAutoReleaseMeshBuffer();
                using var shadow4 = VertexHelper.GetAutoReleaseMeshBuffer();
                ApplyShadow(smb, shadow1.target, m_kEffectColor, new Vector2(m_kEffectDistance.x, -m_kEffectDistance.y));
                ApplyShadow(smb, shadow2.target, m_kEffectColor, new Vector2(-m_kEffectDistance.x, -m_kEffectDistance.y));
                ApplyShadow(smb, shadow3.target, m_kEffectColor, new Vector2(m_kEffectDistance.x, m_kEffectDistance.y));
                ApplyShadow(smb, shadow4.target, m_kEffectColor, new Vector2(-m_kEffectDistance.x, m_kEffectDistance.y));
                meshBuffer.Combine(shadow1.target);
                meshBuffer.Combine(shadow2.target);
                meshBuffer.Combine(shadow3.target);
                //meshBuffer.Combine(shadow4);
                //VertexHelper.ReleaseMeshBuffer(shadow1);
                //VertexHelper.ReleaseMeshBuffer(shadow2);
                //VertexHelper.ReleaseMeshBuffer(shadow3);
                //VertexHelper.ReleaseMeshBuffer(shadow4);
            }
            meshBuffer.Combine(smb);
        }

        public void ApplyShadow(MeshBuffer rawBuffer,MeshBuffer shadowBuffer, Color color, Vector2 offset)
        {
            int vCount = rawBuffer.m_kVertexList.Length;
            for (int i = 0; i < vCount; i++)
            {
                shadowBuffer.m_kVertexList.Add(rawBuffer.m_kVertexList[i] + new Vector3(offset.x, offset.y, 0));
                shadowBuffer.m_kUVList.Add(rawBuffer.m_kUVList[i]);
                shadowBuffer.m_kColorList.Add(color);
            }
        }
    }
}
