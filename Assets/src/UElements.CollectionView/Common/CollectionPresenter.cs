using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public class CollectionPresenter<TModel, TView> : CollectionPresenterBase<TModel, TView, CollectionItemModelPresenter<TModel, TView>>
        where TView : ModelElement<TModel>
    {
        public CollectionPresenter(
            Func<TModel, TView, CollectionItemModelPresenter<TModel, TView>> presenterFactory,
            Func<TModel, CancellationToken, UniTask<TView>> viewFactory)
            : base(presenterFactory, viewFactory) { }
    }
}