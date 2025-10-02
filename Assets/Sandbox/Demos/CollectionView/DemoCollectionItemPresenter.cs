using System;
using Cysharp.Threading.Tasks;
using R3;
using UElements;
using UElements.CollectionView;
using UElements.R3;

namespace Demos.CollectionView
{
    internal class DemoElementCollectionItemPresenter : ElementCollectionItemPresenter<Model, ModelView>
    {
        public DemoElementCollectionItemPresenter(Model model, Func<Model, UniTask<ModelView>> viewFactory) : base(model, viewFactory) { }

        public override async UniTask Enable()
        {
            await base.Enable();

            View.AddTo(LifetimeToken);

            Model.AnyValueChanged
                .Subscribe(data => View.SetText(data.name + ":" + data.health))
                .AddTo(LifetimeToken);

            View.Clicked
                .SubscribeCallback(Model.IncreaseAmount)
                .AddTo(LifetimeToken);
        }
    }
}