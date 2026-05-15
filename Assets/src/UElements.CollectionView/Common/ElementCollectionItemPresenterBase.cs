using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public abstract class ElementCollectionItemPresenterBase<TModel, TView> : ICollectionItemPresenter<TModel>
        where TView : Element
    {
        private readonly Func<TModel, UniTask<TView>> m_viewFactory;
        public TModel Model { get; }
        protected TView View { get; private set; }

        private readonly CancellationTokenSource m_ct = new();
        protected CancellationToken LifetimeToken => m_ct.Token;

        protected ElementCollectionItemPresenterBase(TModel model, Func<TModel, UniTask<TView>> viewFactory)
        {
            m_viewFactory = viewFactory;
            Model = model;
        }

        public virtual async UniTask Enable()
        {
            View = await m_viewFactory.Invoke(Model);
            View.AddTo(m_ct.Token);
        }

        public virtual UniTask Disable()
        {
            m_ct?.Cancel();
            m_ct?.Dispose();

            return UniTask.CompletedTask;
        }
    }
}