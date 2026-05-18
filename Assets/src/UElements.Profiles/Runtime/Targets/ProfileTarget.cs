using UnityEngine;

namespace UElements.Profiles
{
    public abstract class ProfileTarget : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
    }
}