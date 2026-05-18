// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 20.01.2026
// Description:
// -------------------------------------------------------------------

using System.Threading;

namespace UElements.ViewScope
{
    public sealed class PresentationScope<T> : IPresentationScope<T>
    {
        public T View { get; private set; }

        public CancellationToken Token => m_cts.Token;

        private readonly CancellationTokenSource m_cts;

        public PresentationScope(CancellationToken rootToken)
        {
            m_cts = CancellationTokenSource.CreateLinkedTokenSource(rootToken);
        }

        public void AttachView(T view)
        {
            View = view;
        }

        public void Dispose()
        {
            if (!m_cts.IsCancellationRequested)
                m_cts.Cancel();

            m_cts.Dispose();
        }
    }
}