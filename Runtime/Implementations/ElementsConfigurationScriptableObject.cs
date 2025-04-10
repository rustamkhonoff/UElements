using System.Collections.Generic;
using UnityEngine;

namespace UElements
{
    [CreateAssetMenu(
        menuName = "Services/UElements/Create ElementsConfigurationScriptableObject",
        fileName = "ElementsConfigurationScriptableObject",
        order = 0)
    ]
    public class ElementsConfigurationScriptableObject : ScriptableObject, IElementsConfiguration
    {
        [field: SerializeField] public ElementsRoot ElementsRootPrefab { get; private set; }
        [field: SerializeField] public List<ElementsModule> Modules { get; private set; }
    }
}