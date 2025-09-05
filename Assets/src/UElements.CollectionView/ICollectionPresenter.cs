using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionPresenter<TModel> : IDisposable
    {
        IEnumerable<IModelPresenter<TModel, ModelElement<TModel>>> Presenters { get; }
        IEnumerable<TModel> Models { get; }
        void Initialize(IEnumerable<TModel> data);
        UniTask<ModelElement<TModel>> Add(TModel model);
        void Remove(TModel model);
        void Clear();
    }

    public interface ICollectionPresenter<TModel, out TView> : ICollectionPresenter<TModel>
        where TView : ModelElement<TModel>
    {
        IEnumerable<TView> Views { get; }
    }
}