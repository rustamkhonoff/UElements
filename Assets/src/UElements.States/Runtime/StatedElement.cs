using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace UElements.States
{
    public interface IStatedElement
    {
        void SetState(State state, bool animate);
    }

    public abstract class StatedElement : Element, IStatedElement
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

        public void SetState(State state, bool animate)
        {
            State = state;

            OnSetState(state, animate);
        }

        public void SetState(bool state) => SetState(state ? State.Active : State.Disabled);
        public void SetState(bool state, bool animate) => SetState(state ? State.Active : State.Disabled, animate);
        public void SetStateNoAnimation(bool state) => SetState(state ? State.Active : State.Disabled, false);
        public void SetState(State state) => SetState(state, true);
        public void SetStateNoAnimation(State state) => SetState(state, false);

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

        public TData Default => _default;

        public TData Active => _active;

        public TData Disabled => _disabled;

        public TData Get(State state)
        {
            return state switch
            {
                State.Disabled => Disabled,
                State.Default => Default,
                State.Active => Active,
                _ => Default
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