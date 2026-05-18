using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace UElements.Profiles
{
    [Serializable]
    public abstract class BaseProfileOperation<TTarget, TValue> : IProfileOperation where TTarget : ProfileTarget
    {
        [SerializeField, ProfileTargetId] private string _targetId;
        [SerializeField] private TValue _value;

        [Preserve] public Type TargetType => typeof(TTarget);

        public UniTask ApplyAsync(ITargetProvider rt, CancellationToken ct)
        {
            if (!rt.TryGet(_targetId, out TTarget t) || t == null)
                return UniTask.CompletedTask;

            return OnApplyAsync(t, _value, ct);
        }

        public void Apply(ITargetProvider tr)
        {
            if (!tr.TryGet(_targetId, out TTarget t) || t == null)
                return;

            OnApply(t, _value);
        }

        protected abstract void OnApply(TTarget target, TValue value);
        protected abstract UniTask OnApplyAsync(TTarget target, TValue value, CancellationToken ct = default);

        public string TargetId => _targetId;
        public virtual void Cancel() { }
    }
}