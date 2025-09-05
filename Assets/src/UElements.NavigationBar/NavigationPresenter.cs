using System.Collections.Generic;
using System.Linq;
using UElements.CollectionView;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace UElements.NavigationBar
{
    internal class NavigationPresenter<TPageModel> : INavigationPresenter<TPageModel>
        where TPageModel : INavigationPageModel
    {
        private readonly Dictionary<string, TPageModel> m_models = new();
        private readonly ReactiveProperty<TPageModel> m_activePage = new();
        public ReadOnlyReactiveProperty<TPageModel> ActivePage => m_activePage;

        private readonly ICollectionPresenter<TPageModel> m_collection;
        private readonly RectTransform m_contentParent;
        private ElementBase m_activeContent;

        public NavigationPresenter(ICollectionPresenter<TPageModel> collectionPresenter, RectTransform contentParent)
        {
            m_collection = collectionPresenter;
            m_contentParent = contentParent;
        }

        public async UniTask Add(TPageModel model)
        {
            if (m_models.ContainsKey(model.Key)) return;

            m_models[model.Key] = model;

            await m_collection.Add(model);
        }

        public void Remove(TPageModel model)
        {
            if (!m_models.ContainsKey(model.Key)) return;

            m_models.Remove(model.Key);

            m_collection.Remove(model);
        }

        public UniTask<bool> TrySwitch(string key)
        {
            return TrySwitch(m_models[key]);
        }

        public async UniTask<bool> TrySwitch(TPageModel model)
        {
            if (model == null || !m_models.ContainsKey(model.Key)) return false;
            if (m_activePage.Value != null && m_activePage.Value.Key == model.Key) return false;

            m_activePage.Value = model;

            if (m_activeContent != null) m_activeContent.SafeDispose();
            m_activeContent = await ElementsGlobal.Create(model.ContentRequest.WithParent(m_contentParent));

            return true;
        }

        public void Dispose()
        {
            m_collection?.Dispose();
            m_activePage?.Dispose();
        }
    }
}