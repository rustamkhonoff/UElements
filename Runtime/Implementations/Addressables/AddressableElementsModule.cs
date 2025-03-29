using System.Linq;
using UnityEngine;

namespace UElements.Addressables
{
    [CreateAssetMenu(
        menuName = "Services/UElements/Addressables/Create AddressableElementsModule",
        fileName = "AddressableElementsModule",
        order = 0)
    ]
    public class AddressableElementsModule : ElementsModule
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }

        public override IElementsProvider ElementsProvider => new AddressableElementsProvider(Key, Maps);

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
            foreach (ElementMap elementMap in Maps.Where
                         (a => a.AssetReference != null && a.AssetReference.editorAsset != null))
            {
                if (UElementsExtensions.IsEmptyKey(elementMap.Key))
                    elementMap.Key = UElementsExtensions.GetKey(elementMap.AssetReference.editorAsset);
            }
        }
#endif
    }
}