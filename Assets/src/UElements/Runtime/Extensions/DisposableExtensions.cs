using System;

namespace UElements.Extensions
{
    public static class DisposableExtensions
    {
        public static T AddToElement<T>(this T disposable, ElementBase elementBase) where T : IDisposable
        {
            elementBase.LifetimeToken.Register(disposable.Dispose);
            return disposable;
        }
    }
}