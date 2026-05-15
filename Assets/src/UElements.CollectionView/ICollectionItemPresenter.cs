using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public interface ICollectionItemPresenter<out TModel>
    {
        TModel Model { get; }
        UniTask Enable();
        UniTask Disable();
    }
}