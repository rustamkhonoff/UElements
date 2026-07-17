// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 27.01.2026
// Description:
// -------------------------------------------------------------------

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public abstract class BaseLitMotionProfileOperation<TTarget, TValue> : BaseProfileOperation<TTarget, TValue>
        where TTarget : ProfileTarget
    {
        [Serializable]
        public class Configuration
        {
            [field: SerializeField] public bool Animate { get; private set; } = true;
            [field: SerializeField] public float Duration { get; private set; } = 0.125f;
            [field: SerializeField] public Ease Ease { get; private set; } = Ease.InOutSine;
            [field: SerializeField] public float Delay { get; set; }
        }

        [SerializeField] protected Configuration _configuration;
        private MotionHandle m_motionHandle;

        protected override async sealed UniTask OnApplyAsync(TTarget target, TValue value, CancellationToken ct = default)
        {
            if (m_motionHandle.IsActive()) 
                m_motionHandle.Cancel();

            if (_configuration.Animate)
            {
                m_motionHandle = OnApplyMotionHandle(target, value, _configuration);
                await m_motionHandle.ToUniTask(ct);
            }
            else
            {
                // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                OnApply(target, value);
            }
        }

        public override void Cancel()
        {
            if (m_motionHandle.IsActive())
                m_motionHandle.Cancel();
        }

        protected abstract MotionHandle OnApplyMotionHandle(TTarget target, TValue value, Configuration cfg);
    }
}