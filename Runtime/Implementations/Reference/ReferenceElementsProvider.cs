using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.Reference
{
    public class ReferenceElementsProvider : IElementsProvider
    {
        private readonly Dictionary<string, GameObject> m_assetReferences;
        public string Key { get; }

        public ReferenceElementsProvider(string key, IEnumerable<ElementMap> maps)
        {
            Key = key;
            m_assetReferences = maps.ToDictionary(a => a.Key, a => a.Prefab);
        }

        public IEnumerable<string> ExistKeys => m_assetReferences.Keys;

        public bool HasElement<T>(string key) where T : ElementBase
        {
            return m_assetReferences.ContainsKey(key);
        }

        public UniTask<T> GetElement<T>(string key) where T : ElementBase
        {
            GameObject found = m_assetReferences[key];
            if (found == null)
            {
                Debug.LogException(
                    new NullReferenceException($"There is no element found with Key {key}, for Type {typeof(T)}"));
                return UniTask.FromResult<T>(null);
            }

            return UniTask.FromResult(found.GetComponent<T>());
        }

        public UniTask Prewarm()
        {
            return UniTask.CompletedTask;
        }

        public void Release()
        {
            //Nothing
        }
    }
}