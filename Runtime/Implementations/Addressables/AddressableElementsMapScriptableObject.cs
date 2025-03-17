using UnityEngine;

namespace UElements.Addressables
{
    [CreateAssetMenu(
        menuName = "Services/UElements/Addressables/Create AddressablesElementsMapScriptableObject",
        fileName = "AddressablesElementsMapScriptableObject",
        order = 0)
    ]
    public class AddressableElementsMapScriptableObject : ScriptableObject
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }
    }
}