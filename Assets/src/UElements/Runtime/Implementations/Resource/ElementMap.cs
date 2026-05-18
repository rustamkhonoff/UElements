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
        [field: SerializeField] public GameObject Prefab { get; internal set; }
        public bool IsValid() => UElementsExtensions.TryGetAssetsFolderPath(Prefab, out _);
#endif
    }
}