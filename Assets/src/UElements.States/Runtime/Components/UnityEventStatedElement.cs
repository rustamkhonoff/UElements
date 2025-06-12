using UnityEngine;
using UnityEngine.Events;

namespace UElements.States
{
    public class UnityEventStatedElement : StatedElement
    {
        [SerializeField] private StatedData<UnityEvent> _events;

        protected override void OnSetState(State state, bool animate)
        {
            _events.Get(state)?.Invoke();
        }
    }
}