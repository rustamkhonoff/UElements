using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.CollectionView
{
    public static class CollectionPresenterBuilder
    {
        public static CollectionPresenter<TModel, TView> Build<TModel, TView>(
            TView prefab,
            RectTransform parent,
            Action<TModel, TView> onCreated = null,
            Action<TModel, TView> onDispose = null)
            where TView : ModelElement<TModel>
        {
            return Build(new ElementRequest(prefab.gameObject, parent), onCreated, onDispose);
        }

        public static CollectionPresenter<TModel, TView> Build<TModel, TView>(
            ElementRequest request,
            RectTransform parent,
            Action<TModel, TView> onCreated = null,
            Action<TModel, TView> onDispose = null)
            where TView : ModelElement<TModel>
        {
            return Build(request.WithParent(parent), onCreated, onDispose);
        }

        public static CollectionPresenter<TModel, TView> Build<TModel, TView>(
            ElementRequest request,
            Action<TModel, TView> onCreated = null,
            Action<TModel, TView> onDispose = null)
            where TView : ModelElement<TModel>
        {
            return new CollectionPresenter<TModel, TView>(
                (model, view) => new CollectionItemPresenter<TModel, TView>(model, view, onCreated, onDispose),
                (model, token) => ElementsGlobal.Elements.Create<TView, TModel>(model, request, token)
            );
        }

        public static CollectionPresenter<TModel, TView> Build<TModel, TView>(
            Func<TModel, CancellationToken, UniTask<TView>> viewFactory,
            Action<TModel, TView> onCreated = null,
            Action<TModel, TView> onDispose = null)
            where TView : ModelElement<TModel>
        {
            return new CollectionPresenter<TModel, TView>(
                (model, view) => new CollectionItemPresenter<TModel, TView>(model, view, onCreated, onDispose),
                viewFactory
            );
        }

        public static CollectionPresenter<TModel, TView> Build<TModel, TView>(
            Func<TModel, ElementRequest> request,
            Action<TModel, TView> onCreated = null,
            Action<TModel, TView> onDispose = null)
            where TView : ModelElement<TModel>
        {
            return new CollectionPresenter<TModel, TView>(
                (model, view) => new CollectionItemPresenter<TModel, TView>(model, view, onCreated, onDispose),
                (model, token) => ElementsGlobal.Elements.Create<TView, TModel>(model, request?.Invoke(model), token)
            );
        }
    }
}