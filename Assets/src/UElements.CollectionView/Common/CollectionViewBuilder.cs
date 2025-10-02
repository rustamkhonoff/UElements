using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public static class CollectionPresenterBuilder
    {
        public static UniTask<ICollectionPresenter<TModel, TView>> BuildCollectionPresenter<TModel, TView>(this IEnumerable<TModel> models,
            Func<TModel, ElementRequest> itemElementRequestFactory) where TView : Element
        {
            return BuildCollectionPresenter(models, model => CreatePresenter<TModel, TView>(model, itemElementRequestFactory));
        }

        private static ICollectionModelPresenter<TModel, TView> CreatePresenter<TModel, TView>(TModel arg, Func<TModel, ElementRequest> reqF)
            where TView : Element
        {
            UniTask<TView> CreateView(TModel model) => ElementsGlobal.Instance.Create<TView>(reqF(arg));
            return new ElementCollectionItemPresenter<TModel, TView>(arg, CreateView);
        }

        public static async UniTask<ICollectionPresenter<TModel, TView>> BuildCollectionPresenter<TModel, TView>(
            this IEnumerable<TModel> models,
            Func<TModel, ICollectionModelPresenter<TModel, TView>> presenter)
        {
            CollectionPresenter<TModel, TView> presenterInstance = new(presenter);
            await presenterInstance.Initialize(models);
            return presenterInstance;
        }

        public static ICollectionPresenter<TModel, TView> BuildCollectionPresenter<TModel, TView>(
            Func<TModel, ICollectionModelPresenter<TModel, TView>> presenter)
        {
            CollectionPresenter<TModel, TView> presenterInstance = new(presenter);
            return presenterInstance;
        }
    }
}