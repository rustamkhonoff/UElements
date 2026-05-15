using System;
using System.Threading;

namespace UElements
{
    public static class DisposableExtensions
    {
        public static T AddTo<T>(this T disposable, ElementBase elementBase) where T : IDisposable
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
                elementBase.SafeDispose();
                return new CancellationTokenRegistration();
            }

            return ct.Register(elementBase.SafeDispose);
        }
    }
}