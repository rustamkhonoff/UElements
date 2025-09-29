using System;
using LitMotion;
using UElements.States;
using UnityEngine;
using UnityEngine.UI;

namespace UElements.LitMotion
{
    public class LMotionGraphicColorStatedElement : BaseLMotionStatedElement
    {
        [Serializable]
        public class Data : LMotionStatedData
        {
            [field: SerializeField] public Color Color { get; private set; } = Color.clear;

            public void Set(Color color) => Color = color;
        }

        [SerializeField] private StatedData<Data> _datas;
        [SerializeField] private Graphic _graphic;

        protected override void OnSetState(State state, bool animate)
        {
            CancelAnimations();

            Data data = _datas.Get(state);
            LMotion.Create(_graphic.color, data.Color, data.Duration).WithEase(data.Ease).Bind(a => _graphic.color = a).AddTo(Handle);
        }

        public void SetActiveData(Color color) => _datas.Active.Set(color);
        public void SetDefaultData(Color color) => _datas.Default.Set(color);
        public void SetDisabledData(Color color) => _datas.Disabled.Set(color);
    }
}