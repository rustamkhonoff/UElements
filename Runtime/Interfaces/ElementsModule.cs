using UnityEngine;

namespace UElements
{
    public abstract class ElementsModule : ScriptableObject
    {
        public abstract IElementsProvider ElementsProvider { get; }
    }
}