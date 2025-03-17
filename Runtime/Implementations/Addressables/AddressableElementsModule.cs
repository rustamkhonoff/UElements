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

        public override IElementsProvider ElementsProvider => new AddressableElementsProvider(Maps);
    }
}