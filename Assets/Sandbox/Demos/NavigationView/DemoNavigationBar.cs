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

        private void Start()
        {
            NavigationBarBuilder.Build<DemoNavigationModel, DemoNavigationSwitcherView>
                (_navigationPageModels, _contentParent, _switcherRequest, out m_navigationState, out _);

            m_navigationState.TrySwitch(_navigationPageModels[0]);
        }
    }
}