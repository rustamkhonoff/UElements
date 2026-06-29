using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UElements;
using UElements.NavigationBar;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.NavigationView
{
    public class ElementNavigationTabBase : ElementNavigationTabBase<DemoNavigationModel>
    {
        public override event Action SwitchRequested;

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private Image _bg;
        [SerializeField] private Color _a, _b;
        [SerializeField] private TMP_Text _text;

        protected override void Initialize(CancellationToken ct)
        {
            _image.sprite = Model.Icon;
            Model.Name.SubscribeToText(_text).AddTo(ct);
            _button.onClick.AddListener(RequestSwitch);
        }

        private void RequestSwitch()
        {
            SwitchRequested?.Invoke();
        }

        public override void SetState(bool state)
        {
            // _selected.SetState(state);
            LMotion.Create(_bg.color, _a.Or(_b, state), 0.25f).BindToColor(_bg).AddTo(this);
        }

        public override void SetInitialState(bool state)
        {
            // _selected.SetState(state);
            _bg.color = state ? _a : _b;
        }

        protected override void DeInitialize()
        {
            _button.onClick.RemoveListener(RequestSwitch);
        }
    }

    public static class DataExtensions
    {
        public static T Or<T>(this T left, T right, bool state)
        {
            if (state) return left;
            return right;
        }
    }
}