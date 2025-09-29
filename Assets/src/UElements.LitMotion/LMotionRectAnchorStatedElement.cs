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

            public void Set(Vector4 vector4)
            {
                Min = new Vector2(vector4.x, vector4.y);
                Max = new Vector2(vector4.z, vector4.w);
            }
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

        public void SetActiveData(Vector4 data) => _datas.Active.Set(data);
        public void SetDefaultData(Vector4 data) => _datas.Default.Set(data);
        public void SetDisabledData(Vector4 data) => _datas.Disabled.Set(data);
    }
}