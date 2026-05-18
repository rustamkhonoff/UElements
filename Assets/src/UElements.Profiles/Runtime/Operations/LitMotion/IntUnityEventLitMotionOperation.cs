using System;
using LitMotion;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public class IntUnityEventLitMotionOperation : UnityEventLitMotionOperation<int>
    {
        protected override MotionHandle OnApplyMotionHandle(UnityEventTargetBase<int> target, int value, Configuration cfg)
        {
            return LMotion.Create(target.LastValue, value, cfg.Duration).WithDelay(cfg.Delay).WithEase(cfg.Ease).Bind(target.Invoke);
        }
    }
}