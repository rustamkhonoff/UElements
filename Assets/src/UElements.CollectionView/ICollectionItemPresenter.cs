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