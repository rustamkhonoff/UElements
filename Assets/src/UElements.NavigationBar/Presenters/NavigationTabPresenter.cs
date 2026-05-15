using System;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;

namespace UElements.NavigationBar
{
    public class NavigationTabPresenter<TModel, TView> : ICollectionItemPresenter<TModel>
        where TModel : INavigationModel
        where TView : INavigationTab
    {
        private readonly NavigationState<TModel> m_state;
        private readonly Func<TModel, UniTask<TView>> m_viewFunc;
        public TModel Model { get; }
        public TView View { get; private set; }

        public NavigationTabPresenter(NavigationState<TModel> state, TModel model, Func<TModel, UniTask<TView>> viewFunc)
        {
            m_state = state;
            m_viewFunc = viewFunc;
            Model = model;
        }

        public async UniTask Enable()
        {
            m_state.PageChanged += HandlePageChanged;

            View = await m_viewFunc(Model);
            View.SwitchRequested += OnSwitchRequested;
            View.SetInitialState(m_state.ActivePage != null && Model.Key == m_state.ActivePage.Key);
        }

        private void HandlePageChanged(TModel obj)
        {
            View.SetState(obj != null && obj.Key == Model.Key);
        }

        private void OnSwitchRequested()
        {
            m_state.TrySwitch(Model.Key);
        }

        public async UniTask Disable()
        {
            m_state.PageChanged -= HandlePageChanged;

            if (View != null)
            {
                View.SwitchRequested -= OnSwitchRequested;
                await View.Close();
            }
        }
    }
}