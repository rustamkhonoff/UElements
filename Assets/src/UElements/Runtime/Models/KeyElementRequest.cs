using System;
using UnityEngine;

namespace UElements
{
    [Serializable]
    public struct KeyElementRequest
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public Transform Parent { get; private set; }
        [field: SerializeField] public bool OnlyOneInstance { get; private set; }

        public static implicit operator ElementRequest(KeyElementRequest request) => new(request.Key, request.Parent, request.OnlyOneInstance);
    }
}