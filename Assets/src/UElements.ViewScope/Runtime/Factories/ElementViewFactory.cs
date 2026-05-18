using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.ViewScope
{
    public class ElementViewFactory<TBase, TConcrete> : IViewFactory<TBase>
        where TConcrete : Element, TBase
        where TBase : class
    {
        private readonly IElementsCreator m_elements;
        private readonly ElementRequest? m_request;

        public ElementViewFactory(IElementsCreator elements, ElementRequest? request = null)
        {
            m_elements = elements;
            m_request = request;
        }

        public async UniTask<IPresentationScope<TBase>> Create(CancellationToken rootToken = default)
        {
            PresentationScope<TBase> scope = new(rootToken);
            TConcrete view = await m_elements.Create<TConcrete>(new ElementCreateOptions(m_request, scope.Token, rootToken));
            scope.AttachView(view);

            view.LifetimeToken.Register(scope.Dispose);

            return scope;
        }
    }

    public class ElementViewFactory<TConcrete> : IViewFactory<TConcrete>
        where TConcrete : Element
    {
        private readonly IElementsCreator m_elements;
        private readonly ElementRequest? m_request;

        public ElementViewFactory(IElementsCreator elements, ElementRequest? request = null)
        {
            m_elements = elements;
            m_request = request;
        }

        public async UniTask<IPresentationScope<TConcrete>> Create(CancellationToken rootToken = default)
        {
            PresentationScope<TConcrete> scope = new(rootToken);
            TConcrete view = await m_elements.Create<TConcrete>(new ElementCreateOptions(m_request, scope.Token, rootToken));
            scope.AttachView(view);

            view.LifetimeToken.Register(scope.Dispose);

            return scope;
        }
    }
}