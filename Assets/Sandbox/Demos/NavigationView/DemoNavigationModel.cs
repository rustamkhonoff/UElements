using System;
using R3;
using Sirenix.OdinInspector;
using UElements;
using UElements.NavigationBar;
using UnityEngine;

namespace Demos.NavigationView
{
    [Serializable]
    public class DemoNavigationModel : INavigationPageModel
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public ElementRequest ContentRequest { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<bool> Locked { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<bool> BadgeActive { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<string> Name { get; private set; }
    }
}