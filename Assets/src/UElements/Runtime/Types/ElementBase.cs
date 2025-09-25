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
        protected IElements Elements { get; private set; }

        internal UniTask Initialize(IElements elements)
        {
            Elements = elements;
            Initialize();
            return InitializeAsync();
        }

        public async UniTask Show(Action callback)
        {
            await ElementController.Show(this);
            callback?.Invoke();
        }

        public async UniTask Hide(Action callback)
        {
            await ElementController.Hide(this);
            callback?.Invoke();
        }

        public async UniTask Close(Action callback)
        {
            await ElementController.Hide(this);
            callback?.Invoke();
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

            Close(null).Forget();

            OnDisposing();

            CleanupBase();
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
            if (!m_disposed) Dispose();
        }
    }
}