using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UElements.Resource
{
    [CreateAssetMenu(
        menuName = "Services/UElements/Resources/Create ResourceElementsModule",
        fileName = "ResourceElementsModule",
        order = 0)
    ]
    public class ResourceElementsModule : ElementsModule
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }

        public override IElementsProvider ElementsProvider => new ResourceElementsProvider(Key, Maps);

#if UNITY_EDITOR
        [field: SerializeField] public bool AutoValidate { get; private set; } = true;

        private void OnValidate()
        {
            if (AutoValidate)
                Validate();
        }

        [ContextMenu("Validate")]
        private void Validate()
        {
            foreach (ElementMap elementMap in Maps.Where(a => a.Prefab != null))
            {
                if (UElementsExtensions.IsEmptyKey(elementMap.Key))
                    elementMap.Key = UElementsExtensions.GetKey(elementMap.Prefab.gameObject);

                elementMap.Path = UElementsExtensions.GetResourcesPath(elementMap.Prefab);
            }
        }
#endif
    }
}