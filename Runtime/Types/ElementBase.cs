using System;
using System.Threading;
using UnityEngine;

namespace UElements
{
    public abstract class ElementBase : MonoBehaviour, IDisposable
    {
        public IElementController ElementController { get; set; } = new DefaultElementController();
        private CancellationTokenSource m_cancellationTokenSource;

        protected IElements Elements { get; private set; }

        internal void Initialize(IElements elements)
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            Elements = elements;
        }

        public void Show() => Show(null);
        public void Show(Action callback) => ElementController.Show(callback);
        public void Hide() => Hide(null);

        public void Hide(Action callback)
        {
            callback += () => Destroy(gameObject);
            ElementController.Hide(callback);
        }

        public virtual void Initialize() { }
        public virtual void Dispose() { }

        public CancellationToken LifetimeToken => m_cancellationTokenSource.Token;

        private void OnDestroy()
        {
            Dispose();
            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource = null;
            Elements = null;
        }
    }
}