using System;
using UnityEngine;

namespace UElements.Resource
{
    [Serializable]
    public class ElementMap
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public string Path { get; private set; }
#if UNITY_EDITOR
        [field: SerializeField] public ElementBase Prefab { get; private set; }
        internal void UpdatePath(string path) => Path = path;
#endif
    }
}