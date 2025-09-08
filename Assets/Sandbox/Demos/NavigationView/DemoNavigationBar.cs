using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        [SerializeField] private DemoNavigationModel _a;

        private INavigation<DemoNavigationModel> m_navigation;

        private async void Start()
        {
            m_navigation = await NavigationBuilder.BuildNavigation<DemoNavigationModel, NavigationTab>(_ => _switcherRequest, _contentParent);
            foreach (DemoNavigationModel navigationPageModel in _navigationPageModels)
                await m_navigation.Add(navigationPageModel);
            m_navigation.TrySwitch(_navigationPageModels[0]);
        }

        private void OnDestroy()
        {
            m_navigation.Dispose();
        }

        [Button] public void DisposeNavigation() => m_navigation.Dispose();
        [Button] private void AddModel() => m_navigation.Add(_a);
        [Button] private void RemoveModel() => m_navigation.Remove(_a);
    }
}