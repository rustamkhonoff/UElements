using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UElements;
using UElements.NavigationBar;
using UElements.R3;
using UElements.States;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.NavigationView
{
    public class NavigationTab : NavigationTabViewBase<DemoNavigationModel>
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private Image _bg;
        [SerializeField] private Color _a, _b;
        [SerializeField] private StatedElement _locked;
        [SerializeField] private StatedElement _selected;
        [SerializeField] private StatedElement _badgeActive;
        [SerializeField] private TMP_Text _text;

        public override void Initialize()
        {
            _image.sprite = Model.Icon;
            _button.SubscribeClick(TrySwitch).AddToElement(this);
            Model.Locked.Subscribe(_locked.SetState).AddToElement(this);
            Model.BadgeActive.CombineLatest(Model.Locked, (badge, locked) => badge && !locked).Subscribe(_badgeActive.SetState).AddToElement(this);
            Model.Name.SubscribeToText(_text).AddToElement(this);
        }

        private void TrySwitch()
        {
            if (Model.Locked.Value) return;
            Switch();
        }

        protected override void OnSetState(bool state)
        {
            _selected.SetState(state);
            LMotion.Create(_bg.color, _a.Or(_b, state), 0.25f).BindToColor(_bg).AddTo(this);
        }

        public override void SetInitialState(bool state)
        {
            _selected.SetState(state);
            _bg.color = state ? _a : _b;
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