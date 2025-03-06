using UnityEngine;

namespace UElements.Resource
{
    [CreateAssetMenu(menuName = "Services/UElements/Resources/Create AddressableElementsModule", fileName = "AddressableElementsModule",
        order = 0)]
    public class ResourceElementsModule : ElementsModule
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }

        public override IElementsProvider ElementsProvider => new ResourceElementsProvider(Maps);
    }
}