using UnityEngine;

namespace UElements
{
    public interface IElementsFactory
    {
        T Instantiate<T>(T prefab, Transform parent = null) where T : ElementBase;
    }
}