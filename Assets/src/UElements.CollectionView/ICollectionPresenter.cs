using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionPresenter<TModel, TView> : IDisposable
    {
        IEnumerable<TModel> Models { get; }
        UniTask Initialize(IEnumerable<TModel> data);
        UniTask Add(TModel model);
        void Remove(TModel model);
        void Clear();
    }
}