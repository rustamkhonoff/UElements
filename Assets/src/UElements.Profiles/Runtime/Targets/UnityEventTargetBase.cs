using UnityEngine;
using UnityEngine.Events;

namespace UElements.Profiles
{
    public class UnityEventTargetBase<T> : ProfileTarget
    {
        [field: SerializeField] public UnityEvent<T> Event { get; private set; }
        [field: SerializeField] public T LastValue { get; private set; }

        public void Invoke(T value)
        {
            Event?.Invoke(value);
            LastValue = value;
        }

        private void OnValidate()
        {
            for (int i = 0; i < Event.GetPersistentEventCount(); i++)
                Event.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
        }
    }
}