using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public class ElementCollectionItemPresenter<TModel, TView> : ICollectionModelPresenter<TModel, TView>
        where TView : Element
    {
        private readonly Func<TModel, UniTask<TView>> m_viewFactory;
        public TModel Model { get; }
        protected TView View { get; private set; }

        private readonly CancellationTokenSource m_ct = new();
        protected CancellationToken LifetimeToken => m_ct.Token;

        public ElementCollectionItemPresenter(TModel model, Func<TModel, UniTask<TView>> viewFactory)
        {
            m_viewFactory = viewFactory;
            Model = model;
        }

        public virtual async UniTask Enable()
        {
            View = await m_viewFactory.Invoke(Model);
            View.AddTo(m_ct);
        }

        public virtual UniTask Disable()
        {
            m_ct?.Cancel();
            m_ct?.Dispose();

            return UniTask.CompletedTask;
        }
    }
}