using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UElements.CollectionView;
using UnityEngine;

namespace UElements.NavigationBar
{
    // public interface INavigationPresenter : IDisposable { }
    //
    // [Serializable]
    // public class NavigationPresenter<TModel, TView> : IDisposable, INavigationPresenter where TView : NavigationSwitcherView<TModel>
    //     where TModel : INavigationPageModel
    // {
    //     private readonly ICollectionPresenter<TModel, TView> m_collectionPresenter;
    //
    //     public NavigationPresenter(
    //         INavigationState<TModel> state,
    //         RectTransform contentParent,
    //         Func<TModel, CancellationToken, UniTask<TView>> switcherFactory)
    //     {
    //         m_collectionPresenter = new CollectionPresenter<TModel, TView>(
    //             (model, view) => new NavigationSwitcherPresenter<TModel, TView>(model, view, state, contentParent),
    //             switcherFactory
    //         );
    //
    //         foreach (TModel pageModel in state.Models)
    //             m_collectionPresenter.Add(pageModel).Forget();
    //     }
    //
    //
    //     public void Dispose()
    //     {
    //         m_collectionPresenter.Dispose();
    //     }
    // }
}