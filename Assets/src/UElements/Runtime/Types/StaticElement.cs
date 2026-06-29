using Cysharp.Threading.Tasks;

namespace UElements
{
    public class StaticElement : Element
    {
        private void Start()
        {
            StartAsync().Forget();
        }

        private async UniTask StartAsync()
        {
            await InitializeInternal(ElementsGlobal.Instance, destroyCancellationToken);
            await Show();
        }
    }
}