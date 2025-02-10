using System;
using UnityEngine.AddressableAssets;

namespace Addressables
{
    [Serializable]
    public class ElementAssetReference : AssetReferenceGameObject
    {
        public ElementAssetReference(string guid) : base(guid) { }
    }
}