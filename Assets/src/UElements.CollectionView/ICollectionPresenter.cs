using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionPresenter<TModel> : IDisposable
    {
        IEnumerable<TModel> Models { get; }
        UniTask Add(TModel model);
        void Remove(TModel model);
        void Clear();
    }
}