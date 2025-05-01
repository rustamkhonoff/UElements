using System;
using UnityEngine;

namespace UElements.Reference
{
    [Serializable]
    public class ElementMap
    {
        [field: SerializeField] public string Key { get; internal set; }
        [field: SerializeField] public GameObject Prefab { get; internal set; }
    }
}