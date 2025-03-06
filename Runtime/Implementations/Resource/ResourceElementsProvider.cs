using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.Resource
{
    public class ResourceElementsProvider : IElementsProvider
    {
        private readonly Dictionary<string, ResourceRequest> m_cache;
        private readonly Dictionary<string, string> m_assetPaths;

        public ResourceElementsProvider(ResourceElementsMapScriptableObject map) : this(map.Maps) { }

        public ResourceElementsProvider(IEnumerable<ElementMap> maps)
        {
            m_cache = new Dictionary<string, ResourceRequest>();
            m_assetPaths = maps.ToDictionary(a => a.Key, a => a.Path);
        }

        public IEnumerable<string> ExistKeys => m_assetPaths.Keys;

        public bool HasElement<T>(string key) where T : ElementBase
        {
            return m_assetPaths.ContainsKey(key);
        }

        public async UniTask<T> GetElement<T>(string key) where T : ElementBase
        {
            string foundPath = m_assetPaths[key];
            if (foundPath == null)
            {
                Debug.LogException(new NullReferenceException($"There is no element found with Key {key}, for Type {typeof(T)}"));
                return null;
            }

            GameObject result;
            if (m_cache.TryGetValue(key, out ResourceRequest request))
            {
                if (request.isDone)
                {
                    result = (GameObject)request.asset;
                }
                else
                {
                    ResourceRequest handle = request;
                    await handle.ToUniTask();
                    result = (GameObject)handle.asset;
                }
            }
            else
            {
                ResourceRequest handle = Resources.LoadAsync(foundPath);
                m_cache[key] = handle;
                await handle.ToUniTask();
                result = (GameObject)handle.asset;
            }

            return result.GetComponent<T>();
        }

        public void Release()
        {
            m_cache.Clear();
        }
    }
}