using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    internal class NavigationPresenter<TModel> : IDisposable, INavigationPresenter
        where TModel : INavigationModel
    {
        public event Action<object> ContentCreated;

        private readonly NavigationState<TModel> m_state;
        private readonly Func<TModel, INavigationContentPresenter> m_presenterBuilder;
        private INavigationContentPresenter m_activePresenter;

        public NavigationPresenter(NavigationState<TModel> state, Func<TModel, INavigationContentPresenter> presenterBuilder)
        {
            m_state = state;
            m_presenterBuilder = presenterBuilder;
            m_state.PageChanged += OnActivePageChanged;
        }

        private async void OnActivePageChanged(TModel model)
        {
            if (m_activePresenter != null)
                await m_activePresenter.Disable();

            INavigationContentPresenter newPresenter = m_presenterBuilder(model);

            m_activePresenter = newPresenter ?? throw new NullReferenceException($"Can't create content presenter for model {model.Key}");

            await m_activePresenter.Enable();
            ContentCreated?.Invoke(m_activePresenter);
        }

        public bool TrySwitch(string key)
        {
            return m_state.TrySwitch(key);
        }

        public void Dispose()
        {
            if (m_activePresenter != null)
                m_activePresenter.Disable().Forget();
            m_state.PageChanged -= OnActivePageChanged;
        }
    }
}