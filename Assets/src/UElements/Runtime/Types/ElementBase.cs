using System.Threading;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public abstract class ElementBase : MonoBehaviour, IDisposable
    {
        public event Action Disposing;
        public IElementController ElementController { get; set; } = new DefaultElementController();
        private CancellationTokenSource m_cancellationTokenSource = new();
        private bool m_disposed;
        private bool m_closed;
        protected IElements Elements { get; private set; }

        internal UniTask Initialize(IElements elements)
        {
            Elements = elements;
            Initialize();
            return InitializeAsync();
        }

        public UniTask Show(Action callback)
        {
            return Show(callback, ElementController);
        }

        public async UniTask Show(Action callback, IElementController customController)
        {
            await customController.Show(this);
            callback?.Invoke();
        }

        public UniTask Hide(Action callback)
        {
            return Hide(callback, ElementController);
        }

        public async UniTask Hide(Action callback, IElementController customController)
        {
            await customController.Hide(this);
            callback?.Invoke();
        }

        public UniTask Close(Action callback)
        {
            return Close(callback, ElementController);
        }

        public async UniTask Close(Action callback, IElementController customController)
        {
            if (m_closed) return;
            m_closed = true;

            await customController.Hide(this);
            callback?.Invoke();

            CleanupBase();
            Destroy(gameObject);
        }

        public virtual void Initialize() { }

        public virtual UniTask InitializeAsync()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnDisposing() { }

        public void Dispose()
        {
            if (m_disposed) return;
            m_disposed = true;

            OnDisposing();

            if (!m_closed)
            {
                CleanupBase();
                Destroy(gameObject);
            }
        }

        private void CleanupBase()
        {
            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource?.Dispose();
            m_cancellationTokenSource = null;
            Elements = null;

            Disposing?.Invoke();
        }

        public CancellationToken LifetimeToken => m_cancellationTokenSource.Token;

        private void OnDestroy()
        {
            if (!m_disposed)
                Dispose();
        }
    }
}