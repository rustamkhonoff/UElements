using System;
using UnityEngine;

namespace UElements.Resource
{
    [Serializable]
    public class ElementMap
    {
        [field: SerializeField] public string Key { get; internal set; }
        [field: SerializeField] public string Path { get; internal set; }
#if UNITY_EDITOR
        [field: SerializeField] public ElementBase Prefab { get; internal set; }
        internal void UpdatePath(string path) => Path = path;
        internal void UpdateKey(string key) => Key = key;
#endif
    }
}