using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionModelPresenter<out TModel, out TView>
    {
        TModel Model { get; }
        UniTask Enable();
        UniTask Disable();
    }
}