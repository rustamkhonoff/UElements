using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public interface INavigationModel
    {
        string Key { get; }
    }

    public interface INavigationContentPresenter
    {
        UniTask Enable();
        UniTask Disable();
    }

    public interface INavigationTabPresenter
    {
        UniTask Enable();
        UniTask Disable();
    }

    public class ElementNavigationContentPresenter<TModel> : INavigationContentPresenter
        where TModel : INavigationModel
    {
        private readonly TModel m_model;
        private readonly Func<TModel, ElementRequest> m_selector;
        private readonly Action<ElementBase> m_onViewCreated;
        private ElementBase m_elementBase;

        public ElementNavigationContentPresenter(TModel model, Func<TModel, ElementRequest> selector, Action<ElementBase> onViewCreated = null)
        {
            m_model = model;
            m_selector = selector;
            m_onViewCreated = onViewCreated;
        }

        public async UniTask Enable()
        {
            m_elementBase = await ElementsGlobal.Instance.Create(m_selector(m_model));
            m_onViewCreated?.Invoke(m_elementBase);
        }

        public UniTask Disable()
        {
            m_elementBase.SafeDispose();
            return UniTask.CompletedTask;
        }
    }

    public class ElementNavigationContentPresenter<TModel, TView> : INavigationContentPresenter
        where TModel : INavigationModel
        where TView : Element
    {
        private readonly TModel m_model;
        private readonly Func<TModel, ElementRequest> m_selector;
        private readonly Action<TView> m_onViewCreated;
        private TView m_view;

        public ElementNavigationContentPresenter(TModel model, Func<TModel, ElementRequest> selector, Action<TView> onViewCreated)
        {
            m_model = model;
            m_selector = selector;
            m_onViewCreated = onViewCreated;
        }

        public async UniTask Enable()
        {
            m_view = await ElementsGlobal.Instance.Create<TView>(m_selector(m_model));
            m_onViewCreated?.Invoke(m_view);
        }

        public UniTask Disable()
        {
            m_view.SafeDispose();
            return UniTask.CompletedTask;
        }
    }
}