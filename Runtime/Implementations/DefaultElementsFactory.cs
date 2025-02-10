using UnityEngine;

namespace UElements
{
    public class ElementsFactory : IElementsFactory
    {
        public T Instantiate<T>(T prefab, Transform parent = null) where T : ElementBase
        {
            return Object.Instantiate(prefab, parent);
        }
    }
}