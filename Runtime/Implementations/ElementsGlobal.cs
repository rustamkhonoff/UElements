using System;

namespace UElements
{
    public static class ElementsGlobal
    {
        public static Action<IElements> OnInitialized;
        public static Action OnDisposed;

        public static bool Initialized { get; private set; }
        public static IElements Elements { get; private set; }

        internal static void Initialize(IElements elements)
        {
            Initialized = true;
            Elements = elements;
            OnInitialized?.Invoke(Elements);
        }

        internal static void Dispose()
        {
            Initialized = false;
            Elements = null;
            OnDisposed?.Invoke();
        }
    }
}