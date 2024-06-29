using HUDUber;

namespace HUDUber.Prefab
{
    public class HUDPrefabScriptPanel : HUDPrefabScript<Panel>
    {
        public override Graphic Build()
        {
            Panel panel = new Panel();
            panel.Copy(m_kGraphic);
            panel.m_kRect.position = transform.position;

            for (int i = 0; i < transform.childCount; i++)
            {
                var script = transform.GetChild(i).GetComponent<HUDPrefabBaseScript>();
                if (script != null)
                {
                    panel.AddChild(script.Build());
                }
            }
            return panel;
        }
    }
}