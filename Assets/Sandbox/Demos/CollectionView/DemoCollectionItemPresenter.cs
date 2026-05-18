using System;
using Cysharp.Threading.Tasks;
using R3;
using UElements;
using UElements.CollectionView;

namespace Demos.CollectionView
{
    internal class DemoElementCollectionItemPresenterBase : ElementCollectionItemPresenterBase<Model, ModelView>
    {
        public DemoElementCollectionItemPresenterBase(Model model, Func<Model, UniTask<ModelView>> viewFactory) : base(model, viewFactory) { }

        public override async UniTask Enable()
        {
            await base.Enable();

            View.AddTo(LifetimeToken);

            Model.AnyValueChanged
                .Subscribe(data => View.SetText(data.name + ":" + data.health))
                .AddTo(LifetimeToken);

            View.Clicked
                .Subscribe(_ => Model.IncreaseAmount())
                .AddTo(LifetimeToken);
        }
    }
}