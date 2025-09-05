using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using Sirenix.OdinInspector;
using UElements;
using UElements.CollectionView;
using UElements.R3;
using UnityEngine;

namespace Demos.CollectionView
{
    public class DemoCollectionViewTest : MonoBehaviour
    {
        [SerializeField] private List<Model> _models = new();
        [SerializeField] private CollectionItemRequest _request;
        [SerializeField] private CollectionItemRequest _kingRequest;

        private ICollectionPresenter<Model> m_collectionPresenter;

        private void Start()
        {
            m_collectionPresenter = _models.BuildCollectionPresenter<Model, ModelView>(PresenterFactory, RequestFactory);
        }

        private ElementRequest RequestFactory(Model model) => model.Nickname.Value == "King" ? _kingRequest : _request;
        private CollectionModelPresenter PresenterFactory(Model arg1, ModelView arg2) => new(arg1, arg2);

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

    public static class Test
    {
        public static IDisposable BindToCollection<TModel>(this ICollectionPresenter<TModel> presenter, IObservableCollection<TModel> collection)
        {
            CompositeDisposable compositeDisposable = new();

            presenter.Initialize(collection);
            collection.SubscribeEvents(
                added => presenter.Add(added).Forget(),
                presenter.Remove,
                presenter.Clear
            ).AddTo(compositeDisposable);

            return compositeDisposable;
        }
    }
}