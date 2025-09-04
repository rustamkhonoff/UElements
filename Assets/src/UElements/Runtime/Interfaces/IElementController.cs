using System;

namespace UElements
{
    public interface IElementController
    {
        void Show(ElementBase element, Action callback);
        void Hide(ElementBase element, Action callback);
    }
}