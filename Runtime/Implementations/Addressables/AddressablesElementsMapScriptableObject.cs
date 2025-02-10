using UnityEngine;

namespace Addressables
{
    [CreateAssetMenu(menuName = "Services/UElements/Create AddressablesElementsMapScriptableObject",
        fileName = "AddressablesElementsMapScriptableObject", order = 0)]
    public class AddressableElementsMapScriptableObject : ScriptableObject
    {
        [field: SerializeField] public ElementMap[] Maps { get; private set; }
    }
}