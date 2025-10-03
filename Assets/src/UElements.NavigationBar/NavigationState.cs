using System;
using System.Collections.Generic;

namespace UElements.NavigationBar
{
    public class NavigationState<TModel> : INavigationState<TModel>
        where TModel : INavigationModel
    {
        public event Action<TModel> PageChanged;

        private readonly Dictionary<string, TModel> m_pages = new();

        public TModel ActivePage { get; private set; }

        public void Register(TModel model)
        {
            m_pages[model.Key] = model;
        }

        public void UnRegister(TModel model)
        {
            m_pages.Remove(model.Key);
        }

        public bool TrySwitch(string key)
        {
            if (ActivePage != null && ActivePage.Key == key) return false;
            if (!m_pages.TryGetValue(key, out TModel entry)) return false;

            ActivePage = entry;
            PageChanged?.Invoke(entry);

            return true;
        }
    }
}