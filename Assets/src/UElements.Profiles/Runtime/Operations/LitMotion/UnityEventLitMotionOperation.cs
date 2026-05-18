// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
using System;

namespace UElements.Profiles.LitMotion
{
    [Serializable]
    public abstract class UnityEventLitMotionOperation<TType> : BaseLitMotionProfileOperation<UnityEventTargetBase<TType>, TType>
    {
        protected override void OnApply(UnityEventTargetBase<TType> target, TType value)
        {
            target.Invoke(value);
        }
    }
}