using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UElements
{
    public static class UElementsExtensions
    {
        public static string GetKey<T>(ElementRequest? elementRequest) where T : ElementBase
        {
            if (elementRequest.HasValue && !string.IsNullOrEmpty(elementRequest.Value.Key))
            {
                return elementRequest.Value.Key;
            }
            else if (Attribute.GetCustomAttribute(typeof(T), typeof(ElementKeyAttribute)) is { } attribute)
            {
                return ((ElementKeyAttribute)attribute).Key;
            }
            else
            {
                return typeof(T).Name;
            }
        }

        public static string GetKey(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out ElementBase elementBase))
            {
                if (Attribute.GetCustomAttribute(elementBase.GetType(), typeof(ElementKeyAttribute)) is { } attribute)
                {
                    return ((ElementKeyAttribute)attribute).Key;
                }
                else
                {
                    return elementBase.GetType().Name;
                }
            }

            return "null";
        }

        public static bool IsEmptyKey(string key)
        {
            return string.IsNullOrEmpty(key) || key == "null";
        }
        public static string GetResourcesPath(Object asset)
        {
            string fullPath = UnityEditor.AssetDatabase.GetAssetPath(asset);
            int resourcesIndex = fullPath.IndexOf("/Resources/", StringComparison.Ordinal);

            if (resourcesIndex == -1)
            {
                Debug.LogError("Asset is not located in a Resources folder.");
                return null;
            }

            string relativePath = fullPath[(resourcesIndex + "/Resources/".Length)..];
            relativePath = System.IO.Path.ChangeExtension(relativePath, null);
            return relativePath;
        }
    }
}