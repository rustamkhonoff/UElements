using System;
using UnityEngine;

namespace UElements.Addressables
{
    [Serializable]
    public class ElementMap
    {
        [field: SerializeField] public string Key { get; internal set; }
        [field: SerializeField] public ElementAssetReference AssetReference { get; internal set; }
    }
}