using UnityEngine;

namespace UElements
{
    public abstract class ElementsModule : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; }
        public abstract IElementsProvider ElementsProvider { get; }
    }
}