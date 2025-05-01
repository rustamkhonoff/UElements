using System;
using R3;

namespace UElements.R3
{
    public static class R3DisposableExtensions
    {
        public static IDisposable SubscribeCallback<T>(this Observable<T> source, Action onNext)
        {
            void Invoke(T a) => onNext?.Invoke();
            return source.Subscribe(Invoke);
        }
    }
}