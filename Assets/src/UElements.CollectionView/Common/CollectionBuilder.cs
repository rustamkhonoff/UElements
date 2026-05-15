using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements.CollectionView
{
    public static class CollectionBuilder
    {
        public static async UniTask<ICollectionPresenter<TModel>> BuildCollectionPresenter<TModel>(
            this IEnumerable<TModel> models,
            Func<TModel, ICollectionItemPresenter<TModel>> presenter
        )
        {
            CollectionPresenter<TModel> presenterInstance = new(presenter);
            foreach (TModel model in models)
                await presenterInstance.Add(model);
            return presenterInstance;
        }

        public static ICollectionPresenter<TModel> BuildCollectionPresenter<TModel>(
            Func<TModel, ICollectionItemPresenter<TModel>> presenter
        )
        {
            CollectionPresenter<TModel> presenterInstance = new(presenter);
            return presenterInstance;
        }
    }
}