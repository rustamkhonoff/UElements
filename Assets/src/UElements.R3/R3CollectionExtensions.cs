using System;
using ObservableCollections;
using R3;

namespace UElements.R3
{
    public static class R3CollectionExtensions
    {
        public static IDisposable SubscribeEvents<T>(this IObservableCollection<T> source,
            Action<T> onAdd = null,
            Action<T> onRemove = null,
            Action clear = null)
        {
            CompositeDisposable compositeDisposable = new();

            if (onAdd is not null) compositeDisposable.Add(source.ObserveAdd().Subscribe(InvokeOnAdd));
            if (onRemove is not null) compositeDisposable.Add(source.ObserveRemove().Subscribe(InvokeOnRemove));
            if (clear is not null) compositeDisposable.Add(source.ObserveReset().Subscribe(Clear));

            return compositeDisposable;

            void InvokeOnAdd(CollectionAddEvent<T> a) => onAdd(a.Value);
            void InvokeOnRemove(CollectionRemoveEvent<T> a) => onRemove(a.Value);
            void Clear(CollectionResetEvent<T> a) => clear.Invoke();
        }
    }
}