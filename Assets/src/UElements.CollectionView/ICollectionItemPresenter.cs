using System;

namespace UElements.CollectionView
{
    public interface ICollectionModelPresenter<out TModel, out TView>
        where TView : ModelElement<TModel>
    {
        TModel Model { get; }
        TView View { get; }
        void Initialize();
        void Dispose();
    }

    public class CollectionModelPresenter<TModel, TView> : CollectionModelPresenterBase<TModel, TView> where TView : ModelElement<TModel>
    {
        private readonly Action<TModel, TView> m_init;
        private readonly Action<TModel, TView> m_dispose;

        public CollectionModelPresenter(TModel model, TView view, Action<TModel, TView> init = null, Action<TModel, TView> dispose = null) :
            base(model, view)
        {
            m_init = init;
            m_dispose = dispose;
        }

        public override void Initialize()
        {
            m_init?.Invoke(Model, View);
        }

        public override void Dispose()
        {
            m_dispose?.Invoke(Model, View);
        }
    }

    public abstract class CollectionModelPresenterBase<TModel, TView> : ICollectionModelPresenter<TModel, TView>
        where TView : ModelElement<TModel>
    {
        public TModel Model { get; }
        public TView View { get; }

        protected CollectionModelPresenterBase(TModel model, TView view)
        {
            Model = model;
            View = view;
        }

        public abstract void Initialize();
        public abstract void Dispose();
    }
}