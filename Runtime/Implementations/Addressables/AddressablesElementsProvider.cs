using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UElements;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Addressables
{
    public class AddressableElementsProvider : IElementsProvider
    {
        private readonly AddressableElementsMapScriptableObject m_map;
        private readonly Dictionary<string, AsyncOperationHandle> m_cache;

        public AddressableElementsProvider(AddressableElementsMapScriptableObject map)
        {
            m_map = map;
            m_cache = new Dictionary<string, AsyncOperationHandle>();
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
            if (m_cache.TryGetValue(key, out AsyncOperationHandle asyncOperationHandle))
            {
                if (asyncOperationHandle.Status is AsyncOperationStatus.Succeeded)
                {
                    result = (GameObject)asyncOperationHandle.Result;
                }
                else
                {
                    AsyncOperationHandle handle = asyncOperationHandle;
                    await handle.ToUniTask();
                    result = (GameObject)handle.Result;
                }
            }
            else
            {
                AsyncOperationHandle handle = found.AssetReference.LoadAssetAsync();
                m_cache[key] = handle;
                await handle.ToUniTask();
                result = (GameObject)handle.Result;
            }

            return result.GetComponent<T>();
        }

        public void Release()
        {
            foreach (AsyncOperationHandle asyncOperationHandle in m_cache.Values.Where(a => a.IsValid()))
                UnityEngine.AddressableAssets.Addressables.Release(asyncOperationHandle);

            m_cache.Clear();
        }

        public void Dispose()
        {
            Release();
        }
    }
}