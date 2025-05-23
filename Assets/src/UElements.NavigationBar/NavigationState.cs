using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace UElements.NavigationBar
{
    public interface INavigationSwitcherContext<TPageModel>
    {
        ReadOnlyReactiveProperty<TPageModel> ActivePage { get; }
        IReadOnlyCollection<TPageModel> Models { get; }
        bool TrySwitch(TPageModel model);
    }

    public class NavigationContextAdapter<TPageModel> : INavigationSwitcherContext<TPageModel> where TPageModel : INavigationPageModel
    {
        private readonly NavigationState<TPageModel> m_state;
        public NavigationContextAdapter(NavigationState<TPageModel> state) => m_state = state;
        public ReadOnlyReactiveProperty<TPageModel> ActivePage => m_state.ActivePage;
        public IReadOnlyCollection<TPageModel> Models => m_state.Models;
        public bool TrySwitch(TPageModel model) => m_state.TrySwitch(model);
    }

    public class NavigationState<TPageModel> where TPageModel : INavigationPageModel
    {
        private readonly Dictionary<string, TPageModel> m_models = new();
        private readonly Dictionary<TPageModel, string> m_reverseModelsLookup = new();
        private readonly ReactiveProperty<TPageModel> m_activePage = new();

        public ReadOnlyReactiveProperty<TPageModel> ActivePage => m_activePage;

        public IReadOnlyCollection<TPageModel> Models => m_models.Values;

        public NavigationState(IEnumerable<TPageModel> pages)
        {
            foreach (TPageModel pageModel in pages)
            {
                if (!m_models.TryAdd(pageModel.Key, pageModel))
                    Debug.LogWarning(new InvalidOperationException("Detected 2 or more models with similar keys"));
            }

            foreach ((string key, TPageModel value) in m_models)
            {
                m_reverseModelsLookup[value] = key;
            }
        }

        public bool TrySwitch(string key)
        {
            return m_models.TryGetValue(key, out TPageModel model) && TrySwitch(model);
        }

        public bool TrySwitch(TPageModel model)
        {
            if (model == null || !m_reverseModelsLookup.ContainsKey(model)) return false;
            if (m_activePage.Value != null && m_activePage.Value.Key == model.Key) return false;

            m_activePage.Value = model;

            return true;
        }
    }
}