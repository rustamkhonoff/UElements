using R3;
using System;
using UElements;
using UnityEngine;
using UElements.NavigationBar;

namespace Demos.NavigationView
{
    [Serializable]
    public class DemoNavigationModel : INavigationModel
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public ElementRequest ElementRequest { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<bool> Locked { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<bool> BadgeActive { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<string> Name { get; private set; }
    }
}