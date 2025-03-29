using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public interface IElementsProvider
    {
        public string Key { get; }
        IEnumerable<string> ExistKeys { get; }
        bool HasElement<T>(string key) where T : ElementBase;
        UniTask<T> GetElement<T>(string key) where T : ElementBase;
        UniTask Prewarm();
        void Release();
    }
}