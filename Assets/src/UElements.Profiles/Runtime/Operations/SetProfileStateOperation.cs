// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 24.07.2026
// Description:
// -------------------------------------------------------------------
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.Profiles
{
    [Serializable]
    public sealed class SetProfileStateOperation : BaseProfileOperation<ProfileControllerTarget, SetProfileStateOperation.Data>
    {
        [Serializable]
        public class Data
        {
            [field: SerializeField] public string State { get; private set; }
            [field: SerializeField] public bool UsePredefinedState { get; private set; }
            [field: SerializeField] public string Value { get; private set; }
            [field: SerializeField] public bool UsePredefinedValue { get; private set; }
            [field: SerializeField] public bool Immediate { get; private set; }
        }

        protected override void OnApply(ProfileControllerTarget target, Data value)
        {
            target.Value.SetValueImmediate(value.State, value.State);
        }

        protected override async UniTask OnApplyAsync(ProfileControllerTarget target, Data value, CancellationToken ct = default)
        {
            if (value.Immediate)
                target.Value.SetValueImmediate(value.State, value.State);
            else
                await target.Value.SetValue(value.State, value.State, ct);
        }
    }
}