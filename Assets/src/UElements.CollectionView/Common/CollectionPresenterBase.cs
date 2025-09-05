using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UElements.CollectionView
{
    public class CollectionPresenter<TModel, TView> : ICollectionPresenter<TModel, TView> where TView : ModelElement<TModel>
    {
        private readonly ElementRequest m_itemRequest;
        private readonly Func<TModel, TView, IModelPresenter<TModel, TView>> m_presenterFactory;
        private readonly Func<TModel, CancellationToken, UniTask<TView>> m_viewFactory;
        private readonly Dictionary<TModel, IModelPresenter<TModel, TView>> m_presenters;

        private CancellationTokenSource m_lifeTimeTokenSource = new();

        public CollectionPresenter(
            Func<TModel, TView, IModelPresenter<TModel, TView>> presenterFactory,
            Func<TModel, CancellationToken, UniTask<TView>> viewFactory)
        {
            m_presenterFactory = presenterFactory;
            m_viewFactory = viewFactory;
            m_presenters = new Dictionary<TModel, IModelPresenter<TModel, TView>>();
        }

        public IEnumerable<TModel> Models => m_presenters.Keys;
        public IEnumerable<IModelPresenter<TModel, ModelElement<TModel>>> Presenters => m_presenters.Values;
        public IEnumerable<TView> Views => Presenters.Select(a => a.View).Cast<TView>();

        public virtual async void Initialize(IEnumerable<TModel> data)
        {
            foreach (TModel model in data)
                await Add(model);
        }

        public virtual async UniTask<ModelElement<TModel>> Add(TModel model)
        {
            if (model == null)
            {
                Debug.LogException(new NullReferenceException());
                return null;
            }

            if (m_presenters.ContainsKey(model))
            {
                Debug.LogException(new InvalidOperationException("Model already has presenter, aborting"));
                return null;
            }

            TView view = await m_viewFactory.Invoke(model, m_lifeTimeTokenSource.Token);
            IModelPresenter<TModel, TView> presenterBase = m_presenterFactory.Invoke(model, view);

            presenterBase.Initialize();

            m_presenters[model] = presenterBase;
            return view;
        }

        public virtual void Clear()
        {
            foreach ((TModel _, IModelPresenter<TModel, TView> value) in m_presenters)
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
            IModelPresenter<TModel, TView> modelPresenter = m_presenters[model];

            modelPresenter.Dispose();
        }


        public virtual void Dispose()
        {
            Clear();

            m_lifeTimeTokenSource.Cancel();
            m_lifeTimeTokenSource.Dispose();
            m_lifeTimeTokenSource = null;
        }
    }
}