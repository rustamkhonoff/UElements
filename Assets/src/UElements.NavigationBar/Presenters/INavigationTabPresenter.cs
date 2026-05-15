using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public interface INavigationTabPresenter
    {
        UniTask Enable();
        UniTask Disable();
    }
}