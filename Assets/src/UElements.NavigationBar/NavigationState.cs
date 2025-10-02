using System;
using System.Collections.Generic;

namespace UElements.NavigationBar
{
    public readonly struct NavigationEntry<TModel> where TModel : INavigationModel
    {
        public TModel Model { get; }
        public Func<TModel, INavigationContentPresenter> PresenterBuilder { get; }

        public NavigationEntry(TModel model, Func<TModel, INavigationContentPresenter> presenterBuilder)
        {
            Model = model;
            PresenterBuilder = presenterBuilder;
        }
    }

    public class NavigationState<TModel> : INavigationState<TModel>
        where TModel : INavigationModel
    {
        public event Action<NavigationEntry<TModel>> PageChanged;

        private readonly Dictionary<string, NavigationEntry<TModel>> m_pages = new();

        public TModel ActivePage { get; private set; }

        public void Register(TModel model, Func<TModel, INavigationContentPresenter> contentBuilder)
        {
            m_pages[model.Key] = new NavigationEntry<TModel>(model, contentBuilder);
        }

        public void UnRegister(TModel model)
        {
            m_pages.Remove(model.Key);
        }

        public bool TrySwitch(string key)
        {
            if (ActivePage != null && ActivePage.Key == key) return false;
            if (!m_pages.TryGetValue(key, out NavigationEntry<TModel> entry)) return false;

            ActivePage = entry.Model;
            PageChanged?.Invoke(entry);

            return true;
        }
    }
}