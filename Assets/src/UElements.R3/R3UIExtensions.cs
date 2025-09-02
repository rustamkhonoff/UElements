using System;
using R3;
using TMPro;
using UnityEngine.UI;

namespace UElements.R3
{
    public static class R3UIExtensions
    {
        public static IDisposable SubscribeSingleClick(this Button source, Action onNext)
        {
            void OnNext(Unit unit) => onNext?.Invoke();
            return source.OnClickAsObservable().Take(1).Subscribe(OnNext);
        }

        public static IDisposable SubscribeClick(this Button source, Action onNext)
        {
            void OnNext(Unit unit) => onNext?.Invoke();
            return source.OnClickAsObservable().Subscribe(OnNext);
        }

        public static IDisposable SubscribeClickWithState<T>(this Button source, Action<T> onNext, T state)
        {
            return source.OnClickAsObservable().Subscribe(_ => onNext?.Invoke(state));
        }

        public static IDisposable SubscribeClickWithState<T>(this Button source, Action<T> onNext, Func<T> state)
        {
            return source.OnClickAsObservable().Subscribe(_ => onNext?.Invoke(state()));
        }

        public static IDisposable SubscribeClickWithState<T>(this Button source, Action<T> onNext, ModelElement<T> model)
        {
            return source.OnClickAsObservable().Subscribe(_ => onNext?.Invoke(model.Model));
        }

        public static IDisposable SubscribeClick(this Button source, Action onNext, Func<bool> valid)
        {
            void OnNext(Unit unit) => onNext?.Invoke();
            bool IsValid(Unit unit) => valid.Invoke();
            return source.OnClickAsObservable().Where(IsValid).Subscribe(OnNext);
        }

        public static IDisposable SubscribeEndEdit(this TMP_InputField source, Action<string> onNext) =>
            source.onEndEdit.AsObservable().Subscribe(onNext);

        public static IDisposable SubscribeValueChanged(this TMP_InputField source, Action<string> onNext) =>
            source.onValueChanged.AsObservable().Subscribe(onNext);
    }
}