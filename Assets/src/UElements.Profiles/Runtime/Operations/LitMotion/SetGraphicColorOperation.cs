using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public sealed class SetGraphicColorOperation : BaseLitMotionProfileOperation<GraphicTarget, Color>
    {
        protected override MotionHandle OnApplyMotionHandle(GraphicTarget target, Color value, Configuration cfg)
        {
            return LMotion.Create(target.Value.color, value, cfg.Duration).WithDelay(cfg.Delay).WithEase(cfg.Ease).BindToColor(target.Value);
        }

        protected override void OnApply(GraphicTarget target, Color value)
        {
            target.Value.color = value;
        }
    }
}