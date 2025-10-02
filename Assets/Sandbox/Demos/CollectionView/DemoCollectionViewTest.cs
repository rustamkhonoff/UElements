using System.Collections.Generic;
using Sirenix.OdinInspector;
using UElements;
using UElements.CollectionView;
using UnityEngine;

namespace Demos.CollectionView
{
    internal class DemoCollectionViewTest : MonoBehaviour
    {
        [SerializeField] private List<Model> _models = new();
        [SerializeField] private CollectionItemRequest _request;
        [SerializeField] private CollectionItemRequest _kingRequest;

        private ICollectionPresenter<Model, ModelView> m_collectionPresenter;

        private async void Start()
        {
            m_collectionPresenter = await _models.BuildCollectionPresenter<Model, ModelView>(PresenterFactory);
        }

        private ICollectionModelPresenter<Model, ModelView> PresenterFactory(Model arg)
        {
            return new DemoElementCollectionItemPresenter(arg, model => ElementsGlobal.Instance.Create<ModelView>(_request));
        }


        private Model a = new(1000, "A");
        private Model b = new(100, "B");
        private Model c = new(10, "C");
        [Button] private void AddA() => m_collectionPresenter.Add(a);
        [Button] private void AddB() => m_collectionPresenter.Add(b);
        [Button] private void AddC() => m_collectionPresenter.Add(c);
        [Button] private void RemoveA() => m_collectionPresenter.Remove(a);
        [Button] private void RemoveB() => m_collectionPresenter.Remove(b);
        [Button] private void RemoveC() => m_collectionPresenter.Remove(c);

        private void OnDestroy() => m_collectionPresenter.Dispose();
    }
}