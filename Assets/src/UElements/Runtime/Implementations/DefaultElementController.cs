using Cysharp.Threading.Tasks;

namespace UElements
{
    public class DefaultElementController : IElementController
    {
        public UniTask Show(ElementBase element)
        {
            element.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public UniTask Hide(ElementBase element)
        {
            element.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}