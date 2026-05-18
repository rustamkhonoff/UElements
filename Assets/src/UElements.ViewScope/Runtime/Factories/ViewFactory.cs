using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UElements.ViewScope
{
    public class ViewFactory<TBase, TConcrete> : IViewFactory<TBase>
        where TConcrete : class, TBase, IDisposable
    {
        private readonly Func<object[], TConcrete> m_instantiator;
        private readonly object[] m_args;

        public ViewFactory(Func<object[], TConcrete> instantiator, params object[] args)
        {
            m_instantiator = instantiator;
            m_args = args;
        }

        public UniTask<IPresentationScope<TBase>> Create(CancellationToken rootToken = default)
        {
            PresentationScope<TBase> scope = new(rootToken);
            TConcrete view = m_instantiator(m_args);
            scope.Token.Register(view.Dispose);
            scope.AttachView(view);
            return new UniTask<IPresentationScope<TBase>>(scope);
        }
    }
}