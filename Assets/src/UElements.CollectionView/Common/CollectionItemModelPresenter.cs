using System;

namespace UElements.CollectionView
{
    public class CollectionItemModelPresenter<TModel, TView> : CollectionModelPresenterBase<TModel, TView> where TView : ModelElement<TModel>
    {
        private readonly Action<TModel, TView> m_initialize;
        private readonly Action<TModel, TView> m_dispose;

        public CollectionItemModelPresenter(
            TModel model, TView view,
            Action<TModel, TView> initialize,
            Action<TModel, TView> dispose)
            : base(model, view)
        {
            m_initialize = initialize;
            m_dispose = dispose;
        }

        protected override void OnInitialize()
        {
            m_initialize?.Invoke(Model, View);
        }

        protected override void OnDispose()
        {
            m_dispose?.Invoke(Model, View);
        }
    }
}