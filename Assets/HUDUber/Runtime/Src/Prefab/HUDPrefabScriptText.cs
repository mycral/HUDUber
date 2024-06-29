using HUDUber;
namespace HUDUber.Prefab
{
    public class HUDPrefabScriptText : HUDPrefabScript<Text>
    {
        private Text m_kText;

        public override Graphic Build()
        {
            m_kText = new Text();
            m_kText.Copy(m_kGraphic);
            m_kText.m_kRect.position = transform.localPosition;
            return m_kText;
        }

        private void OnValidate()
        {
            if(m_kText != null)
            {
                m_kText.Copy(m_kGraphic);
            }
        }
    }
}