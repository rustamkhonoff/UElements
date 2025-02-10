using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UElements.Resource
{
    [CreateAssetMenu(menuName = "Services/UElements/Create ResourceElementsMapScriptableObject", fileName = "ResourceElementsMapScriptableObject",
        order = 0)]
    public class ResourceElementsMapScriptableObject : ScriptableObject
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }

#if UNITY_EDITOR
        [field: SerializeField] public bool AutoValidate { get; private set; } = true;

        private void OnValidate()
        {
            if (AutoValidate)
                UpdateElementsPath();
        }

        [ContextMenu("Update Elements Path")]
        private void UpdateElementsPath()
        {
            foreach (ElementMap elementMap in Maps.Where(a => a.Prefab != null))
            {
                string path = GetResourcesPath(elementMap.Prefab);
                elementMap.UpdatePath(path);
            }
        }

        private static string GetResourcesPath(Object asset)
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
#endif
    }
}