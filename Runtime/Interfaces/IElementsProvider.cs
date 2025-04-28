using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements
{
    public interface IElementsProvider
    {
        public string Key { get; }
        IEnumerable<string> ExistKeys { get; }
        bool HasElement<T>(string key) where T : ElementBase;
        UniTask<T> GetElement<T>(string key, CancellationToken cancellationToken = default) where T : ElementBase;
        UniTask Prewarm();
        void Release();
    }
}