using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UElements.CollectionView
{
    [Serializable]
    public struct CollectionItemRequest
    {
        [field: SerializeField] [CanBeNull] public string Key { get; private set; }
        [field: SerializeField] [CanBeNull] public Transform Parent { get; private set; }

        public static implicit operator ElementRequest(CollectionItemRequest request) => new(request.Key, request.Parent, false);
    }
}