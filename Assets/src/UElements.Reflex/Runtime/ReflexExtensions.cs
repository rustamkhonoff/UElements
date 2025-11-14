using System;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace UElements.Zenject
{
    public static class ReflexExtensions
    {
        [Preserve]
        private class UElementsReflexFactory : IElementsFactory
        {
            public T Instantiate<T>(T prefab, Transform parent = null) where T : ElementBase
            {
                T instance = Object.Instantiate(prefab, parent);
                GameObjectInjector.InjectRecursive(instance.gameObject, Container.ProjectContainer);
                return instance;
            }
        }

        public static ContainerBuilder AddUElements(this ContainerBuilder container, string configurationPath = "ElementsConfigurationScriptableObject")
        {
            container.AddSingleton(typeof(Elements), typeof(IElements), typeof(IDisposable));
            container.AddSingleton(typeof(UElementsReflexFactory), typeof(IElementsFactory));
            container.AddSingleton(_ => Resources.Load<ElementsConfigurationScriptableObject>(configurationPath), typeof(IElementsConfiguration));

            container.OnContainerBuilt += Initialize;

            return container;
        }

        private static void Initialize(Container obj)
        {
            obj.Resolve<IElements>();
        }
    }
}