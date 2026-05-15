using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<string, string> m_elementsToProvidersCache;

        private ElementsRoot m_elementsRoot;

        public Elements(
            IEnumerable<IElementsProvider> elementsProvider,
            IElementsFactory elementsFactory,
            IElementsConfiguration elementsConfiguration
        )
        {
            m_elementsFactory = elementsFactory;
            m_elementsConfiguration = elementsConfiguration;
            m_activeElementsCache = new Dictionary<string, List<ElementBase>>();
            m_elementsToProvidersCache = new Dictionary<string, string>();
            m_providers = elementsConfiguration.Modules.ToDictionary(a => a.Key, a => a.ElementsProvider);
            foreach (IElementsProvider provider in elementsProvider)
                m_providers.Add(provider.Key, provider);

            ElementsGlobal.Initialize(this);
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

        public async UniTask CloseAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
            {
                foreach (ElementBase cachedElement in cachedElements)
                    await cachedElement.Close();
                cachedElements.Clear();
            }
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

        public void ReleaseAllProviders()
        {
            foreach (IElementsProvider elementsProvider in m_providers.Values)
                elementsProvider.Release();
        }

        public void ReleaseProvider(string key)
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
                return UniTask.FromResult(request.Value.CustomPrefabReference.GetComponent<T>());

            //Check for previous requests
            if (m_elementsToProvidersCache.TryGetValue(key, out string providerKey))
                return m_providers[providerKey].GetElement<T>(key, cancellationToken);

            IElementsProvider elementsProvider = m_providers.Values.FirstOrDefault(a => a.HasElement<T>(key));
            if (elementsProvider == null)
            {
                Debug.LogException(new NullReferenceException($"There is no element with key {key}"));
                return UniTask.FromResult<T>(null);
            }

            m_elementsToProvidersCache[key] = elementsProvider.Key;
            return elementsProvider.GetElement<T>(key, cancellationToken);
        }

        private void AddToCache(ElementBase elementBase, string key)
        {
            if (!m_activeElementsCache.ContainsKey(key))
                m_activeElementsCache[key] = new List<ElementBase>();
            m_activeElementsCache[key].Add(elementBase);
        }

        private async UniTask ShowElement(ElementBase elementBase)
        {
            await elementBase.Show();
        }

        private Transform GetParent(ElementRequest? elementRequest)
        {
            if (elementRequest.HasValue && elementRequest.Value.Parent != null)
                return elementRequest.Value.Parent;

            EnsureRootExist();
            return m_elementsRoot.Parent;
        }

        private void EnsureRootExist()
        {
            if (m_elementsRoot != null) return;
            m_elementsRoot = m_elementsFactory.Instantiate(m_elementsConfiguration.ElementsRootPrefab);
        }

        public async UniTask<ElementBase> Create(object model, ElementCreateOptions options)
        {
            ElementBase instance = await Create_Internal<ElementBase>(options);

            if (!ModelElementHelper.TryInitializeModel(instance, model))
                Debug.LogWarning($"Trying to create element with model {model.GetType()}");

            await InitializeElement(instance);
            await ShowElement(instance);
            return instance;
        }

        public async UniTask<ElementBase> Create(ElementCreateOptions options)
        {
            ElementBase instance = await Create_Internal<ElementBase>(options);
            await InitializeElement(instance);
            await ShowElement(instance);
            return instance;
        }

        public async UniTask<T> Create<T>(ElementCreateOptions options) where T : Element
        {
            T instance = await Create_Internal<T>(options);
            await InitializeElement(instance);
            await ShowElement(instance);
            return instance;
        }

        public async UniTask<T> Create<T, TModel>(TModel model, ElementCreateOptions options) where T : ModelElement<TModel>
        {
            T instance = await Create_Internal<T>(options);
            instance.InitializeModel(model);
            await InitializeElement(instance);
            await ShowElement(instance);
            return instance;
        }

        private async UniTask InitializeElement(ElementBase elementBase)
        {
            elementBase.InitializeElements(this);
            await elementBase.InitializeAsync();
            // ReSharper disable once MethodHasAsyncOverload
            elementBase.Initialize();
        }

        private async UniTask<T> Create_Internal<T>(ElementCreateOptions options)
            where T : ElementBase
        {
            ElementRequest request = options.Request;
            string key = GetKey<T>(request);

            T prefab = await GetPrefab<T>(key, request, options.CreationToken);

            options.CreationToken.ThrowIfCancellationRequested();

            if (prefab == null)
                throw new InvalidOperationException($"Element prefab not found. Key: {key}");

            await HandleOnlyOneInstance(request, key);

            T instance = m_elementsFactory.Instantiate(prefab, GetParent(request));

            AddToCache(instance, key);
            BindCacheRemoving(instance, key);
            BindCloseToLifetime(instance, options.LifetimeToken);

            return instance;
        }

        private async UniTask HandleOnlyOneInstance(ElementRequest request, string key)
        {
            if (!request.OnlyOneInstance)
                return;

            if (!m_activeElementsCache.TryGetValue(key, out List<ElementBase> cachedElements))
                return;

            for (int i = cachedElements.Count - 1; i >= 0; i--)
                await cachedElements[i].Close();

            cachedElements.Clear();
        }

        private void BindCacheRemoving(ElementBase instance, string key)
        {
            instance.LifetimeToken.Register(() =>
            {
                if (m_activeElementsCache.TryGetValue(key, out List<ElementBase> elements))
                    elements.Remove(instance);
            }, instance);
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