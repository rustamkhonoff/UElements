using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.Profiles
{
    [Serializable]
    public abstract class UnityEventOperation<TType> : BaseProfileOperation<UnityEventTargetBase<TType>, TType>
    {
        protected override UniTask OnApplyAsync(UnityEventTargetBase<TType> target, TType value, CancellationToken ct = default)
        {
            target.Invoke(value);
            return UniTask.CompletedTask;
        }

        protected override void OnApply(UnityEventTargetBase<TType> target, TType value)
        {
            target.Invoke(value);
        }
    }
}