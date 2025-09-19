using System;
using UnityEngine;

namespace UElements
{
    [Serializable]
    public struct ReferenceElementRequest
    {
        [field: SerializeField] public GameObject CustomPrefabReference { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }

        public static implicit operator ElementRequest(ReferenceElementRequest request) => new(request.CustomPrefabReference, request.Parent);
    }
}