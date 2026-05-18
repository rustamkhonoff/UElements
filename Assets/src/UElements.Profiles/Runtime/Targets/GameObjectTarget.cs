using UnityEngine;

namespace UElements.Profiles
{
    public sealed class GameObjectTarget : ProfileTarget
    {
        [field: SerializeField] public GameObject Target { get; private set; }
        private void Reset() => Target = gameObject;
    }
}