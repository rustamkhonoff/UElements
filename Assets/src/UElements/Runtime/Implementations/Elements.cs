using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static UElements.UElementsExtensions;

namespace UElements
{
    public sealed class Elements : IElements, IDisposable
    {
        private readonly IElementsFactory m_elementsFactory;
        private readonly IElementsConfiguration m_elementsConfiguration;
        private readonly Dictionary<string, List<ElementBase>> m_activeElementsCache;
        private readonly Dictionary<string, IElementsProvider> m_providers;

        private ElementsRoot m_elementsRoot;

        public Elements(
            IEnumerable<IElementsProvider> elementsProvider,
            IElementsFactory elementsFactory,
            IElementsConfiguration elementsConfiguration)
        {
            m_elementsFactory = elementsFactory;
            m_elementsConfiguration = elementsConfiguration;
            m_activeElementsCache = new Dictionary<string, List<ElementBase>>();
            m_providers = elementsConfiguration.Modules.ToDictionary(a => a.Key, a => a.ElementsProvider);
            foreach (IElementsProvider provider in elementsProvider)
                m_providers.Add(provider.Key, provider);

            ElementsGlobal.Initialize(this);
        }

        public async UniTask<ElementBase> Create(ElementRequest request, object model, CancellationToken cancellationToken = default)
        {
            ElementBase instance = await Create_Internal<ElementBase>(request, cancellationToken);
            
            if (!ModelElementHelper.TryInitializeModel(instance, model)) 
                Debug.LogWarning($"Trying to create element with model {model.GetType()}");

            instance.Initialize(this);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        public async UniTask<ElementBase> Create(ElementRequest request, CancellationToken cancellationToken = default)
        {
            ElementBase instance = await Create_Internal<ElementBase>(request, cancellationToken);
            instance.Initialize(this);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        public async UniTask<T> Create<T>(ElementRequest? request = null, CancellationToken cancellationToken = default) where T : Element
        {
            T instance = await Create_Internal<T>(request, cancellationToken);
            instance.Initialize(this);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        public async UniTask<T> Create<T, TModel>(TModel model, ElementRequest? request = null, CancellationToken cancellationToken = default)
            where T : ModelElement<TModel>
        {
            T instance = await Create_Internal<T>(request, cancellationToken);
            instance.Initialize(this);
            instance.InitializeModel(model);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        private async UniTask<T> Create_Internal<T>(ElementRequest? request = null, CancellationToken cancellationToken = default)
            where T : ElementBase
        {
            ElementRequest fixedRequest = request ?? ElementRequest.Default;
            string key = GetKey<T>(fixedRequest);

            T prefab = await GetPrefab<T>(key, request, cancellationToken);

            TryHandleRequestSettings(fixedRequest, key);

            T instance = m_elementsFactory.Instantiate(prefab, GetParent(fixedRequest));
            instance.Disposing += () =>
            {
                if (m_activeElementsCache.TryGetValue(key, out List<ElementBase> elementBases))
                    elementBases.Remove(instance);
            };

            AddToCache(instance, key);

            return instance;
        }

        public T GetActive<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements) &&
                cachedElements.Count > 0)
                return (T)cachedElements.First();
            return null;
        }

        public bool HasActive<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
                return cachedElements.Count > 0;
            return false;
        }

        public void CloseAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
                cachedElements.ForEach(a => a.Dispose());
        }

        public List<T> GetAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
                return cachedElements.OfType<T>().ToList();
            return new List<T>();
        }

        public UniTask PrewarmProvider(string moduleKey)
        {
            if (!m_providers.TryGetValue(moduleKey, out IElementsProvider provider))
            {
                Debug.LogException(new NullReferenceException($"No module with key {moduleKey}"));
                return UniTask.CompletedTask;
            }

            return provider.Prewarm();
        }

        public void Release()
        {
            foreach (IElementsProvider elementsProvider in m_providers.Values)
                elementsProvider.Release();
        }

        public void Release(string key)
        {
            if (m_providers.TryGetValue(key, out IElementsProvider provider))
                provider.Release();
        }

        public async UniTask<Type> GetElementTypeForRequest(ElementRequest request)
        {
            object prefab = await GetPrefab<ElementBase>(request.Key, request);
            return prefab.GetType();
        }

        private UniTask<T> GetPrefab<T>(string key, ElementRequest? request, CancellationToken cancellationToken = default) where T : ElementBase
        {
            if (request.HasValue && request.Value.CustomPrefabReference != null && request.Value.CustomPrefabReference.GetComponent<T>() != null)
            {
                return UniTask.FromResult(request.Value.CustomPrefabReference.GetComponent<T>());
            }
            else
            {
                IElementsProvider elementsProvider = m_providers.Values.FirstOrDefault(a => a.HasElement<T>(key));
                if (elementsProvider == null)
                {
                    Debug.LogException(new NullReferenceException($"There is no element with key {key}"));
                    return UniTask.FromResult<T>(null);
                }

                return elementsProvider.GetElement<T>(key, cancellationToken);
            }
        }

        private void AddToCache(ElementBase elementBase, string key)
        {
            if (!m_activeElementsCache.ContainsKey(key))
                m_activeElementsCache[key] = new List<ElementBase>();
            m_activeElementsCache[key].Add(elementBase);
        }

        private void EnsureRootExist()
        {
            if (m_elementsRoot != null) return;
            m_elementsRoot = m_elementsFactory.Instantiate(m_elementsConfiguration.ElementsRootPrefab);
        }

        private void TryHandleRequestSettings(ElementRequest? elementRequest, string key)
        {
            if (!elementRequest.HasValue) return;

            if (elementRequest.Value.OnlyOneInstance &&
                m_activeElementsCache.TryGetValue(key, out List<ElementBase> cachedElements))
            {
                if (cachedElements.Count >= 0)
                    cachedElements.ForEach(a => a.Dispose());
                cachedElements.Clear();
            }
        }

        private Transform GetParent(ElementRequest? elementRequest)
        {
            if (elementRequest.HasValue && elementRequest.Value.Parent != null)
            {
                return elementRequest.Value.Parent;
            }
            else
            {
                EnsureRootExist();
                return m_elementsRoot.Parent;
            }
        }

        public void Dispose()
        {
            foreach (IElementsProvider elementsProvider in m_providers.Values)
                elementsProvider.Release();
            m_providers.Clear();

            ElementsGlobal.Dispose();
        }
    }
}