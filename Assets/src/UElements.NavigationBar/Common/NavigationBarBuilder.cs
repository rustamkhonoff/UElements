using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;
using UnityEngine;

namespace UElements.NavigationBar
{
    public static class NavigationBarBuilder
    {
        public static UniTask Build<TModel, TView>(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            ElementRequest switcherRequest,
            out INavigationState<TModel> navigationState)
            where TModel : INavigationPageModel
            where TView : NavigationSwitcherView<TModel>
        {
            return Build<TModel, TView>(models, contentParent, _ => switcherRequest, out navigationState);
        }

        public static UniTask Build<TModel, TView>(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            Func<TModel, ElementRequest> switcherRequestFactory,
            out INavigationState<TModel> navigationState)
            where TModel : INavigationPageModel
            where TView : NavigationSwitcherView<TModel>
        {
            NavigationStateAdapter<TModel> state = new(new NavigationState<TModel>(models));

            CollectionPresenter<TModel, TView> collectionPresenter = new(
                (model, view) => new NavigationSwitcherPresenter<TModel, TView>(model, view, state, contentParent),
                (model, token) => ElementsGlobal.Elements.Create<TView, TModel>(model, switcherRequestFactory(model), token)
            );
            navigationState = state;

            return UniTask.WhenAll(state.Models.Select(a => collectionPresenter.Add(a)));
        }
    }
}