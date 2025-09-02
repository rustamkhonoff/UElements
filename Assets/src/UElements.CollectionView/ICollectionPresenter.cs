using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionPresenter<TModel, TView> : IDisposable
        where TView : ModelElement<TModel>
    {
        event Action<TModel, TView> OnCreated;
        event Action<TModel, TView> OnDisposing;
        IEnumerable<TModel> Models { get; }
        IEnumerable<TView> Views { get; }
        Dictionary<TModel, ICollectionItemPresenter<TModel, TView>>.ValueCollection Presenters { get; }
        bool TryGetCollectionItemPresenter(TModel model, out ICollectionItemPresenter<TModel, TView> presenter);
        void Initialize(IEnumerable<TModel> data);
        UniTask<TView> Add(TModel model);
        void Remove(TModel model);
        void Clear();
    }
}