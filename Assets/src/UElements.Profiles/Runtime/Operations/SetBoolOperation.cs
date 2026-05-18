// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 10.04.2026
// Description:
// -------------------------------------------------------------------
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.Profiles
{
    [Serializable]
    public sealed class SetBoolOperation : BaseProfileOperation<BoolUnityEventTargetBase, bool>
    {
        protected override void OnApply(BoolUnityEventTargetBase target, bool value)
        {
            target.Invoke(value);
        }

        protected override UniTask OnApplyAsync(BoolUnityEventTargetBase target, bool value, CancellationToken ct = default)
        {
            target.Invoke(value);
            return UniTask.CompletedTask;
        }
    }
}