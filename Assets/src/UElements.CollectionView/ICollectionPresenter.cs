using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionPresenter<TModel> : IDisposable
    {
        IEnumerable<ICollectionModelPresenter<TModel, ModelElement<TModel>>> Presenters { get; }
        IEnumerable<TModel> Models { get; }
        UniTask Initialize(IEnumerable<TModel> data);
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