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
        [field: SerializeField] [CanBeNull] public GameObject CustomPrefabReference { get; private set; }

        public ElementRequest(string key, Transform parent, bool onlyOneInstance)
        {
            Key = key;
            Parent = parent;
            OnlyOneInstance = onlyOneInstance;
            CustomPrefabReference = null;
        }

        public ElementRequest(GameObject customPrefabReference, Transform parent)
        {
            Key = null;
            Parent = parent;
            OnlyOneInstance = false;
            CustomPrefabReference = customPrefabReference;
        }

        public ElementRequest(string key)
        {
            Key = key;
            Parent = null;
            OnlyOneInstance = true;
            CustomPrefabReference = null;
        }

        public ElementRequest(Transform parent)
        {
            Key = null;
            Parent = parent;
            OnlyOneInstance = true;
            CustomPrefabReference = null;
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

        public ElementRequest WithCustomPrefab(GameObject prefab)
        {
            CustomPrefabReference = prefab;
            return this;
        }

        public static ElementRequest Default => new ElementRequest().WithOnlyOneInstance(true);
        public static ElementRequest RequestOnlyOneInstance => new ElementRequest().WithOnlyOneInstance(true);
        public static ElementRequest RequestMultipleInstances => new ElementRequest().WithOnlyOneInstance(false);
        public static implicit operator ElementRequest(string key) => new(key);
        public static implicit operator ElementRequest(Transform parent) => new(parent);
        public static implicit operator ElementRequest(GameObject prefab) => new ElementRequest().WithCustomPrefab(prefab);
    }
}