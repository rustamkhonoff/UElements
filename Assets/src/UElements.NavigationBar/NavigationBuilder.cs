using System;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;

namespace UElements.NavigationBar
{
    public static class NavigationBuilder
    {
        public static UniTask<INavigation<TModel>> BuildNavigation<TModel, TView>(Func<TModel, UniTask<TView>> navFactory)
            where TModel : INavigationModel
            where TView : INavigationTab
        {
            INavigationState<TModel> state = new NavigationState<TModel>();

            ICollectionPresenter<TModel, TView> collectionPresenter = CollectionPresenterBuilder.BuildCollectionPresenter<TModel, TView>(
                model => new NavigationTabPresenter<TModel, TView>(state, model, navFactory)
            );

            INavigationPresenter navigationPresenter = new NavigationPresenter<TModel>(state);
            INavigation<TModel> navigation = new Navigation<TModel, TView>(state, collectionPresenter, navigationPresenter);
            return UniTask.FromResult(navigation);
        }
    }
}