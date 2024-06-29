using HUDUber;
namespace HUDUber.Prefab
{
    public class HUDPrefabScriptLayout : HUDPrefabScript<Layout>
    {
        private Layout m_kTargetLayout;
        public override Graphic Build()
        {
            m_kTargetLayout = new Layout();
            m_kTargetLayout.Copy(m_kGraphic);

            m_kTargetLayout.m_kRect.position = transform.localPosition;

            for (int i = 0; i < transform.childCount; i++)
            {
                HUDPrefabBaseScript script = transform.GetChild(i).GetComponent<HUDPrefabBaseScript>();
                if (script != null)
                {
                    m_kTargetLayout.AddChild(script.Build());
                }
            }

            return m_kTargetLayout;
        }

        public void OnValidate()
        {
            if(m_kTargetLayout != null)
            { 
                m_kTargetLayout.Copy(m_kGraphic); 
            }
        }
    }
}