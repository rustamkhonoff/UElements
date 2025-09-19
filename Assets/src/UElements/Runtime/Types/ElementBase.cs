using System.Threading;
using UnityEngine;
using System;

namespace UElements
{
    public abstract class ElementBase : MonoBehaviour, IDisposable
    {
        public event Action Disposing;
        public IElementController ElementController { get; set; } = new DefaultElementController();
        private CancellationTokenSource m_cancellationTokenSource = new();
        private bool m_disposed;
        protected IElements Elements { get; private set; }
        internal void Initialize(IElements elements)
        {
            Elements = elements;
            Initialize();
        }

        public void Show(Action callback) => ElementController.Show(this, callback);
        public void Hide(Action callback) => ElementController.Hide(this, callback);
        public void Close(Action callback) => ElementController.Hide(this, callback + (() => Destroy(gameObject)));
        protected virtual void Initialize() { }
        protected virtual void OnDisposing() { }

        public void Dispose()
        {
            if (m_disposed) return;
            m_disposed = true;

            Close(null);

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