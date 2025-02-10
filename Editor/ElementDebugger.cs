using Sirenix.OdinInspector;
using UElements;
using UnityEngine;

namespace Editor
{
    [RequireComponent(typeof(ElementBase))]
    public class ElementDebugger : MonoBehaviour
    {
        [Button] private void Show() => GetComponent<ElementBase>().Show(() => Debug.Log("Show"));
        [Button] private void Hide() => GetComponent<ElementBase>().Hide(() => Debug.Log("Hide"));
    }
}