using System.Collections.Generic;
using UElements.CollectionView;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using R3;

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
            return NavigationBarBuilder.Build<TModel, NavigationSwitcherViewBase<TModel>>(models, contentParent, _ => switcherRequest);
        }

        public static UniTask<INavigationState<TModel>> Build(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            Func<TModel, ElementRequest> switcherRequest)
        {
            return NavigationBarBuilder.Build<TModel, NavigationSwitcherViewBase<TModel>>(models, contentParent, switcherRequest);
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
            where TView : NavigationSwitcherViewBase<TModel>
        {
            return Build<TModel, TView>(models, contentParent, _ => switcherRequest);
        }

        public static async UniTask<INavigationState<TModel>> Build<TModel, TView>(
            IEnumerable<TModel> models,
            RectTransform contentParent,
            Func<TModel, ElementRequest> switcherRequestFactory)
            where TModel : INavigationPageModel
            where TView : NavigationSwitcherViewBase<TModel>
        {
            INavigationState<TModel> state = new NavigationState<TModel>(models);

            ICollectionPresenter<TModel, TView> switchersPresenter = new CollectionPresenter<TModel, TView>(
                (model, view) => new NavigationSwitcherPresenter<TModel, TView>(model, view, state, contentParent),
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, switcherRequestFactory(model), token)
            );

            await UniTask.WhenAll(state.Models.Select(a => switchersPresenter.Add(a)));

            return new NavigationContext<TModel, TView>(state, switchersPresenter);
        }
    }

    public sealed class NavigationContext<TModel, TView> : INavigationState<TModel>
        where TModel : INavigationPageModel
        where TView : NavigationSwitcherViewBase<TModel>
    {
        private readonly INavigationState<TModel> m_state;
        private readonly ICollectionPresenter<TModel, TView> m_presenter;

        public NavigationContext(INavigationState<TModel> state, ICollectionPresenter<TModel, TView> presenter)
        {
            m_state = state;
            m_presenter = presenter;
        }

        public ReadOnlyReactiveProperty<TModel> ActivePage => m_state.ActivePage;
        public IReadOnlyCollection<TModel> Models => m_state.Models;
        public bool TrySwitch(TModel model) => m_state.TrySwitch(model);

        public void Dispose()
        {
            m_presenter.Dispose();
            m_state.Dispose();
        }
    }
}