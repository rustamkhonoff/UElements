using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.Resource
{
    public class ResourceElementsProvider : IElementsProvider
    {
        private readonly Dictionary<string, ResourceRequest> m_cache;
        private readonly Dictionary<string, string> m_assetPaths;

        public string Key { get; }

        public ResourceElementsProvider(string key, IEnumerable<ElementMap> maps)
        {
            Key = key;
            m_cache = new Dictionary<string, ResourceRequest>();
            m_assetPaths = maps.ToDictionary(a => a.Key, a => a.Path);
        }

        public IEnumerable<string> ExistKeys => m_assetPaths.Keys;

        public bool HasElement<T>(string key) where T : ElementBase
        {
            return m_assetPaths.ContainsKey(key);
        }

        public async UniTask<T> GetElement<T>(string key, CancellationToken cancellationToken = default) where T : ElementBase
        {
            string foundPath = m_assetPaths[key];
            if (foundPath == null)
            {
                Debug.LogException(
                    new NullReferenceException($"There is no element found with Key {key}, for Type {typeof(T)}"));
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
                    await handle.ToUniTask(cancellationToken: cancellationToken);
                    result = (GameObject)handle.asset;
                }
            }
            else
            {
                ResourceRequest handle = Resources.LoadAsync(foundPath);
                m_cache[key] = handle;
                await handle.ToUniTask(cancellationToken: cancellationToken);
                result = (GameObject)handle.asset;
            }

            return result.GetComponent<T>();
        }

        public async UniTask Prewarm()
        {
            foreach (var item in m_assetPaths)
            {
                if (m_cache.TryGetValue(item.Key, out ResourceRequest request))
                {
                    if (!request.isDone)
                    {
                        ResourceRequest handle = request;
                        await handle.ToUniTask();
                    }
                }
                else
                {
                    ResourceRequest handle = Resources.LoadAsync(item.Value);
                    m_cache[item.Key] = handle;
                    await handle.ToUniTask();
                }
            }
        }

        public void Release()
        {
            m_cache.Clear();
        }
    }
}