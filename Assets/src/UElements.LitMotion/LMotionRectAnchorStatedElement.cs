using System;
using LitMotion;
using UElements.States;
using UnityEngine;

namespace UElements.LitMotion
{
    [Serializable]
    public class LMotionRectAnchorStatedElement : BaseLMotionStatedElement
    {
        [Serializable]
        public class Data : LMotionStatedData
        {
            [field: SerializeField] public Vector2 Min { get; private set; } = Vector2.zero;
            [field: SerializeField] public Vector2 Max { get; private set; } = Vector2.one;
        }

        [SerializeField] private StatedData<Data> _datas;
        [SerializeField] private RectTransform _rectTransform;

        protected override void OnSetState(State state, bool animate)
        {
            CancelAnimations();

            Data data = _datas.Get(state);
            if (!animate)
            {
                _rectTransform.anchorMin = data.Min;
                _rectTransform.anchorMax = data.Max;
            }
            else
            {
                Vector2 minS = _rectTransform.anchorMin;
                Vector2 maxS = _rectTransform.anchorMax;

                void ToDo(float t)
                {
                    _rectTransform.anchorMin = Vector2.LerpUnclamped(minS, data.Min, t);
                    _rectTransform.anchorMax = Vector2.LerpUnclamped(maxS, data.Max, t);
                }

                LMotion.Create(0f, 1f, data.Duration).WithEase(data.Ease).Bind(ToDo).AddTo(Handle);
            }
        }
    }
}