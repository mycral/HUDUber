using UnityEngine;


namespace HUDUber
{
    public abstract class HUDPrefabScript<T> : HUDPrefabBaseScript where T : Graphic
    {
        public T m_kGraphic;
    }
}