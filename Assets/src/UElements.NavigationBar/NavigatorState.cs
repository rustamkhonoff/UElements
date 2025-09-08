using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;
using System.Collections.Generic;

namespace UElements.NavigationBar
{
    public static class NavigationBuilder
    {
        public static UniTask<INavigation<TModel>> BuildNavigation<TModel, TView>(Func<TModel, ElementRequest> request, RectTransform parent)
            where TModel : INavigationPageModel
            where TView : NavigationTabViewBase<TModel>
        {
            INavigationState<TModel> state = new NavigationState<TModel>();
            INavigationContentFactory<TModel> contentFactory = new DefaultContentFactory<TModel>(parent);

            ICollectionPresenter<TModel> collectionPresenter = new CollectionPresenter<TModel, TView>
            (
                (model, view) => new NavigationTabPresenter<TModel, TView>(state, model, view),
                (model, token) => ElementsGlobal.Create<TView, TModel>(model, request(model), token)
            );
            INavigationPresenter<TModel> navigationPresenter = new NavigationPresenter<TModel>(state, contentFactory);

            INavigation<TModel> navigation = new Navigation<TModel>(state, collectionPresenter, navigationPresenter);

            return UniTask.FromResult(navigation);
        }
    }

    public interface INavigation<TModel> : IDisposable
        where TModel : INavigationPageModel
    {
        TModel ActivePage { get; }
        UniTask Add(TModel model);
        void Remove(TModel model);
        bool TrySwitch(TModel model);
        bool TrySwitch(string key);
    }

    public interface INavigationState<TModel>
        where TModel : INavigationPageModel
    {
        event Action<TModel> PageChanged;
        TModel ActivePage { get; }
        IReadOnlyDictionary<string, TModel> Pages { get; }
        void Register(TModel model);
        void UnRegister(TModel model);
        bool TrySwitch(string key);
        bool TrySwitch(TModel model);
    }

    public class NavigationState<TModel> : INavigationState<TModel>
        where TModel : INavigationPageModel
    {
        public event Action<TModel> PageChanged;

        private readonly Dictionary<string, TModel> m_pages = new();

        public TModel ActivePage { get; private set; }

        public IReadOnlyDictionary<string, TModel> Pages => m_pages;

        public void Register(TModel model)
        {
            m_pages[model.Key] = model;
        }

        public void UnRegister(TModel model)
        {
            m_pages.Remove(model.Key);
        }

        public bool TrySwitch(string key)
        {
            if (!m_pages.TryGetValue(key, out TModel model)) return false;
            return TrySwitch(model);
        }

        public bool TrySwitch(TModel model)
        {
            if (model == null) return false;
            if (ActivePage != null && ActivePage.Key == model.Key) return false;

            ActivePage = model;
            PageChanged?.Invoke(ActivePage);
            return true;
        }
    }

    public interface INavigationPresenter<in TModel>
        where TModel : INavigationPageModel
    {
        bool TrySwitch(TModel model);
        bool TrySwitch(string key);
        void Dispose();
    }

    internal class NavigationPresenter<TModel> : IDisposable, INavigationPresenter<TModel>
        where TModel : INavigationPageModel
    {
        private readonly INavigationState<TModel> m_state;
        private readonly INavigationContentFactory<TModel> m_factory;

        private ElementBase m_activeContent;

        public NavigationPresenter(INavigationState<TModel> state, INavigationContentFactory<TModel> factory)
        {
            m_state = state;
            m_factory = factory;

            m_state.PageChanged += OnActivePageChanged;
        }

        private void OnActivePageChanged(TModel model)
        {
            OnActivePageChangedAsync(model).Forget();
        }

        private async UniTask OnActivePageChangedAsync(TModel model)
        {
            m_activeContent.SafeDispose();
            if (model != null)
                m_activeContent = await m_factory.Create(model);
        }

        public bool TrySwitch(TModel model)
        {
            return m_state.TrySwitch(model);
        }

        public bool TrySwitch(string key)
        {
            return m_state.TrySwitch(key);
        }

        public void Dispose()
        {
            m_activeContent.SafeDispose();
            m_state.PageChanged -= OnActivePageChanged;
        }
    }

    public class NavigationTabPresenter<TModel, TView> : CollectionModelPresenterBase<TModel, TView>
        where TModel : INavigationPageModel
        where TView : NavigationTabViewBase<TModel>
    {
        private readonly INavigationState<TModel> m_state;

        public NavigationTabPresenter(INavigationState<TModel> state, TModel model, TView view) : base(model, view)
        {
            m_state = state;
        }

        public override void Initialize()
        {
            View.SwitchRequested += OnSwitchRequested;
            m_state.PageChanged += HandlePageChanged;

            View.SetInitialState(m_state.ActivePage != null && Model.Key == m_state.ActivePage.Key);
        }

        private void HandlePageChanged(TModel obj)
        {
            View.SetState(obj != null && obj.Key == Model.Key);
        }

        private void OnSwitchRequested(TModel model)
        {
            m_state.TrySwitch(model);
        }

        public override void Dispose()
        {
            View.SafeDispose();
            View.SwitchRequested -= OnSwitchRequested;
            m_state.PageChanged -= HandlePageChanged;
        }
    }

    public abstract class NavigationTabViewBase<TModel> : ModelElement<TModel>
        where TModel : INavigationPageModel
    {
        public event Action<TModel> SwitchRequested;

        protected void Switch() => SwitchRequested?.Invoke(Model);

        public void SetState(bool state) => OnSetState(state);
        public virtual void SetInitialState(bool state) => OnSetInitialState(state);
        protected abstract void OnSetState(bool state);
        protected virtual void OnSetInitialState(bool state) { }
    }

    internal class Navigation<TModel> : INavigation<TModel>
        where TModel : INavigationPageModel
    {
        private readonly INavigationState<TModel> m_state;
        private readonly INavigationPresenter<TModel> m_presenter;
        private readonly ICollectionPresenter<TModel> m_collection;

        public Navigation(
            INavigationState<TModel> state,
            ICollectionPresenter<TModel> collection,
            INavigationPresenter<TModel> presenter)
        {
            m_state = state;
            m_collection = collection;
            m_presenter = presenter;
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
            return m_presenter.TrySwitch(model);
        }

        public bool TrySwitch(string key)
        {
            return m_presenter.TrySwitch(key);
        }

        public void Dispose()
        {
            m_presenter.Dispose();
            m_collection.Dispose();
        }
    }
}