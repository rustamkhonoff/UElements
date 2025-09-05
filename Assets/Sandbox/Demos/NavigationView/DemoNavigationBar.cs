using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        private INavigationPresenter<DemoNavigationModel> m_navigationPresenter;

        private void Start() => CreateNavigation().Forget();

        [Button]
        private async UniTask CreateNavigation()
        {
            m_navigationPresenter =
                await NavigationBarBuilder.Build<DemoNavigationModel, NavigationSwitcherView>(_contentParent, _ => _switcherRequest);

            foreach (DemoNavigationModel navigationPageModel in _navigationPageModels)
                await m_navigationPresenter.Add(navigationPageModel);

            m_navigationPresenter.TrySwitch(_navigationPageModels[0]);
        }

        [Button]
        private void DisposeNavigation() => m_navigationPresenter?.Dispose();
    }
}