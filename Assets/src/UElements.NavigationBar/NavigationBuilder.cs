using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;

namespace UElements.NavigationBar
{
    public static class NavigationBuilder
    {
        public async static UniTask<INavigation<TModel>> BuildNavigation<TModel, TTab>(this IEnumerable<TModel> models,
            Func<TModel, UniTask<TTab>> navFactory,
            Func<TModel, INavigationContentPresenter> contentPresenterFactory, string defaultPage = null)
            where TModel : INavigationModel
            where TTab : INavigationTab
        {
            INavigation<TModel> navigation = BuildNavigation(navFactory, contentPresenterFactory);
            foreach (TModel navigationModel in models) await navigation.Add(navigationModel);
            if (defaultPage != null) navigation.TrySwitch(defaultPage);
            return navigation;
        }

        public async static UniTask<INavigation<TModel>> BuildNavigation<TModel, TTab>(this IEnumerable<TModel> models,
            Func<TModel, UniTask<TTab>> navFactory,
            Func<TModel, INavigationContentPresenter> contentPresenterFactory, TModel defaultPage = default)
            where TModel : INavigationModel
            where TTab : INavigationTab
        {
            INavigation<TModel> navigation = BuildNavigation(navFactory, contentPresenterFactory);
            foreach (TModel navigationModel in models) await navigation.Add(navigationModel);
            if (defaultPage != null) navigation.TrySwitch(defaultPage);
            return navigation;
        }

        public static INavigation<TModel> BuildNavigation<TModel, TTab>(Func<TModel, UniTask<TTab>> navFactory,
            Func<TModel, INavigationContentPresenter> contentPresenterFactory)
            where TModel : INavigationModel
            where TTab : INavigationTab
        {
            INavigationState<TModel> state = new NavigationState<TModel>();

            ICollectionPresenter<TModel, TTab> collectionPresenter = CollectionPresenterBuilder.BuildCollectionPresenter<TModel, TTab>(
                model => new NavigationTabPresenter<TModel, TTab>(state, model, navFactory)
            );

            INavigationPresenter navigationPresenter = new NavigationPresenter<TModel>(state, contentPresenterFactory);
            INavigation<TModel> navigation = new Navigation<TModel, TTab>(state, collectionPresenter, navigationPresenter);
            return navigation;
        }
    }
}