using System;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public class DefaultElementController : IElementController
    {
        public UniTask Show(ElementBase element, Action callback)
        {
            element.gameObject.SetActive(true);
            callback?.Invoke();
            return UniTask.CompletedTask;
        }

        public UniTask Hide(ElementBase element, Action callback)
        {
            element.gameObject.SetActive(false);
            callback?.Invoke();
            return UniTask.CompletedTask;
        }
    }
}