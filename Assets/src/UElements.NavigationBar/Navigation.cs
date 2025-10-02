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

        private readonly INavigationState<TModel> m_state;
        private readonly INavigationPresenter m_presenter;
        private readonly ICollectionPresenter<TModel, TTabView> m_collection;

        public Navigation(
            INavigationState<TModel> state,
            ICollectionPresenter<TModel, TTabView> collection,
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

        private void HandlePageChanged(NavigationEntry<TModel> obj)
        {
            PageChanged?.Invoke(obj.Model);
        }

        public TModel ActivePage => m_state.ActivePage;

        public UniTask Add(TModel model, Func<TModel, INavigationContentPresenter> contentBuilder)
        {
            m_state.Register(model, contentBuilder);
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