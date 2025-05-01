using Cysharp.Threading.Tasks;
using UElements.NavigationBar;
using UElements.R3;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.NavigationView
{
    public class DemoNavigationSwitcherView : NavigationSwitcherView<DemoNavigationModel>
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private Image _bg;
        [SerializeField] private Color _a, _b;

        public override void Initialize()
        {
            _image.sprite = Model.Icon;
            _button.SubscribeClick(Switch).AddTo(LifetimeToken);
        }

        protected override void OnSetSelected(bool state)
        {
            _bg.color = state ? _a : _b;
        }
    }
}