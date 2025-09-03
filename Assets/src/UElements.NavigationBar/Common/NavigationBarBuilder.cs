using System.Collections.Generic;
using UElements.CollectionView;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

namespace UElements.NavigationBar
{
    public static class NavigationBarBuilder<TModel>
        where TModel : INavigationPageModel
    {
        public static UniTask<INavigationState<TModel>> Build(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            ElementRequest switcherRequest)
        {
            return NavigationBarBuilder.Build<TModel, NavigationSwitcherView<TModel>>(models, contentParent, _ => switcherRequest);
        }

        public static UniTask<INavigationState<TModel>> Build(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            Func<TModel, ElementRequest> switcherRequest)
        {
            return NavigationBarBuilder.Build<TModel, NavigationSwitcherView<TModel>>(models, contentParent, switcherRequest);
        }
    }

    public static class NavigationBarBuilderExtensions
    {
        public static UniTask<INavigationState<TModel>> BuildNavigationBar<TModel>(this IEnumerable<TModel> models, RectTransform parent,
            ElementRequest request)
            where TModel : INavigationPageModel
        {
            return NavigationBarBuilder<TModel>.Build(models, parent, request);
        }

        public static UniTask<INavigationState<TModel>> BuildNavigationBar<TModel>(this IEnumerable<TModel> models, RectTransform parent,
            Func<TModel, ElementRequest> request)
            where TModel : INavigationPageModel
        {
            return NavigationBarBuilder<TModel>.Build(models, parent, request);
        }
    }

    public static class NavigationBarBuilder
    {
        public static UniTask<INavigationState<TModel>> Build<TModel, TView>(this
                IEnumerable<TModel> models,
            RectTransform contentParent,
            ElementRequest switcherRequest)
            where TModel : INavigationPageModel
            where TView : NavigationSwitcherView<TModel>
        {
            return Build<TModel, TView>(models, contentParent, _ => switcherRequest);
        }

        public static async UniTask<INavigationState<TModel>> Build<TModel, TView>(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            Func<TModel, ElementRequest> switcherRequestFactory)
            where TModel : INavigationPageModel
            where TView : NavigationSwitcherView<TModel>
        {
            NavigationStateAdapter<TModel> state = new(new NavigationState<TModel>(models));

            CollectionPresenter<TModel, TView> collectionPresenter = new(
                (model, view) => new NavigationSwitcherPresenter<TModel, TView>(model, view, state, contentParent),
                (model, token) => ElementsGlobal.Elements.Create<TView, TModel>(model, switcherRequestFactory(model), token)
            );
            await UniTask.WhenAll(state.Models.Select(a => collectionPresenter.Add(a)));

            return state;
        }
    }
}