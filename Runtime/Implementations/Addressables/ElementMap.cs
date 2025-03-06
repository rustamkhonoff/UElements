using System;
using UnityEngine;

namespace UElements.Addressables
{
    [Serializable]
    public class ElementMap
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public ElementAssetReference AssetReference { get; private set; }
    }
}