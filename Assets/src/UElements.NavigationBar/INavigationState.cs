using System;

namespace UElements.NavigationBar
{
    public interface INavigationState<TModel>
        where TModel : INavigationModel
    {
        event Action<NavigationEntry<TModel>> PageChanged;
        TModel ActivePage { get; }
        void Register(TModel model, Func<TModel, INavigationContentPresenter> contentBuilder);
        void UnRegister(TModel model);
        bool TrySwitch(string key);
    }
}