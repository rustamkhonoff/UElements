using System;
using Cysharp.Threading.Tasks;
using R3;

namespace UElements.NavigationBar
{
    public interface INavigationPresenter<TModel> : IDisposable
        where TModel : INavigationPageModel
    {
        ReadOnlyReactiveProperty<TModel> ActivePage { get; }
        UniTask<bool> TrySwitch(TModel model);
        UniTask<bool> TrySwitch(string key);
        UniTask Add(TModel model);
        void Remove(TModel model);
    }
}