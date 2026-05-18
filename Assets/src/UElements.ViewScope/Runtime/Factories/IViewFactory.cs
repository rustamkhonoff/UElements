// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 14.01.2026
// Description:
// -------------------------------------------------------------------

using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.ViewScope
{
    public interface IViewFactory<T>
    {
        UniTask<IPresentationScope<T>> Create(CancellationToken rootToken = default);
    }
}