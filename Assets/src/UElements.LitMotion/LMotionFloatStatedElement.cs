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
    }
}