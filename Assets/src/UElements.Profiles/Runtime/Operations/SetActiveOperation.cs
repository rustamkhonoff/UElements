using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.Profiles
{
    [Serializable]
    public sealed class SetActiveOperation : BaseProfileOperation<GameObjectTarget, bool>
    {
        protected override void OnApply(GameObjectTarget target, bool value)
        {
            target.Target.SetActive(value);
        }

        protected override UniTask OnApplyAsync(GameObjectTarget target, bool value, CancellationToken ct = default)
        {
            target.Target.SetActive(value);
            return UniTask.CompletedTask;
        }
    }
}