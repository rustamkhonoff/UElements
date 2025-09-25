using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public static class DisposableExtensions
    {
        public static T AddToElement<T>(this T disposable, ElementBase elementBase) where T : IDisposable
        {
            elementBase.LifetimeToken.Register(disposable.Dispose);
            return disposable;
        }

        public static CancellationTokenRegistration AddTo(this ElementBase elementBase, CancellationToken ct)
        {
            if (!ct.CanBeCanceled)
                throw new ArgumentException("Require CancellationToken CanBeCanceled");
            if (ct.IsCancellationRequested)
            {
                elementBase.Dispose();
                return new CancellationTokenRegistration();
            }

            return ct.Register(elementBase.Dispose);
        }

        public static CancellationTokenRegistration AddTo(this ElementBase elementBase, CancellationTokenSource cts)
        {
            return elementBase.AddTo(cts.Token);
        }
    }

    public static class ElementsExtensions
    {
        public static void Close<T>(this IElements elements) where T : ElementBase => elements.CloseAll<T>();
    }

    public static class ElementExtensions
    {
        public static void SafeDispose(this ElementBase elementBase)
        {
            if (elementBase == null) return;
            elementBase.Dispose();
        }

        public static UniTask Show(this ElementBase elementBase) => elementBase.Show(null);
        public static UniTask Close(this ElementBase elementBase) => elementBase.Close(null);
        public static UniTask Hide(this ElementBase elementBase) => elementBase.Hide(null);
    }
}