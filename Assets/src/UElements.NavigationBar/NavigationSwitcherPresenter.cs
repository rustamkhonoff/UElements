using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UElements.CollectionView;
using UnityEngine;

namespace UElements.NavigationBar
{
    public class NavigationSwitcherPresenter<TModel, TView> : IModelPresenter<TModel, TView>
        where TModel : INavigationPageModel
        where TView : NavigationSwitcherViewBase<TModel>
    {
        public TModel Model { get; }
        public TView View { get; }

        private readonly RectTransform m_pagesParent;
        private readonly CancellationTokenSource m_cancellationTokenSource = new();
        private readonly INavigationState<TModel> m_navigationState;
        private ElementBase m_activePage;

        public ReadOnlyReactiveProperty<bool> IsSelected => m_navigationState.ActivePage.Select(a => a.Key == Model.Key).ToReadOnlyReactiveProperty();

        public NavigationSwitcherPresenter(
            TModel model, TView view,
            INavigationState<TModel> navigationState,
            RectTransform pagesParent)
        {
            Model = model;
            View = view;
            m_navigationState = navigationState;
            m_pagesParent = pagesParent;
        }

        public void Initialize()
        {
            m_navigationState.ActivePage
                .Where(a => a != null)
                .Subscribe(a => HandleActivePageChanged(a, a.Key == Model.Key).Forget())
                .AddTo(m_cancellationTokenSource.Token);

            View.OnSwitchRequest.Subscribe(a => m_navigationState.TrySwitch(a)).AddTo(m_cancellationTokenSource.Token);
            IsSelected.Subscribe(View.SetSelected).AddTo(m_cancellationTokenSource.Token);
        }

        private async UniTask HandleActivePageChanged(TModel model, bool selected)
        {
            View.SetSelected(selected);

            if (selected && m_activePage == null)
            {
                m_activePage = await CreatePage(model);
                m_activePage.AddTo(m_cancellationTokenSource.Token);
            }
            else if (m_activePage != null)
            {
                m_activePage.SafeDispose();
                m_activePage = null;
            }
        }

        private UniTask<ElementBase> CreatePage(TModel model) => ElementsGlobal.Create(model.ContentRequest.WithParent(m_pagesParent));

        public void Dispose()
        {
            View.SafeDispose();
            m_activePage.SafeDispose();

            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource?.Dispose();
        }
    }
}