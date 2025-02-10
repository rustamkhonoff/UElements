using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.Resource
{
    public class ResourceElementsProvider : IElementsProvider
    {
        private ResourceElementsMapScriptableObject m_map;
        private readonly Dictionary<string, ResourceRequest> m_cache;

        public ResourceElementsProvider(ResourceElementsMapScriptableObject map)
        {
            m_map = map;
            m_cache = new Dictionary<string, ResourceRequest>();
        }

        public async UniTask<T> GetElement<T>(string key) where T : ElementBase
        {
            ElementMap found = m_map.Maps.FirstOrDefault(a => a.Key == key);
            if (found == null)
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
                ResourceRequest handle = Resources.LoadAsync(found.Path);
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

        public void Dispose()
        {
            Release();
        }
    }
}