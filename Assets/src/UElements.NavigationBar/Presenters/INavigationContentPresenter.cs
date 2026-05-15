using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public interface INavigationContentPresenter
    {
        UniTask Enable();
        UniTask Disable();
    }
}