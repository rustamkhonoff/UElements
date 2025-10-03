using System;

namespace UElements.NavigationBar
{
    public interface INavigationState<TModel>
        where TModel : INavigationModel
    {
        event Action<TModel> PageChanged;
        TModel ActivePage { get; }
        void Register(TModel model);
        void UnRegister(TModel model);
        bool TrySwitch(string key);
    }
}