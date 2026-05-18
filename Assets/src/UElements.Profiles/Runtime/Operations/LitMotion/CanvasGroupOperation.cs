// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 12.05.2026
// Description:
// -------------------------------------------------------------------
using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public sealed class CanvasGroupOperation : BaseLitMotionProfileOperation<CanvasGroupTarget, CanvasGroupOperation.Data>
    {
        [Serializable]
        public class Data
        {
            [field: SerializeField] public float Alpha { get; private set; }
            [field: SerializeField] public bool Interactable { get; private set; }
            [field: SerializeField] public bool BlocksRaycasts { get; private set; }
        }

        protected override void OnApply(CanvasGroupTarget target, Data value)
        {
            target.Value.interactable = value.Interactable;
            target.Value.blocksRaycasts = value.BlocksRaycasts;
            target.Value.alpha = value.Alpha;
        }

        protected override MotionHandle OnApplyMotionHandle(CanvasGroupTarget target, Data value, Configuration cfg)
        {
            target.Value.interactable = value.Interactable;
            target.Value.blocksRaycasts = value.BlocksRaycasts;
            return LMotion.Create(target.Value.alpha, value.Alpha, cfg.Duration).WithDelay(cfg.Delay).WithEase(cfg.Ease).BindToAlpha(target.Value);
        }
    }
}