using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using UElements;
using UElements.CollectionView;
using UElements.R3;
using UnityEngine;

namespace Demos.CollectionView
{
    public class DemoCollectionViewTest : MonoBehaviour
    {
        [SerializeField] private List<DemoCollectionModel> _models = new();
        [SerializeField] private CollectionItemRequest _request;
        [SerializeField] private CollectionItemRequest _kingRequest;

        private ICollectionPresenter<DemoCollectionModel, DemoCollectionItemView> m_collectionPresenter;
        private ObservableList<DemoCollectionModel> m_modelsReactive = new();

        private void Start()
        {
            m_modelsReactive = new ObservableList<DemoCollectionModel>(_models);

            m_collectionPresenter = CollectionPresenterBuilder.Build<DemoCollectionModel, DemoCollectionItemView>(Request, OnItemCreated, OnItemDisposed);
            m_collectionPresenter.Initialize(m_modelsReactive);

            m_modelsReactive
                .SubscribeEvents(a => m_collectionPresenter.Add(a).Forget(), m_collectionPresenter.Remove, m_collectionPresenter.Clear)
                .AddTo(this);
        }


        private ElementRequest Request(DemoCollectionModel model)
        {
            return model.Nickname.Value == "King" ? _kingRequest : _request;
        }

        private void OnItemCreated(DemoCollectionModel arg1, DemoCollectionItemView arg2) => arg2.AddCallback(HandleClick);
        private void HandleClick(DemoCollectionModel obj) => obj.IncreaseAmount();
        private void OnItemDisposed(DemoCollectionModel arg1, DemoCollectionItemView arg2) => arg2.RemoveCallback(HandleClick);
        private void OnDestroy() => m_collectionPresenter.Dispose();
    }
}