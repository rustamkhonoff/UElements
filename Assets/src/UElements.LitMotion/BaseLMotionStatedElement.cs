using LitMotion;
using UElements.States;
using UnityEngine;

namespace UElements.LitMotion
{
    public abstract class BaseLMotionStatedElement : StatedElement
    {
        protected readonly CompositeMotionHandle Handle = new();

        protected void CancelAnimations()
        {
            Handle.Cancel();
        }
    }
    public abstract class LMotionStatedData
    {
        [field: SerializeField] public float Duration { get; private set; } = 0.25f;
        [field: SerializeField] public Ease Ease { get; private set; } = Ease.InOutSine;
    }
}