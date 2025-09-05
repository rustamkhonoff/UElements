using System.Collections.Generic;
using UElements.CollectionView;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using System.Threading;
using R3;

namespace UElements.NavigationBar
{
    public static class NavigationBarBuilder
    {
        public static UniTask<INavigationPresenter<TModel>> Build<TModel, TView>(
            RectTransform contentParent,
            Func<TModel, ElementRequest> switcherRequestFactory)
            where TModel : INavigationPageModel
            where TView : NavigationSwitcherViewBase<TModel>
        {
            ReactiveCommand<TModel> switchCommand = new();

            ICollectionPresenter<TModel> switchersCollection = new CollectionPresenter<TModel, TView>(
                (model, view) => new NavigationModelPresenter<TModel, TView>(switchCommand, model, view),
                (model, ct) => ElementsGlobal.Create<TView, TModel>(model, switcherRequestFactory(model), ct)
            );

            INavigationPresenter<TModel> presenter = new NavigationPresenter<TModel>(switchersCollection, contentParent);
            INavigationPresenter<TModel> presenterWrapper = new NavigationPresenterCommandWrapper<TModel>(presenter, switchCommand);

            return UniTask.FromResult(presenterWrapper);
        }
    }

    public class NavigationPresenterCommandWrapper<TModel> : INavigationPresenter<TModel> where TModel : INavigationPageModel
    {
        private readonly INavigationPresenter<TModel> m_presenter;
        private readonly ReactiveCommand<TModel> m_command;
        private readonly CancellationTokenSource m_cts = new();

        public NavigationPresenterCommandWrapper(INavigationPresenter<TModel> presenter, ReactiveCommand<TModel> command)
        {
            m_presenter = presenter;
            m_command = command;

            command.Subscribe(a => TrySwitch(a)).AddTo(m_cts.Token);
        }

        public ReadOnlyReactiveProperty<TModel> ActivePage => m_presenter.ActivePage;
        public UniTask<bool> TrySwitch(TModel model) => m_presenter.TrySwitch(model);
        public UniTask<bool> TrySwitch(string key) => m_presenter.TrySwitch(key);
        public UniTask Add(TModel model) => m_presenter.Add(model);
        public void Remove(TModel model) => m_presenter.Remove(model);

        public void Dispose()
        {
            m_cts.Cancel();
            m_cts.Dispose();
            m_command.Dispose();
            m_presenter.Dispose();
        }
    }

    public class NavigationModelPresenter<TModel, TView> : CollectionModelPresenterBase<TModel, TView>
        where TModel : INavigationPageModel
        where TView : NavigationSwitcherViewBase<TModel>
    {
        private readonly ReactiveCommand<TModel> m_switchCommand;
        private readonly CancellationTokenSource m_cts = new();

        public NavigationModelPresenter(ReactiveCommand<TModel> switchCommand, TModel model, TView view) : base(model, view)
        {
            m_switchCommand = switchCommand;
        }

        public override void Initialize()
        {
            View.OnSwitchRequest.Subscribe(m_switchCommand.Execute).AddTo(m_cts.Token);
        }

        public override void Dispose()
        {
            m_cts.Cancel();
            m_cts.Dispose();
        }
    }
}