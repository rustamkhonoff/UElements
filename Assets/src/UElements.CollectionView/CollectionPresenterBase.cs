using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace UElements.CollectionView
{
    public abstract class CollectionPresenterBase<TModel, TView, TPresenter> : IDisposable
        where TView : ModelElement<TModel>
        where TPresenter : CollectionModelPresenterBase<TModel, TView>
    {
        public event Action<TModel, TView> OnCreated, OnDisposed;

        private readonly ElementRequest m_itemRequest;
        private readonly Func<TModel, TView, TPresenter> m_presenterFactory;
        private readonly Func<TModel, CancellationToken, UniTask<TView>> m_viewFactory;
        private readonly Dictionary<TModel, TPresenter> m_presenters;

        private CancellationTokenSource m_lifeTimeTokenSource = new();

        public CollectionPresenterBase(
            Func<TModel, TView, TPresenter> presenterFactory,
            Func<TModel, CancellationToken, UniTask<TView>> viewFactory)
        {
            m_presenterFactory = presenterFactory;
            m_viewFactory = viewFactory;
            m_presenters = new Dictionary<TModel, TPresenter>();
        }

        public bool TryGetCollectionItemPresenter(TModel model, out TPresenter presenter)
        {
            presenter = null;

            if (!m_presenters.ContainsKey(model)) return false;
            presenter = m_presenters[model];

            return true;
        }

        public async void Initialize(IEnumerable<TModel> data)
        {
            foreach (TModel model in data)
                await AddAsync(model);
        }

        public void Add(TModel model)
        {
            AddAsync(model).Forget();
        }

        public async UniTask<TView> AddAsync(TModel model)
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
            TPresenter presenterBase = m_presenterFactory.Invoke(model, view);

            OnCreated?.Invoke(model, view);
            presenterBase.Initialize();

            m_presenters[model] = presenterBase;

            return view;
        }

        public void Remove(TModel model)
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
            CollectionModelPresenterBase<TModel, TView> presenter = m_presenters[model];

            OnDisposed?.Invoke(model, presenter.View);
            presenter.Dispose();
        }

        public void Clear()
        {
            foreach ((TModel _, CollectionModelPresenterBase<TModel, TView> value) in m_presenters)
                Remove_Internal(value.Model);
            m_presenters.Clear();
        }

        public void Dispose()
        {
            Clear();

            m_lifeTimeTokenSource.Cancel();
            m_lifeTimeTokenSource.Dispose();
            m_lifeTimeTokenSource = null;
        }
    }
}