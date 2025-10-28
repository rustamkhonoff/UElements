using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements
{
    public abstract class ElementBase : MonoBehaviour, IDisposable
    {
        public event Action Disposing;

        public IElementController ElementController { get; set; } = new DefaultElementController();

        private CancellationTokenSource m_cancellationTokenSource = new();
        private bool m_closed;
        private bool m_disposed;

        protected IElements Elements { get; private set; }

        internal UniTask Initialize(IElements elements)
        {
            Elements = elements;
            Initialize();
            return InitializeAsync();
        }

        #region Public API

        public async UniTask Show(Action callback = null, IElementController customController = null)
        {
            var controller = customController ?? ElementController;
            await controller.Show(this);
            callback?.Invoke();
        }

        public async UniTask Hide(Action callback = null, IElementController customController = null)
        {
            var controller = customController ?? ElementController;
            await controller.Hide(this);
            callback?.Invoke();
        }

        public async UniTask Close(Action callback = null, IElementController customController = null)
        {
            if (m_closed) return;
            m_closed = true;

            IElementController controller = customController ?? ElementController;

            try
            {
                await controller.Hide(this);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[ElementBase] Hide failed: {e.Message}");
            }

            callback?.Invoke();

            CleanupBase();
            Destroy(gameObject);
        }

        #endregion

        #region Lifecycle

        public virtual void Initialize() { }
        public virtual UniTask InitializeAsync() => UniTask.CompletedTask;

        protected virtual void OnDisposing() { }

        public void Dispose()
        {
            if (m_disposed) return;
            m_disposed = true;

            Close().Forget();
            OnDisposing();
        }

        private void CleanupBase()
        {
            if (m_cancellationTokenSource != null)
            {
                m_cancellationTokenSource.Cancel();
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }

            Elements = null;
            Disposing?.Invoke();
        }

        public CancellationToken LifetimeToken => m_cancellationTokenSource.Token;

        private void OnDestroy()
        {
            if (!m_disposed) Dispose();
        }

        #endregion
    }
}