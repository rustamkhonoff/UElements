using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
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
}