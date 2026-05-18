using System;
using LitMotion;
using UnityEngine;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public class RectTransformPositionChangeOperation : BaseLitMotionProfileOperation<RectTransformTarget, Vector2>
    {
        protected override void OnApply(RectTransformTarget target, Vector2 value)
        {
            target.Value.anchoredPosition = value;
        }

        protected override MotionHandle OnApplyMotionHandle(RectTransformTarget target, Vector2 value, Configuration cfg)
        {
            return LMotion.Create(target.Value.anchoredPosition, value, cfg.Duration).WithDelay(cfg.Delay).WithEase(cfg.Ease).Bind(v => target.Value.anchoredPosition = v);
        }
    }
}