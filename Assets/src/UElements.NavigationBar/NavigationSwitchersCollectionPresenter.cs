using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;

namespace UElements.NavigationBar
{
    public class NavigationSwitchersCollectionPresenter<TPageModel, TSwitcherView> :
        CollectionPresenterBase<TPageModel, TSwitcherView, NavigationSwitcherPresenter<TPageModel, TSwitcherView>>
        where TSwitcherView : NavigationSwitcherView<TPageModel>
        where TPageModel : INavigationPageModel
    {
        public NavigationSwitchersCollectionPresenter(
            Func<TPageModel, TSwitcherView, NavigationSwitcherPresenter<TPageModel, TSwitcherView>> presenterFactory,
            Func<TPageModel, CancellationToken, UniTask<TSwitcherView>> viewFactory) : base(presenterFactory, viewFactory) { }
    }
}