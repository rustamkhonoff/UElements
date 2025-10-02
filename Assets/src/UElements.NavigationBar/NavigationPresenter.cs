using System;

namespace UElements.NavigationBar
{
    internal class NavigationPresenter<TModel> : IDisposable, INavigationPresenter
        where TModel : INavigationModel
    {
        public event Action<object> ContentCreated;

        private readonly INavigationState<TModel> m_state;
        private INavigationContentPresenter m_activePresenter;

        public NavigationPresenter(INavigationState<TModel> state)
        {
            m_state = state;
            m_state.PageChanged += OnActivePageChanged;
        }

        private async void OnActivePageChanged(NavigationEntry<TModel> entry)
        {
            m_activePresenter?.Disable();
            m_activePresenter = entry.PresenterBuilder(entry.Model);
            await m_activePresenter.Enable();
            ContentCreated?.Invoke(m_activePresenter);
        }

        public bool TrySwitch(string key)
        {
            return m_state.TrySwitch(key);
        }

        public void Dispose()
        {
            m_activePresenter?.Disable();
            m_state.PageChanged -= OnActivePageChanged;
        }
    }
}