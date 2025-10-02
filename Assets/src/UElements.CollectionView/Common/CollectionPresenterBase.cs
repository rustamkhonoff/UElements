using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace UElements.CollectionView
{
    public class CollectionPresenter<TModel, TView> : ICollectionPresenter<TModel, TView>
    {
        private readonly ElementRequest m_itemRequest;
        private readonly Func<TModel, ICollectionModelPresenter<TModel, TView>> m_presenterFactory;
        private readonly Dictionary<TModel, ICollectionModelPresenter<TModel, TView>> m_presenters;

        private CancellationTokenSource m_lifeTimeTokenSource = new();

        public CollectionPresenter(Func<TModel, ICollectionModelPresenter<TModel, TView>> presenterFactory)
        {
            m_presenterFactory = presenterFactory;
            m_presenters = new Dictionary<TModel, ICollectionModelPresenter<TModel, TView>>();
        }

        public IEnumerable<TModel> Models => m_presenters.Keys;
        public IEnumerable<ICollectionModelPresenter<TModel, TView>> Presenters => m_presenters.Values;

        public virtual async UniTask Initialize(IEnumerable<TModel> data)
        {
            foreach (TModel model in data)
                await Add(model);
        }

        public virtual UniTask Add(TModel model)
        {
            if (model == null)
            {
                Debug.LogException(new NullReferenceException());
                return default;
            }

            if (m_presenters.ContainsKey(model))
            {
                Debug.LogException(new InvalidOperationException("Model already has presenter, aborting"));
                return default;
            }

            ICollectionModelPresenter<TModel, TView> presenterBase = m_presenterFactory.Invoke(model);


            m_presenters[model] = presenterBase;

            return presenterBase.Enable();
        }

        public virtual void Clear()
        {
            foreach ((TModel _, ICollectionModelPresenter<TModel, TView> value) in m_presenters)
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
            ICollectionModelPresenter<TModel, TView> collectionModelPresenter = m_presenters[model];
            collectionModelPresenter.Disable();
        }


        public virtual void Dispose()
        {
            Clear();

            m_lifeTimeTokenSource?.Cancel();
            m_lifeTimeTokenSource?.Dispose();
            m_lifeTimeTokenSource = null;
        }
    }
}