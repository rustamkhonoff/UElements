using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class CollectionPresenterBuilder
    {
        public static ICollectionPresenter<TModel> BuildCollectionPresenter<TModel>(
            this Func<TModel, ModelElement<TModel>, ICollectionModelPresenter<TModel, ModelElement<TModel>>> presenter,
            Func<TModel, ElementRequest> request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(presenter, request);
        }

        public static ICollectionPresenter<TModel> BuildCollectionPresenter<TModel>(
            this Func<TModel, ModelElement<TModel>, ICollectionModelPresenter<TModel, ModelElement<TModel>>> presenter,
            ElementRequest request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(presenter, _ => request);
        }

        public static ICollectionPresenter<TModel, TView> BuildCollectionPresenter<TModel, TView>(
            this Func<TModel, TView, ICollectionModelPresenter<TModel, TView>> presenter,
            Func<TModel, ElementRequest> request)
            where TView : ModelElement<TModel>
        {
            return new CollectionPresenter<TModel, TView>(
                presenter,
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, request(model), token)
            );
        }

        public static UniTask<ICollectionPresenter<TModel, ModelElement<TModel>>> BuildCollectionPresenter<TModel>(
            this IEnumerable<TModel> models,
            Func<TModel, ModelElement<TModel>, ICollectionModelPresenter<TModel, ModelElement<TModel>>> presenter,
            Func<TModel, ElementRequest> request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(models, presenter, request);
        }

        public static UniTask<ICollectionPresenter<TModel, ModelElement<TModel>>> BuildCollectionPresenter<TModel>(
            this IEnumerable<TModel> models,
            Func<TModel, ModelElement<TModel>, ICollectionModelPresenter<TModel, ModelElement<TModel>>> presenter,
            ElementRequest request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(models, presenter, _ => request);
        }

        public static UniTask<ICollectionPresenter<TModel, ModelElement<TModel>>> BuildCollectionPresenter<TModel>(
            this IEnumerable<TModel> models,
            Func<TModel, ElementRequest> request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(models,
                (model, view) => new CollectionModelPresenter<TModel, ModelElement<TModel>>(model, view), request);
        }

        public static UniTask<ICollectionPresenter<TModel, ModelElement<TModel>>> BuildCollectionPresenter<TModel>(
            this IEnumerable<TModel> models,
            ElementRequest request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(models,
                (model, view) => new CollectionModelPresenter<TModel, ModelElement<TModel>>(model, view), _ => request);
        }

        public static async UniTask<ICollectionPresenter<TModel, TView>> BuildCollectionPresenter<TModel, TView>(
            this IEnumerable<TModel> models,
            Func<TModel, TView, ICollectionModelPresenter<TModel, TView>> presenter,
            Func<TModel, ElementRequest> request)
            where TView : ModelElement<TModel>
        {
            CollectionPresenter<TModel, TView> presenterInstance = new(
                presenter,
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, request(model), token)
            );
            await presenterInstance.Initialize(models);
            return presenterInstance;
        }
    }
}