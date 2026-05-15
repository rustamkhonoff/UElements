using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UElements
{
    public interface IElementsCreator
    {
        UniTask<ElementBase> Create(object model, ElementCreateOptions options);
        UniTask<ElementBase> Create(ElementCreateOptions options);
        UniTask<T> Create<T>(ElementCreateOptions options) where T : Element;
        UniTask<T> Create<T, TModel>(TModel model, ElementCreateOptions options) where T : ModelElement<TModel>;
    }

    public interface IElementsStateController
    {
        bool HasActive<T>(ElementRequest? request = null) where T : ElementBase;
        [CanBeNull] T GetActive<T>(ElementRequest? request = null) where T : ElementBase;
        UniTask CloseAll<T>(ElementRequest? request = null) where T : ElementBase;
        List<T> GetAll<T>(ElementRequest? request = null) where T : ElementBase;
    }

    public interface IElementsProvidersController
    {
        UniTask<Type> GetElementTypeForRequest(ElementRequest request);
        UniTask PrewarmProvider(string moduleKey);
        void ReleaseAllProviders();
        void ReleaseProvider(string key);
    }

    public interface IElements : IElementsCreator, IElementsStateController, IElementsProvidersController { }
}