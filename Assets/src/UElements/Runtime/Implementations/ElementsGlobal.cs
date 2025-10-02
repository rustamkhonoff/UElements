using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements
{
    public static class ElementsGlobal
    {
        public static Action<IElements> OnInitialized;
        public static Action OnDisposed;
        private static IElements _elements;

        public static bool Initialized { get; private set; }

        public static IElements Instance
        {
            get
            {
                if (!Initialized || _elements == null)
                {
                    Debug.LogError($"{nameof(ElementsGlobal)} not initialized!");
                    throw new InvalidOperationException($"{nameof(ElementsGlobal)} not initialized!");
                }

                return _elements;
            }
            private set => _elements = value;
        }

        internal static void Initialize(IElements elements)
        {
            Initialized = true;
            Instance = elements;
            OnInitialized?.Invoke(Instance);
        }

        internal static void Dispose()
        {
            Initialized = false;
            Instance = null;
            OnDisposed?.Invoke();
        }

        public static UniTask<Type> GetElementTypeForRequest(ElementRequest request) => Instance.GetElementTypeForRequest(request);

        public static UniTask<ElementBase> Create(object model, ElementRequest request, CancellationToken cancellationToken = default)
        {
            return Instance.Create(model, request, cancellationToken);
        }

        public static UniTask<ElementBase> Create(ElementRequest request, CancellationToken cancellationToken = default)
        {
            return Instance.Create(request, cancellationToken);
        }

        public static UniTask<T> Create<T>(ElementRequest? request = null, CancellationToken cancellationToken = default) where T : Element
        {
            return Instance.Create<T>(request, cancellationToken);
        }

        public static UniTask<T> Create<T, TModel>(TModel model, ElementRequest? request = null, CancellationToken token = default)
            where T : ModelElement<TModel>
        {
            return Instance.Create<T, TModel>(model, request, token);
        }

        public static bool HasActive<T>(ElementRequest? request = null) where T : ElementBase
        {
            return Instance.HasActive<T>(request);
        }

        public static T GetActive<T>(ElementRequest? request = null) where T : ElementBase
        {
            return Instance.GetActive<T>(request);
        }

        public static void HideAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            Instance.CloseAll<T>(request);
        }

        public static List<T> GetAll<T>(ElementRequest? request = null) where T : ElementBase
        {
            return Instance.GetAll<T>(request);
        }

        public static UniTask PrewarmProvider(string moduleKey)
        {
            return Instance.PrewarmProvider(moduleKey);
        }

        public static void Release()
        {
            Instance.Release();
        }

        public static void Release(string key)
        {
            Instance.Release(key);
        }
    }
}