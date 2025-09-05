using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UElements;
using UElements.CollectionView;
using UElements.R3;

namespace Demos.CollectionView
{
    public class CollectionModelPresenter : CollectionModelPresenterBase<Model, ModelView>
    {
        public CollectionModelPresenter(Model model, ModelView view) : base(model, view) { }

        private readonly CancellationTokenSource m_cts = new();

        public override void Initialize()
        {
            View.AddTo(m_cts);

            Model.AnyValueChanged
                .Subscribe(data => View.SetText(data.name + ":" + data.health))
                .AddTo(m_cts.Token);

            View.Clicked
                .SubscribeCallback(Model.IncreaseAmount)
                .AddTo(m_cts.Token);
        }

        public override void Dispose()
        {
            m_cts.Cancel();
            m_cts.Dispose();
        }
    }
}