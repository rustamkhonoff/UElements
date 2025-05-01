using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;
using UnityEngine;

namespace UElements.NavigationBar
{
    [Serializable]
    public class NavigationPresenter<TPageModel, TSwitcherView> : IDisposable
        where TSwitcherView : NavigationSwitcherView<TPageModel>
        where TPageModel : INavigationPageModel
    {
        private readonly CollectionPresenterBase<TPageModel, TSwitcherView, NavigationSwitcherPresenter<TPageModel, TSwitcherView>>
            m_collectionPresenter;

        public NavigationPresenter(
            INavigationSwitcherContext<TPageModel> state,
            RectTransform contentParent,
            Func<TPageModel, CancellationToken, UniTask<TSwitcherView>> switcherFactory)
        {
            m_collectionPresenter = new NavigationSwitchersCollectionPresenter<TPageModel, TSwitcherView>(
                (model, view) => new NavigationSwitcherPresenter<TPageModel, TSwitcherView>(model, view, state, contentParent),
                switcherFactory
            );

            foreach (TPageModel pageModel in state.Models)
                m_collectionPresenter.Add(pageModel);
        }


        public void Dispose()
        {
            m_collectionPresenter.Dispose();
        }
    }
}