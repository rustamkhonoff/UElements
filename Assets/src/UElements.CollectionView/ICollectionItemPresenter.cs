namespace UElements.CollectionView
{
    public interface ICollectionItemPresenter<out TModel, out TView>
        where TView : ModelElement<TModel>
    {
        TModel Model { get; }
        TView View { get; }
        void Initialize();
        void Dispose();
    }
}