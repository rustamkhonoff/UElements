using System;
using UnityEngine.AddressableAssets;

namespace UElements.Addressables
{
    [Serializable]
    public class ElementAssetReference : AssetReferenceGameObject
    {
        public ElementAssetReference(string guid) : base(guid) { }
    }
}