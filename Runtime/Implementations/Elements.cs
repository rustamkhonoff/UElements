using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements
{
    public class Elements : IElements
    {
        private readonly IElementsProvider m_elementsProvider;
        private readonly IElementsFactory m_elementsFactory;
        private readonly IElementsConfiguration m_elementsConfiguration;
        private ElementsRoot m_elementsRoot;
        private readonly Dictionary<string, List<ElementBase>> m_activeElementsCache;

        public Elements(IElementsProvider elementsProvider, IElementsFactory elementsFactory, IElementsConfiguration elementsConfiguration)
        {
            m_elementsProvider = elementsProvider;
            m_elementsFactory = elementsFactory;
            m_elementsConfiguration = elementsConfiguration;
            m_activeElementsCache = new Dictionary<string, List<ElementBase>>();
        }

        public async UniTask<ElementBase> Create(ElementRequest request)
        {
            ElementBase instance = await Create_Internal<ElementBase>(request);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        public async UniTask<T> Create<T>(ElementRequest? request = null) where T : Element
        {
            T instance = await Create_Internal<T>(request);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        public async UniTask<T> Create<T, TModel>(TModel model, ElementRequest? request = null) where T : ModelElement<TModel>
        {
            T instance = await Create_Internal<T>(request);
            instance.InitializeModel(model);
            instance.Initialize();
            instance.Show();
            return instance;
        }

        private async UniTask<T> Create_Internal<T>(ElementRequest? request = null) where T : ElementBase
        {
            ElementRequest fixedRequest = request != null ? request.Value : ElementRequest.Default;
            string key = GetKey<T>(fixedRequest);

            T prefab = await m_elementsProvider.GetElement<T>(key);
            if (prefab == null) return null;

            TryHandleRequestSettings(fixedRequest, key);

            T instance = m_elementsFactory.Instantiate(prefab, GetParent(fixedRequest));

            AddToCache(instance, key);

            return instance;
        }

        public T GetActive<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements) && cachedElements.Count > 0)
                return (T)cachedElements.First();
            return null;
        }

        public bool HasActive<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
                return cachedElements.Count > 0;
            return false;
        }

        public void HideAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
                cachedElements.ForEach(a => a.Hide());
        }

        public List<T> GetAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            if (m_activeElementsCache.TryGetValue(GetKey<T>(request), out List<ElementBase> cachedElements))
                return cachedElements.OfType<T>().ToList();
            return new List<T>();
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

            if (elementRequest.Value.OnlyOneInstance && m_activeElementsCache.TryGetValue(key, out List<ElementBase> cachedElements))
            {
                if (cachedElements.Count >= 0)
                    cachedElements.ForEach(a => a.Hide());
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

        private string GetKey<T>(ElementRequest? elementRequest) where T : ElementBase
        {
            if (elementRequest.HasValue && !string.IsNullOrEmpty(elementRequest.Value.Key))
            {
                return elementRequest.Value.Key;
            }
            else if (Attribute.GetCustomAttribute(typeof(T), typeof(ElementKeyAttribute)) is { } attribute)
            {
                return ((ElementKeyAttribute)attribute).Key;
            }
            else
            {
                return typeof(T).Name;
            }
        }
    }
}