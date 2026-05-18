using System;
using LitMotion;
using UnityEngine;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public class RectTransformScaleChangeOperation : BaseLitMotionProfileOperation<RectTransformTarget, float>
    {
        protected override void OnApply(RectTransformTarget target, float value)
        {
            target.Value.localScale = Vector3.one * value;
        }

        protected override MotionHandle OnApplyMotionHandle(RectTransformTarget target, float value, Configuration cfg)
        {
            return LMotion.Create(target.Value.localScale, target.Value.localScale = Vector3.one * value, cfg.Duration).WithEase(cfg.Ease).WithDelay(cfg.Delay).Bind(s => target.Value.localScale = s);
        }
    }
}