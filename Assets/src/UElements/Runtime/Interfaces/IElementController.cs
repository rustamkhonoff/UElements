using Cysharp.Threading.Tasks;

namespace UElements
{
    public interface IElementController
    {
        UniTask Show(ElementBase element);
        UniTask Hide(ElementBase element);
    }
}