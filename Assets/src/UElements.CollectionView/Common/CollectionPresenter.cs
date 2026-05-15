using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace UElements.CollectionView
{
    public class CollectionPresenter<TModel> : ICollectionPresenter<TModel>
    {
        private readonly ElementRequest m_itemRequest;
        private readonly Func<TModel, ICollectionItemPresenter<TModel>> m_presenterFactory;
        private readonly Dictionary<TModel, ICollectionItemPresenter<TModel>> m_presenters;

        private readonly CancellationTokenSource m_lifeTimeTokenSource = new();

        public CollectionPresenter(Func<TModel, ICollectionItemPresenter<TModel>> presenterFactory)
        {
            m_presenterFactory = presenterFactory;
            m_presenters = new Dictionary<TModel, ICollectionItemPresenter<TModel>>();
        }

        public IEnumerable<TModel> Models => m_presenters.Keys;

        public virtual UniTask Add(TModel model)
        {
            if (model == null)
                return UniTask.FromException(new NullReferenceException("Model"));

            if (m_presenters.ContainsKey(model))
                return UniTask.FromException(new InvalidOperationException("Model already has presenter"));

            ICollectionItemPresenter<TModel> presenterBase = m_presenterFactory.Invoke(model);

            m_presenters[model] = presenterBase;

            return presenterBase.Enable();
        }

        public virtual void Clear()
        {
            foreach ((TModel _, ICollectionItemPresenter<TModel> value) in m_presenters)
                Remove_Internal(value.Model);
            m_presenters.Clear();
        }

        public virtual void Remove(TModel model)
        {
            if (model == null)
            {
                Debug.LogException(new NullReferenceException());
                return;
            }

            if (!m_presenters.ContainsKey(model))
            {
                return;
            }

            Remove_Internal(model);
            m_presenters.Remove(model);
        }

        private void Remove_Internal(TModel model)
        {
            ICollectionItemPresenter<TModel> collectionItemPresenter = m_presenters[model];
            collectionItemPresenter.Disable();
        }

        public virtual void Dispose()
        {
            Clear();

            m_lifeTimeTokenSource?.Cancel();
            m_lifeTimeTokenSource?.Dispose();
        }
    }
}