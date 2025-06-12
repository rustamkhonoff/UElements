using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace UElements.States
{
    public abstract class StatedElement : Element
    {
        [field: SerializeField,
#if ODIN_INSPECTOR
                EnableIf(nameof(PartOfComposition))
#endif
        ] public State State { get; private set; }

        [field: SerializeField,
#if ODIN_INSPECTOR
                PropertyOrder(999)
#endif
        ] public CompositeStatedElement Composite { get; set; }

        private bool m_wasNullComposite;

        public void SetState(State state, bool animate = true)
        {
            State = state;

            OnSetState(state, animate);
        }

        protected abstract void OnSetState(State state, bool animate);

        protected virtual void OnValidate()
        {
            SetState(State, false);

            if (Composite == null) m_wasNullComposite = true;

            if (m_wasNullComposite && Composite != null)
            {
                Composite.TryRegisterStatedElement(this);
                m_wasNullComposite = false;
            }
        }

        public bool PartOfComposition => Composite == null;
    }

    [Serializable]
    public class StatedData<TData>
    {
        [SerializeField] private TData _default, _active, _disabled;

        public TData Get(State state)
        {
            return state switch
            {
                State.Disabled => _disabled,
                State.Default => _default,
                State.Active => _active,
                _ => _default
            };
        }
    }

    [Serializable]
    public enum State
    {
        Disabled = -1,
        Default = 0,
        Active = 2
    }
}