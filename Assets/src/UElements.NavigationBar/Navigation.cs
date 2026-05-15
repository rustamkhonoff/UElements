using System;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;

namespace UElements.NavigationBar
{
    internal class Navigation<TModel, TTabView> : INavigation<TModel>
        where TModel : INavigationModel
    {
        public event Action<TModel> PageChanged;
        public event Action<TModel, object> ContentCreated;

        private readonly NavigationState<TModel> m_state;
        private readonly INavigationPresenter m_presenter;
        private readonly ICollectionPresenter<TModel> m_collection;

        public Navigation(
            NavigationState<TModel> state,
            ICollectionPresenter<TModel> collection,
            INavigationPresenter presenter)
        {
            m_state = state;
            m_collection = collection;
            m_presenter = presenter;
            m_state.PageChanged += HandlePageChanged;
            m_presenter.ContentCreated += HandContentCreated;
        }

        private void HandContentCreated(object obj)
        {
            ContentCreated?.Invoke(ActivePage, obj);
        }

        private void HandlePageChanged(TModel obj)
        {
            PageChanged?.Invoke(obj);
        }

        public TModel ActivePage => m_state.ActivePage;

        public UniTask Add(TModel model)
        {
            m_state.Register(model);
            return m_collection.Add(model);
        }

        public void Remove(TModel model)
        {
            m_state.UnRegister(model);
            m_collection.Remove(model);
        }

        public bool TrySwitch(TModel model)
        {
            return TrySwitch(model.Key);
        }

        public bool TrySwitch(string key)
        {
            return m_presenter.TrySwitch(key);
        }

        public void Dispose()
        {
            m_presenter.ContentCreated -= HandContentCreated;
            m_state.PageChanged -= HandlePageChanged;
            m_presenter.Dispose();
            m_collection.Dispose();
        }
    }
}