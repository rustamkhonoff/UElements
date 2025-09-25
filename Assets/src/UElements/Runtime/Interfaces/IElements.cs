using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UElements
{
    public interface IElementsProviders
    {
        UniTask PrewarmProvider(string moduleKey);
        void Release();
        void Release(string key);
        UniTask<Type> GetElementTypeForRequest(ElementRequest request);
    }

    public interface IElementsCreator
    {
        UniTask<ElementBase> Create(ElementRequest request, object model, CancellationToken cancellationToken = default);
        UniTask<ElementBase> Create(ElementRequest request, CancellationToken cancellationToken = default);
        UniTask<T> Create<T>(ElementRequest? request = null, CancellationToken cancellationToken = default) where T : Element;
        UniTask<T> Create<T, TModel>(TModel model, ElementRequest? request = null, CancellationToken token = default) where T : ModelElement<TModel>;
    }

    public interface IElements : IElementsProviders, IElementsCreator
    {
        bool HasActive<T>(ElementRequest? request = null) where T : ElementBase;
        [CanBeNull] T GetActive<T>(ElementRequest? request = null) where T : ElementBase;
        UniTask CloseAll<T>(ElementRequest? request = null) where T : ElementBase;
        List<T> GetAll<T>(ElementRequest? request = null) where T : ElementBase;
    }
}