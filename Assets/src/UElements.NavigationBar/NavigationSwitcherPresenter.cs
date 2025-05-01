using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UElements.CollectionView;
using UnityEngine;

namespace UElements.NavigationBar
{
    public class NavigationSwitcherPresenter<TPageModel, TSwitcherView> : CollectionModelPresenterBase<TPageModel, TSwitcherView>
        where TPageModel : INavigationPageModel
        where TSwitcherView : NavigationSwitcherView<TPageModel>
    {
        private readonly RectTransform m_contentParent;
        private readonly CancellationTokenSource m_cancellationTokenSource = new();
        private ElementBase m_elementBase;
        private readonly INavigationSwitcherContext<TPageModel> m_navigationState;

        public ReadOnlyReactiveProperty<bool> IsSelected => m_navigationState.ActivePage.Select(a => a.Key == Model.Key).ToReadOnlyReactiveProperty();

        public NavigationSwitcherPresenter(
            TPageModel model, TSwitcherView view,
            INavigationSwitcherContext<TPageModel> navigationState,
            RectTransform contentParent)
            : base(model, view)
        {
            m_navigationState = navigationState;
            m_contentParent = contentParent;
        }

        protected override void OnInitialize()
        {
            m_navigationState.ActivePage.Where(a => a != null).Subscribe(HandleActivePageChanged).AddTo(m_cancellationTokenSource.Token);

            View.OnSwitchRequest.Subscribe(a => m_navigationState.TrySwitch(a)).AddTo(m_cancellationTokenSource.Token);
            IsSelected.Subscribe(View.SetSelected).AddTo(m_cancellationTokenSource.Token);
        }

        private async void HandleActivePageChanged(TPageModel model)
        {
            bool isMe = Model.Key == model.Key;
            View.SetSelected(isMe);

            if (Model.Key == model.Key && m_elementBase == null)
            {
                m_elementBase = await ElementsGlobal.Elements.Create(model.ContentRequest.WithParent(m_contentParent));
            }
            else if (m_elementBase != null)
            {
                m_elementBase.Hide();
                m_elementBase = null;
            }
        }

        protected override void OnDispose()
        {
            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource?.Dispose();
        }
    }
}