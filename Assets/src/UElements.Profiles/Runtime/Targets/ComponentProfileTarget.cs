// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
using UnityEngine;

namespace UElements.Profiles
{
    public abstract class ComponentProfileTarget<T> : ProfileTarget
        where T : Component
    {
        [field: SerializeField] public T Value { get; private set; }

        private void Reset()
        {
            Value = GetComponent<T>();
        }
    }
}