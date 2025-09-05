namespace UElements.CollectionView
{
    public interface IModelPresenter<out TModel, out TView>
        where TView : ModelElement<TModel>
    {
        TModel Model { get; }
        TView View { get; }
        void Initialize();
        void Dispose();
    }
    public abstract class ModelPresenterBase<TModel, TView> : IModelPresenter<TModel, TView>
        where TView : ModelElement<TModel>
    {
        public TModel Model { get; }
        public TView View { get; }

        protected ModelPresenterBase(TModel model, TView view)
        {
            Model = model;
            View = view;
        }

        public abstract void Initialize();
        public abstract void Dispose();
    }
}