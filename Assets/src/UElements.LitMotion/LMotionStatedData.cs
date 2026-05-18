using LitMotion;
using UnityEngine;

namespace UElements.LitMotion
{
    public abstract class LMotionStatedData
    {
        [field: SerializeField] public float Duration { get; private set; } = 0.25f;
        [field: SerializeField] public Ease Ease { get; private set; } = Ease.InOutSine;
    }
}