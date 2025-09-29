using System;
using LitMotion;
using UElements.States;
using UnityEngine;
using UnityEngine.Events;

namespace UElements.LitMotion
{
    [Serializable]
    public class LMotionFloatStatedElement : BaseLMotionStatedElement
    {
        [Serializable]
        public class Data : LMotionStatedData
        {
            [field: SerializeField] public float Value { get; private set; }
            public void Set(float value) => Value = value;
        }

        [SerializeField] private UnityEvent<float> _event;
        [SerializeField] private StatedData<Data> _datas;
        [SerializeField] private float _value;

        protected override void OnSetState(State state, bool animate)
        {
            CancelAnimations();

            Data data = _datas.Get(state);
            if (animate)
            {
                LMotion
                    .Create(_value, data.Value, data.Duration)
                    .WithEase(data.Ease)
                    .Bind(a =>
                    {
                        _value = a;
                        _event?.Invoke(_value);
                    }).AddTo(Handle);
            }
            else
            {
                _event?.Invoke(data.Value);
            }
        }
        public void SetActiveData(float data) => _datas.Active.Set(data);
        public void SetDefaultData(float data) => _datas.Default.Set(data);
        public void SetDisabledData(float data) => _datas.Disabled.Set(data);
    }
}