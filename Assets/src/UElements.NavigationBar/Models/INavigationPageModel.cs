using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
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