using System;

namespace UElements
{
    public interface IElementController
    {
        void Show(Action callback);
        void Hide(Action callback);
    }
}