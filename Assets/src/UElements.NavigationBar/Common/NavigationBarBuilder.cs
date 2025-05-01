using System;
using System.Collections.Generic;
using UnityEngine;

namespace UElements.NavigationBar
{
    public static class NavigationBarBuilder
    {
        public static void Build<TPageModel, TSwitcherView>(
            IEnumerable<TPageModel> models,
            RectTransform contentParent,
            ElementRequest switcherRequest,
            out INavigationSwitcherContext<TPageModel> navigationState,
            out NavigationPresenter<TPageModel, TSwitcherView> presenter)
            where TPageModel : INavigationPageModel
            where TSwitcherView : NavigationSwitcherView<TPageModel>
        {
            Build(models, contentParent, _ => switcherRequest, out navigationState, out presenter);
        }

        public static void Build<TPageModel, TSwitcherView>(
            IEnumerable<TPageModel> models,
            RectTransform contentParent,
            Func<TPageModel, ElementRequest> switcherRequestFactory,
            out INavigationSwitcherContext<TPageModel> navigationState,
            out NavigationPresenter<TPageModel, TSwitcherView> presenter)
            where TPageModel : INavigationPageModel
            where TSwitcherView : NavigationSwitcherView<TPageModel>
        {
            navigationState = new NavigationContextAdapter<TPageModel>(new NavigationState<TPageModel>(models));

            presenter = new NavigationPresenter<TPageModel, TSwitcherView>(
                navigationState,
                contentParent,
                (model, token) => ElementsGlobal.Elements.Create<TSwitcherView, TPageModel>(model, switcherRequestFactory(model), token)
            );
        }
    }
}