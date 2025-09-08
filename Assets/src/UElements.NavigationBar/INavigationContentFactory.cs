using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public interface INavigationContentFactory<in TModel>
        where TModel : INavigationPageModel
    {
        UniTask<ElementBase> Create(TModel model);
    }
}