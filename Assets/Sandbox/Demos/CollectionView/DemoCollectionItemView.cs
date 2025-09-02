using System;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UElements;
using UElements.R3;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.CollectionView
{
    public class DemoCollectionItemView : ModelElement<DemoCollectionModel>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _click;

        private Action<DemoCollectionModel> m_action;

        public override void Initialize()
        {
            _click.SubscribeClick(() => m_action?.Invoke(Model)).AddTo(this);

            Model.AnyValueChanged
                .Subscribe(_ => _text.SetText(Model.Nickname.Value + ":" + Model.Health.Value))
                .AddTo(LifetimeToken);
        }

        public void AddCallback(Action<DemoCollectionModel> action)
        {
            m_action += action;
        }

        public void RemoveCallback(Action<DemoCollectionModel> action)
        {
            m_action -= action;
        }
    }
}