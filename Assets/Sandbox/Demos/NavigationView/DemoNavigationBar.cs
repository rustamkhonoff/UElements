using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demos.NavigationView;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UElements;
using UElements.CollectionView;
using UElements.NavigationBar;
using UnityEngine;

namespace Sandbox.Demos.NavigationView
{
    public class DemoNavigationBar : ModelElement<DemoNavigationBar.ViewModel>
    {
        public record ViewModel(string PageKey);

        [SerializeField] private RectTransform _contentParent;
        [SerializeField] private CollectionItemRequest _switcherRequest;
        [SerializeField] private List<DemoNavigationModel> _navigationPageModels;
        [SerializeField] private DemoNavigationModel _a;

        private INavigation<DemoNavigationModel> m_navigation;


        
        public override async void Initialize()
        {
            m_navigation = await _navigationPageModels.BuildNavigation(_switcherRequest, _contentParent);

            if (Model is not null && !string.IsNullOrEmpty(Model.PageKey))
            {
                if (!m_navigation.TrySwitch(Model.PageKey))
                    m_navigation.TrySwitch(_navigationPageModels[0]);
            }
            else
            {
                m_navigation.TrySwitch(_navigationPageModels[0]);
            }
        }

        [Button] public void DisposeNavigation() => m_navigation?.Dispose();
        [Button] private void AddModel() => m_navigation?.Add(_a);
        [Button] private void RemoveModel() => m_navigation?.Remove(_a);

        protected override void OnDisposing() => m_navigation?.Dispose();
    }
}