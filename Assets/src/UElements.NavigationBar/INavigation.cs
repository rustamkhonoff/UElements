using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public interface INavigation<TModel> : IDisposable
        where TModel : INavigationModel
    {
        event Action<TModel> PageChanged;
        event Action<TModel, object> ContentCreated;
        TModel ActivePage { get; }
        UniTask Add(TModel model);
        void Remove(TModel model);
        bool TrySwitch(string key);
        bool TrySwitch(TModel model);
    }
}