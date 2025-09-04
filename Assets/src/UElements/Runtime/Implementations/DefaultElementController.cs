using System;

namespace UElements
{
    public class DefaultElementController : IElementController
    {
        public void Show(ElementBase element, Action callback)
        {
            element.gameObject.SetActive(true);
            callback?.Invoke();
        }

        public void Hide(ElementBase element, Action callback)
        {
            element.gameObject.SetActive(false);
            callback?.Invoke();
        }
    }
}