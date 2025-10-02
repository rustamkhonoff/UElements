using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Demos.NavigationView;
using R3;
using Sirenix.OdinInspector;
using UElements;
using UElements.NavigationBar;
using UnityEngine;

namespace Sandbox.Demos.NavigationView
{
    public class DemoNavigationBar : Element
    {
        [SerializeField] private RectTransform _contentParent;
        [SerializeField] private ReferenceElementRequest _navigationBarElementRequest;
        [SerializeReference] private DemoNavigationModel[] _navigationPageModels;
        [SerializeReference] private DemoNavigationModel _hidden;
        private INavigation<DemoNavigationModel> m_navigation;

        public override async void Initialize()
        {
            m_navigation = await NavigationBuilder.BuildNavigation<DemoNavigationModel, NavigationTab>(CreateSwitcher);
            m_navigation.PageChanged += HandlePageChange;
            m_navigation.ContentCreated += ContentCreated;

            foreach (DemoNavigationModel navigationPageModel in _navigationPageModels)
                await m_navigation.Add(navigationPageModel, GetContentBuilder);

            m_navigation.TrySwitch(_navigationPageModels[0].Key);
        }

        private UniTask<NavigationTab> CreateSwitcher(DemoNavigationModel arg1)
        {
            return ElementsGlobal.Instance.Create<NavigationTab, DemoNavigationModel>(arg1, _navigationBarElementRequest);
        }

        [Button]
        private void Add()
        {
            m_navigation.Add(_hidden, GetContentBuilder);
        }

        [Button]
        private void Remove()
        {
            m_navigation.Remove(_hidden);
        }

        private void ContentCreated(DemoNavigationModel demoNavigationModel, object o)
        {
            Debug.Log(o);
        }

        private INavigationContentPresenter GetContentBuilder(DemoNavigationModel model)
        {
            if (model.Key == "a")
            {
                return new ElementNavigationContentPresenter<DemoNavigationModel, PageA>
                (
                    model,
                    a => a.ElementRequest,
                    view => view.OnEndEdit.Subscribe(a => Debug.Log(a.Name))
                );
            }

            return new ElementNavigationContentPresenter<DemoNavigationModel>(model, a => a.ElementRequest);
        }

        private void HandlePageChange(INavigationModel navigationModel)
        {
            Debug.Log(navigationModel.Key);
        }


        [Button] public void DisposeNavigation() => m_navigation?.Dispose();

        protected override void OnDisposing()
        {
            m_navigation.ContentCreated -= ContentCreated;
            m_navigation.PageChanged -= HandlePageChange;
            m_navigation?.Dispose();
        }
    }
}