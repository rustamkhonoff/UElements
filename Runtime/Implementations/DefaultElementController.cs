using System;

namespace UElements
{
    public class DefaultElementController : IElementController
    {
        public void Show(Action callback)
        {
            callback?.Invoke();
        }

        public void Hide(Action callback)
        {
            callback?.Invoke();
        }
    }
}