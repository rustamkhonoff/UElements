// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 27.01.2026
// Description:
// -------------------------------------------------------------------

using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.Profiles
{
    public interface IProfileOperation
    {
        UniTask ApplyAsync(ITargetProvider targets, CancellationToken ct = default);
        void Apply(ITargetProvider tr);
        string TargetId { get; }
        void Cancel();
    }
}