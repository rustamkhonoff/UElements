using System;
using UElements;
using UElements.NavigationBar;
using UnityEngine;

namespace Demos.NavigationView
{
    [Serializable]
    public class DemoNavigationModel : INavigationPageModel
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        public ElementRequest ContentRequest => Prefab;
    }
}