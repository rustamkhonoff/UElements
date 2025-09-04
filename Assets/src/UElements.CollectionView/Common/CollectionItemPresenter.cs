using System;

namespace UElements.CollectionView
{
    public class CollectionItemPresenter<TModel, TView> : ICollectionItemPresenter<TModel, TView>
        where TView : ModelElement<TModel>
    {
        private readonly Action<TModel, TView> m_initialize;
        private readonly Action<TModel, TView> m_dispose;
        public TModel Model { get; }
        public TView View { get; }

        public CollectionItemPresenter(
            TModel model, TView view,
            Action<TModel, TView> initialize,
            Action<TModel, TView> dispose)
        {
            Model = model;
            View = view;
            m_initialize = initialize;
            m_dispose = dispose;
        }

        public void Initialize()
        {
            m_initialize?.Invoke(Model, View);
        }

        public void Dispose()
        {
            View.Dispose();
            m_dispose?.Invoke(Model, View);
        }
    }
}