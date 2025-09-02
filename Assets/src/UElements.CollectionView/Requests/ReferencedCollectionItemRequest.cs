using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UElements.CollectionView
{
    [Serializable]
    public struct ReferencedCollectionItemRequest
    {
        [field: SerializeField] [CanBeNull] public GameObject Prefab { get; private set; }
        [field: SerializeField] [CanBeNull] public Transform Parent { get; private set; }

        public static implicit operator ElementRequest(ReferencedCollectionItemRequest request) => new(request.Prefab, request.Parent);
    }
}