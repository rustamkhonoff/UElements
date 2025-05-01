using System;

namespace UElements.CollectionView
{
    public abstract class CollectionModelPresenterBase<TModel, TView> : IDisposable
        where TView : ModelElement<TModel>
    {
        public TModel Model { get; }
        public TView View { get; }

        public CollectionModelPresenterBase(TModel model, TView view)
        {
            Model = model;
            View = view;
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public void Dispose()
        {
            View.Hide();
            OnDispose();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }
    }
}