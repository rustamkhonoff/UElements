using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UElements
{
    [Serializable]
    public struct TypedElementRequest
    {
        public enum Type
        {
            Key = 0,
            Reference = 1
        }

        public Type RequestType;
        public string Key;
        public GameObject CustomPrefabReference;
        public Transform Parent;
        public bool OnlyOneInstance;

        public static implicit operator ElementRequest(TypedElementRequest elementRequest)
        {
            if (elementRequest.RequestType is Type.Reference)
                return new ElementRequest(elementRequest.CustomPrefabReference, elementRequest.Parent, elementRequest.OnlyOneInstance);
            else
                return new ElementRequest(elementRequest.Key, elementRequest.Parent, elementRequest.OnlyOneInstance);
        }
    }

    [Serializable]
    public struct ElementRequest
    {
        [field: SerializeField] [CanBeNull] public string Key { get; private set; }
        [field: SerializeField] [CanBeNull] public Transform Parent { get; private set; }
        [field: SerializeField] public bool OnlyOneInstance { get; private set; }
        [field: SerializeField] [CanBeNull] public GameObject CustomPrefabReference { get; private set; }
        [field: SerializeField] public bool AwaitShow { get; private set; }

        public ElementRequest(string key, Transform parent, bool onlyOneInstance)
        {
            Key = key;
            Parent = parent;
            OnlyOneInstance = onlyOneInstance;
            CustomPrefabReference = null;
            AwaitShow = true;
        }

        public ElementRequest(GameObject customPrefabReference, Transform parent)
        {
            Key = null;
            Parent = parent;
            OnlyOneInstance = false;
            CustomPrefabReference = customPrefabReference;
            AwaitShow = true;
        }

        public ElementRequest(GameObject customPrefabReference, Transform parent, bool onlyOneInstance)
        {
            Key = null;
            Parent = parent;
            OnlyOneInstance = onlyOneInstance;
            CustomPrefabReference = customPrefabReference;
            AwaitShow = true;
        }

        public ElementRequest(string key)
        {
            Key = key;
            Parent = null;
            OnlyOneInstance = true;
            CustomPrefabReference = null;
            AwaitShow = true;
        }

        public ElementRequest(Transform parent)
        {
            Key = null;
            Parent = parent;
            OnlyOneInstance = true;
            CustomPrefabReference = null;
            AwaitShow = true;
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

        public ElementRequest WithAwaitShow(bool awaitShow)
        {
            AwaitShow = awaitShow;
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