using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HUDUber
{
    [Serializable]
    public class Layout : Graphic
    {
        public enum LayoutType
        {
            Horizontal,
            Vertical
        }

        public enum ChildAlignment
        {
            UpperOrLeft,
            MiddleCenter,
            LowerOrRight
        }

        public LayoutType m_LayoutType;
        public ChildAlignment m_ChildAlignmentType;
        public float m_Spacing;
        public bool m_bAdjustSize;


        private List<Graphic> m_Childs;


        public override void Copy(Graphic graphic)
        {
            var layout = graphic as Layout;

            base.Copy(layout);
            m_LayoutType = layout.m_LayoutType;
            m_ChildAlignmentType = layout.m_ChildAlignmentType;
            m_Spacing = layout.m_Spacing;
            m_bAdjustSize = layout.m_bAdjustSize;
        }

        public Layout()
        {
            m_Childs = new List<Graphic>();
        }

        public void AddChild(Graphic child)
        {
            m_Childs.Add(child);
        }
        public void RemoveChild(Graphic child) 
        {
            m_Childs.Remove(child);
        }

        public override Vector2 RebuildSize()
        {
            if (m_bAdjustSize)
            {
                switch (m_LayoutType)
                {
                    case LayoutType.Horizontal:
                        AdjustSizeHorizontal();
                        break;
                    case LayoutType.Vertical:
                        AdjustSizeVertical();
                        break;
                }
            }
            return m_kRect.size;
        }

        public virtual void RebuildLayout()
        {
            switch (m_LayoutType)
            {
                case LayoutType.Horizontal:
                    RebuildHorizontalLayout();
                    break;
                case LayoutType.Vertical:
                    RebuildVerticalLayout();
                    break;
            }

            foreach (var child in m_Childs)
            {
                if(child is Layout layout)
                {
                    layout.RebuildLayout();
                }
            }
        }

        private void RebuildHorizontalLayout()
        {
            float curX = 0;
            for(int i =0;i < m_Childs.Count;++i)
            {
                var child = m_Childs[i];
                var childSize = child.RebuildSize();

                child.m_kRect.x = curX + this.m_kRect.x;
                child.m_kRect.y = this.m_kRect.y;

                curX += childSize.x + m_Spacing;
            }
        }
        private void AdjustSizeHorizontal()
        {
            Vector2 size = this.m_kRect.size;

            float totalWidth = 0;
            float totalHeight = 0;

            if (m_Childs.Count > 0)
            {
                foreach (var child in m_Childs)
                {
                    var childSize = child.RebuildSize();
                    totalWidth += childSize.x + m_Spacing;
                    totalHeight = Mathf.Max(totalHeight, childSize.y);
                }
                totalWidth -= m_Spacing;
            }
            m_kRect.size = new Vector2(totalWidth, totalHeight);
        }

        private void RebuildVerticalLayout()
        {
            float curY = 0;
            for (int i = 0; i < m_Childs.Count; ++i)
            {
                var child = m_Childs[i];
                var childSize = child.GetSize();

                child.m_kRect.x = this.m_kRect.x;
                child.m_kRect.y = curY + this.m_kRect.y;

                curY += childSize.y + m_Spacing;
            }
        }

        private void AdjustSizeVertical()
        {
            Vector2 size = this.m_kRect.size;

            float totalWidth = 0;
            float totalHeight = 0;

            if (m_Childs.Count > 0)
            {
                foreach (var child in m_Childs)
                {
                    var childSize = child.GetSize();
                    totalHeight += childSize.y + m_Spacing;
                    totalWidth = Mathf.Max(totalWidth, childSize.x);
                }
                totalHeight -= m_Spacing;
            }
            m_kRect.size = new Vector2(totalWidth, totalHeight);
        }


        public override void BuildMesh(MeshBuffer meshBuffer)
        {
            foreach (var child in m_Childs)
            {
                child.BuildMesh(meshBuffer);
            }
        }
    }
}
