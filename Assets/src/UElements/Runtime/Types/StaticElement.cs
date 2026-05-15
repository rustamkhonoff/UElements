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
            InitializeElements(ElementsGlobal.Instance);
            await InitializeAsync();
            // ReSharper disable once MethodHasAsyncOverload
            Initialize();
            await Show();
        }
    }
}