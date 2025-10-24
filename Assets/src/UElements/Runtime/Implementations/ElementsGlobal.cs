using System;
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
    }
}