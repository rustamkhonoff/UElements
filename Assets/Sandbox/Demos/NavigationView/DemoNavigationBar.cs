using System.Collections.Generic;
using UElements.CollectionView;
using UElements.NavigationBar;
using UnityEngine;

namespace Demos.NavigationView
{
    public class DemoNavigationBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentParent;
        [SerializeField] private CollectionItemRequest _switcherRequest;
        [SerializeField] private List<DemoNavigationModel> _navigationPageModels;

        private INavigationState<DemoNavigationModel> m_navigationState;

        private async void Start()
        {
            m_navigationState = await _navigationPageModels.BuildNavigationBar(_contentParent, _switcherRequest);

            m_navigationState.TrySwitch(_navigationPageModels[0]);
        }
    }
}