using HUDUber;
namespace HUDUber.Prefab
{
    public class HUDPrefabScriptImage : HUDPrefabScript<Image>
    {
        public override Graphic Build()
        {
            Image image = new Image();
            image.Copy(m_kGraphic);
            image.m_kRect.position = transform.localPosition;
            return image;
        }
    }
}