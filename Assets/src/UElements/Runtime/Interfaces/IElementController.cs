using System;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public interface IElementController
    {
        UniTask Show(ElementBase element, Action callback);
        UniTask Hide(ElementBase element, Action callback);
    }
}