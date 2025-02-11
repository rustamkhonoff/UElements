using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UElements
{
    [Serializable]
    public struct ElementRequest
    {
        [field: SerializeField] [CanBeNull] public string Key { get; private set; }
        [field: SerializeField] [CanBeNull] public Transform Parent { get; private set; }
        [field: SerializeField] public bool OnlyOneInstance { get; private set; }

        public ElementRequest(string key)
        {
            Key = key;
            Parent = null;
            OnlyOneInstance = true;
        }

        public ElementRequest(Transform parent)
        {
            Key = null;
            Parent = parent;
            OnlyOneInstance = true;
        }

        public ElementRequest WithOnlyOneInstance(bool value)
        {
            OnlyOneInstance = value;
            return this;
        }

        public ElementRequest WithKey(string key)
        {
            Key = key;
            return this;
        }

        public ElementRequest WithParent(Transform parent)
        {
            Parent = parent;
            return this;
        }

        public static ElementRequest Default => new ElementRequest().WithOnlyOneInstance(true);
        public static ElementRequest RequestOnlyOneInstance => new ElementRequest().WithOnlyOneInstance(true);
        public static ElementRequest RequestMultipleInstances => new ElementRequest().WithOnlyOneInstance(false);
        public static implicit operator ElementRequest(string key) => new(key);
        public static implicit operator ElementRequest(Transform parent) => new(parent);
    }
}