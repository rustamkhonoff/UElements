using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UElements.Addressables
{
    public class AddressableElementsProvider : IElementsProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> m_cache;
        private readonly Dictionary<string, ElementAssetReference> m_assetReferences;

        public AddressableElementsProvider(AddressableElementsMapScriptableObject map) : this(map.Maps) { }

        public AddressableElementsProvider(IEnumerable<ElementMap> maps)
        {
            m_cache = new Dictionary<string, AsyncOperationHandle>();
            m_assetReferences = maps.ToDictionary(a => a.Key, a => a.AssetReference);
        }

        public IEnumerable<string> ExistKeys => m_assetReferences.Keys;

        public bool HasElement<T>(string key) where T : ElementBase
        {
            return m_assetReferences.ContainsKey(key);
        }

        public async UniTask<T> GetElement<T>(string key) where T : ElementBase
        {
            ElementAssetReference found = m_assetReferences[key];
            if (found == null)
            {
                Debug.LogException(new NullReferenceException($"There is no element found with Key {key}, for Type {typeof(T)}"));
                return null;
            }

            GameObject result;
            AsyncOperationHandle handle;

            if (m_cache.TryGetValue(key, out AsyncOperationHandle cachedHandle))
            {
                handle = cachedHandle;
                if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
                    await handle.ToUniTask();
            }
            else
            {
                if (found.OperationHandle.IsValid()) handle = found.OperationHandle;
                else handle = found.LoadAssetAsync<GameObject>();

                m_cache[key] = handle;
                await handle.ToUniTask();
            }

            result = (GameObject)handle.Result;
            return result.GetComponent<T>();
        }


        public void Release()
        {
            foreach (AsyncOperationHandle asyncOperationHandle in m_cache.Values.Where(a => a.IsValid()))
                UnityEngine.AddressableAssets.Addressables.Release(asyncOperationHandle);

            m_cache.Clear();
        }
    }
}