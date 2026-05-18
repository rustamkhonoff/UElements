using System;
using LitMotion;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public class FloatUnityEventLitMotionOperation : UnityEventLitMotionOperation<float>
    {
        protected override MotionHandle OnApplyMotionHandle(UnityEventTargetBase<float> target, float value, Configuration cfg)
        {
            return LMotion.Create(target.LastValue, value, cfg.Duration).WithDelay(cfg.Delay).WithEase(cfg.Ease).Bind(target.Invoke);
        }
    }
}