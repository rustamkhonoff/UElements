using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UElements.VContainer
{
    public static class ZenjectExtensions
    {
        [UnityEngine.Scripting.Preserve]
        private class UElementsVContainerFactory : IElementsFactory
        {
            private readonly IObjectResolver m_resolver;

            public UElementsVContainerFactory(IObjectResolver resolver)
            {
                m_resolver = resolver;
            }

            public T Instantiate<T>(T prefab, Transform parent = null) where T : ElementBase
            {
                return m_resolver.Instantiate(prefab, parent);
            }
        }

        public static IContainerBuilder AddUElements(this IContainerBuilder container, string configurationPath = "ElementsConfigurationScriptableObject")
        {
            container
                .Register<Elements>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            container
                .Register<UElementsVContainerFactory>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            var resource = Resources.Load<ElementsConfigurationScriptableObject>(configurationPath);
            container
                .RegisterInstance(resource)
                .AsImplementedInterfaces();

            container.RegisterBuildCallback(r =>
            {
                r.Resolve<IElements>();
            });

            return container;
        }
    }
}