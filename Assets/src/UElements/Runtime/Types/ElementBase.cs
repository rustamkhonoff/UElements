using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements
{
    public abstract class ElementBase : MonoBehaviour
    {
        private bool m_closing;
        private bool m_cleanedUp;

        private readonly CancellationTokenSource m_lifetimeCts = new();
        public CancellationToken LifetimeToken => m_lifetimeCts?.Token ?? CancellationToken.None;
        protected IElements Elements { get; private set; }
        public IElementController ElementController { get; set; } = new DefaultElementController();

        internal void InitializeElements(IElements elements)
        {
            Elements = elements;
        }

        public virtual void Initialize() { }

        public virtual UniTask InitializeAsync()
        {
            return UniTask.CompletedTask;
        }

        public async UniTask Show()
        {
            if (m_closing || m_cleanedUp)
                return;

            await ElementController.Show(this);
        }

        public async UniTask Hide()
        {
            if (m_closing || m_cleanedUp)
                return;

            await ElementController.Hide(this);
        }

        public async UniTask Close()
        {
            if (m_closing || m_cleanedUp)
                return;

            m_closing = true;

            try
            {
                if (this != null && gameObject != null)
                    await ElementController.Hide(this);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[ElementBase] Hide failed: {e.Message}", this);
            }

            CleanupBase();

            if (this != null && gameObject != null)
                Destroy(gameObject);
        }

        public IDisposable AsCloseDisposable()
        {
            return new CloseDisposable(this);
        }

        protected virtual void DeInitialize() { }

        private void CleanupBase()
        {
            if (m_cleanedUp)
                return;

            m_cleanedUp = true;
            m_closing = true;

            if (m_lifetimeCts != null)
            {
                m_lifetimeCts.Cancel();
            }

            try
            {
                DeInitialize();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }

            Elements = null;

            if (m_lifetimeCts != null)
            {
                m_lifetimeCts.Dispose();
            }
        }

        private void OnDestroy()
        {
            CleanupBase();
        }

        private sealed class CloseDisposable : IDisposable
        {
            private ElementBase m_element;
            private bool m_disposed;

            public CloseDisposable(ElementBase element)
            {
                m_element = element;
            }

            public void Dispose()
            {
                if (m_disposed)
                    return;

                m_disposed = true;

                ElementBase element = m_element;
                m_element = null;

                if (element == null)
                    return;

                UniTask.Void(async () =>
                {
                    await UniTask.SwitchToMainThread();

                    if (element != null)
                        await element.Close();
                });
            }
        }

        private sealed class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Instance = new();

            public void Dispose() { }
        }
    }
}