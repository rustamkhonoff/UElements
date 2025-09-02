using System;
using Cysharp.Threading.Tasks;
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


        public static IDisposable SubscribeEvents<T1, T2>(this IObservableCollection<T1> source,
            UniTaskOrAction<T1, T2> onAdd = default,
            UniTaskOrAction<T1, T2> onRemove = default,
            UniTaskOrAction<T1, T2> clear = default)
        {
            CompositeDisposable compositeDisposable = new();

            return compositeDisposable;
        }
    }

    public struct UniTaskOrAction<T1, T2>
    {
        public Func<T1, UniTask<T2>> UniTaskFunc { get; set; }

        public UniTaskOrAction(Func<T1, T2> action)
        {
            UniTaskFunc = a => UniTask.FromResult(action.Invoke(a));
        }

        public UniTaskOrAction(Func<T1, UniTask<T2>> uniTaskFunc)
        {
            UniTaskFunc = uniTaskFunc;
        }

        public static implicit operator UniTaskOrAction<T1, T2>(Func<T1, UniTask<T2>> func) => new(func);

        public static implicit operator Func<T1, UniTask<T2>>(UniTaskOrAction<T1, T2> uniTaskOrAction) => uniTaskOrAction.UniTaskFunc;
    }
}