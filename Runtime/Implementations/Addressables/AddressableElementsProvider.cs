using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UElements.Addressables
{
    public class AddressableElementsProvider : IElementsProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> m_cache;
        private readonly Dictionary<string, ElementAssetReference> m_assetReferences;

        public string Key { get; }

        public AddressableElementsProvider(string key, IEnumerable<ElementMap> maps)
        {
            Key = key;
            m_cache = new Dictionary<string, AsyncOperationHandle>();
            m_assetReferences = maps.ToDictionary(a => a.Key, a => a.AssetReference);
        }

        public IEnumerable<string> ExistKeys => m_assetReferences.Keys;

        public bool HasElement<T>(string key) where T : ElementBase
        {
            return m_assetReferences.ContainsKey(key);
        }

        public async UniTask<T> GetElement<T>(string key, CancellationToken cancellationToken = default) where T : ElementBase
        {
            ElementAssetReference found = m_assetReferences[key];
            if (found == null)
            {
                Debug.LogException(
                    new NullReferenceException($"There is no element found with Key {key}, for Type {typeof(T)}"));
                return null;
            }

            GameObject result;
            AsyncOperationHandle handle;

            if (m_cache.TryGetValue(key, out AsyncOperationHandle cachedHandle))
            {
                handle = cachedHandle;
                if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
                    await handle.ToUniTask(cancellationToken: cancellationToken);
            }
            else
            {
                if (found.OperationHandle.IsValid()) handle = found.OperationHandle;
                else handle = found.LoadAssetAsync<GameObject>();

                m_cache[key] = handle;
                await handle.ToUniTask(cancellationToken: cancellationToken);
            }

            result = (GameObject)handle.Result;
            return result.GetComponent<T>();
        }

        public async UniTask Prewarm()
        {
            foreach (var item in m_assetReferences)
            {
                AsyncOperationHandle handle;

                if (m_cache.TryGetValue(item.Key, out AsyncOperationHandle cachedHandle))
                {
                    handle = cachedHandle;
                    if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
                        await handle.ToUniTask();
                }
                else
                {
                    if (item.Value.OperationHandle.IsValid()) handle = item.Value.OperationHandle;
                    else handle = item.Value.LoadAssetAsync<GameObject>();

                    m_cache[item.Key] = handle;
                    await handle.ToUniTask();
                }
            }
        }


        public void Release()
        {
            foreach (AsyncOperationHandle asyncOperationHandle in m_cache.Values.Where(a => a.IsValid()))
                UnityEngine.AddressableAssets.Addressables.Release(asyncOperationHandle);

            m_cache.Clear();
        }
    }
}