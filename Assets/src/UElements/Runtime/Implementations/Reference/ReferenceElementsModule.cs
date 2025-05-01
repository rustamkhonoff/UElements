using System.Linq;
using UnityEngine;

namespace UElements.Reference
{
    [CreateAssetMenu(
        menuName = "Services/UElements/Reference/Create ReferenceElementsModule",
        fileName = "ReferenceElementsModule",
        order = 0)
    ]
    public class ReferenceElementsModule : ElementsModule
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }
        public override IElementsProvider ElementsProvider => new ReferenceElementsProvider(Key, Maps);

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
            }
        }
#endif
    }
}