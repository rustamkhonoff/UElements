using UnityEngine;

namespace UElements
{
    public class ElementsRoot : ElementBase
    {
        [field: SerializeField] public Transform Parent { get; private set; }
        private void Reset() => Parent = transform;
    }
}