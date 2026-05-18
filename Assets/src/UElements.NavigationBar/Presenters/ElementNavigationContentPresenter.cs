using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public class ElementNavigationContentPresenter<TModel> : ElementNavigationContentPresenter<TModel, ElementBase>
        where TModel : INavigationModel
    {
        public ElementNavigationContentPresenter(TModel model, Func<TModel, UniTask<ElementBase>> contentFactory, Action<ElementBase> onViewCreated = null) : base(model, contentFactory, onViewCreated) { }
    }

    public class ElementNavigationContentPresenter<TModel, TView> : INavigationContentPresenter
        where TModel : INavigationModel
        where TView : ElementBase
    {
        private readonly TModel m_model;
        private readonly Func<TModel, UniTask<TView>> _contentFactory;
        private readonly Action<TView> m_onViewCreated;
        private TView m_elementBase;

        public ElementNavigationContentPresenter(TModel model, Func<TModel, UniTask<TView>> contentFactory, Action<TView> onViewCreated = null)
        {
            m_model = model;
            _contentFactory = contentFactory;
            m_onViewCreated = onViewCreated;
        }

        public async UniTask Enable()
        {
            m_elementBase = await _contentFactory(m_model);
            m_onViewCreated?.Invoke(m_elementBase);
        }

        public async UniTask Disable()
        {
            if (m_elementBase != null)
            {
                await m_elementBase.Hide();
                m_elementBase.SafeDispose();
            }
        }
    }
}