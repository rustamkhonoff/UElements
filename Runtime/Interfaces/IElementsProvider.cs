using System;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public interface IElementsProvider : IDisposable
    {
        UniTask<T> GetElement<T>(string key) where T : ElementBase;
        void Release();
    }
}