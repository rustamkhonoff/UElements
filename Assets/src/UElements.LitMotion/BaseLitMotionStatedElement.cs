// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
using LitMotion;
using UElements.States;

namespace UElements.LitMotion
{
    public abstract class BaseLitMotionStatedElement : StatedElement
    {
        protected readonly CompositeMotionHandle Handle = new();

        protected void CancelAnimations()
        {
            Handle.Cancel();
        }

        protected override void DeInitialize()
        {
            CancelAnimations();
        }
    }
}