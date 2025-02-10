using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UElements
{
    public interface IElements
    {
        UniTask<ElementBase> Create(ElementRequest request);
        UniTask<T> Create<T>(ElementRequest? request = null) where T : Element;
        UniTask<T> Create<T, TModel>(TModel model, ElementRequest? request = null) where T : ModelElement<TModel>;
        bool HasActive<T>(ElementRequest? request = null) where T : ElementBase;
        [CanBeNull] T GetActive<T>(ElementRequest? request = null) where T : ElementBase;
        void HideAll<T>(ElementRequest? request = null) where T : ElementBase;
        List<T> GetAll<T>(ElementRequest? request = null) where T : ElementBase;
    }
}