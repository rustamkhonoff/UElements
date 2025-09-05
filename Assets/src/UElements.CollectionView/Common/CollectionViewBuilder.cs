using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UElements.CollectionView
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class CollectionPresenterBuilder
    {
        public static ICollectionPresenter<TModel> BuildCollectionPresenter<TModel>(
            this Func<TModel, ElementRequest> request,
            Func<TModel, ModelElement<TModel>, IModelPresenter<TModel, ModelElement<TModel>>> presenter) =>
            BuildCollectionPresenter<TModel, ModelElement<TModel>>(request, presenter);

        public static ICollectionPresenter<TModel, TView> BuildCollectionPresenter<TModel, TView>(
            this Func<TModel, ElementRequest> request,
            Func<TModel, TView, IModelPresenter<TModel, TView>> presenter)
            where TView : ModelElement<TModel>
        {
            return new CollectionPresenter<TModel, TView>(
                presenter,
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, request(model), token)
            );
        }

        public static ICollectionPresenter<TModel> BuildCollectionPresenter<TModel>(
            this Func<TModel, ModelElement<TModel>, IModelPresenter<TModel, ModelElement<TModel>>> presenter,
            Func<TModel, ElementRequest> request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(presenter, request);
        }

        public static ICollectionPresenter<TModel, TView> BuildCollectionPresenter<TModel, TView>(
            this Func<TModel, TView, IModelPresenter<TModel, TView>> presenter,
            Func<TModel, ElementRequest> request)
            where TView : ModelElement<TModel>
        {
            return new CollectionPresenter<TModel, TView>(
                presenter,
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, request(model), token)
            );
        }

        public static ICollectionPresenter<TModel> BuildCollectionPresenter<TModel>(
            this IEnumerable<TModel> models,
            Func<TModel, ModelElement<TModel>, IModelPresenter<TModel, ModelElement<TModel>>> presenter,
            Func<TModel, ElementRequest> request)
        {
            return BuildCollectionPresenter<TModel, ModelElement<TModel>>(models, presenter, request);
        }

        public static ICollectionPresenter<TModel, TView> BuildCollectionPresenter<TModel, TView>(
            this IEnumerable<TModel> models,
            Func<TModel, TView, IModelPresenter<TModel, TView>> presenter,
            Func<TModel, ElementRequest> request)
            where TView : ModelElement<TModel>
        {
            CollectionPresenter<TModel, TView> presenterInstance = new(
                presenter,
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, request(model), token)
            );
            presenterInstance.Initialize(models);
            return presenterInstance;
        }
    }
}