using System;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;

namespace UElements.NavigationBar
{
    public class NavigationTabPresenter<TModel, TView> : ICollectionModelPresenter<TModel, TView>
        where TModel : INavigationModel
        where TView : INavigationTab
    {
        private readonly INavigationState<TModel> m_state;
        private readonly Func<TModel, UniTask<TView>> m_viewFunc;

        public NavigationTabPresenter(INavigationState<TModel> state, TModel model, Func<TModel, UniTask<TView>> viewFunc)
        {
            m_state = state;
            m_viewFunc = viewFunc;
            Model = model;
        }

        public TModel Model { get; }
        public TView View { get; private set; }

        public async UniTask Enable()
        {
            View = await m_viewFunc(Model);

            m_state.PageChanged += HandlePageChanged;

            View.RegisterRequest(OnSwitchRequested);
            View.SetInitialState(m_state.ActivePage != null && Model.Key == m_state.ActivePage.Key);
        }

        private void HandlePageChanged(NavigationEntry<TModel> obj)
        {
            View.SetState(obj.Model != null && obj.Model.Key == Model.Key);
        }

        private void OnSwitchRequested()
        {
            m_state.TrySwitch(Model.Key);
        }

        public UniTask Disable()
        {
            m_state.PageChanged -= HandlePageChanged;
            View.Dispose();

            return UniTask.CompletedTask;
        }
    }
}