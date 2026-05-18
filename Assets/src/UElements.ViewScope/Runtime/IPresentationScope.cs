using System;
using System.Threading;

namespace UElements.ViewScope
{
    public interface IPresentationScope<out TView> : IPresentationScope
    {
        TView View { get; }
    }

    public interface IPresentationScope : IDisposable
    {
        CancellationToken Token { get; }
    }
}