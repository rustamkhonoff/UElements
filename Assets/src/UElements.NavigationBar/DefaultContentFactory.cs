using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.NavigationBar
{
    public class DefaultContentFactory<TModel> : INavigationContentFactory<TModel>
        where TModel : INavigationPageModel
    {
        private readonly RectTransform m_parent;

        public DefaultContentFactory(RectTransform parent)
        {
            m_parent = parent;
        }

        public async UniTask<ElementBase> Create(TModel model)
        {
            if (await ElementsGlobal.GetElementTypeForRequest(model.ContentRequest) == typeof(ModelElement<TModel>))
                return await ElementsGlobal.Create(model, model.ContentRequest.WithParent(m_parent));
            else
                return await ElementsGlobal.Create(model.ContentRequest.WithParent(m_parent));
        }
    }
}