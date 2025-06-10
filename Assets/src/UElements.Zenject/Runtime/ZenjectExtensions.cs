using UElements.Addressables;
using UElements.Resource;
using UnityEngine;
using Zenject;

namespace UElements.Zenject
{
    public static class ZenjectExtensions
    {
        public class UElementsZenjectFactory : IElementsFactory
        {
            private readonly IInstantiator m_instantiator;

            public UElementsZenjectFactory(IInstantiator instantiator)
            {
                m_instantiator = instantiator;
            }

            public T Instantiate<T>(T prefab, Transform parent = null) where T : ElementBase
            {
                return m_instantiator.InstantiatePrefab(prefab, parent).GetComponent<T>();
            }
        }

        public static DiContainer AddUElements(this DiContainer container, string configurationPath = "ElementsConfigurationScriptableObject")
        {
            //CORE
            container
                .BindInterfacesTo<Elements>()
                .AsSingle();

            container
                .BindInterfacesTo<UElementsZenjectFactory>()
                .AsSingle();

            container
                .BindInterfacesTo<ElementsConfigurationScriptableObject>()
                .FromResource(configurationPath)
                .AsSingle();

            return container;
        }
    }
}