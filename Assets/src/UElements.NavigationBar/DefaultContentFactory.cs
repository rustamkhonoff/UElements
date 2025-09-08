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

        public UniTask<ElementBase> Create(TModel model)
        {
            return ElementsGlobal.Create(model.ContentRequest.WithParent(m_parent));
        }
    }
}