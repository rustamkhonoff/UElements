using System.Collections.Generic;
using UnityEngine;

namespace UElements.States
{
    public class CompositeStatedElement : StatedElement
    {
        [SerializeField] private List<StatedElement> _statedElements;

        protected override void OnSetState(State state, bool animate)
        {
            foreach (StatedElement statedElement in _statedElements)
                statedElement.SetState(state, animate);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            foreach (StatedElement statedElement in _statedElements)
                if (statedElement != null)
                    statedElement.Composite = this;
        }

        public void TryRegisterStatedElement(StatedElement statedElement)
        {
            if (statedElement == null || _statedElements.Contains(statedElement))
                return;
            _statedElements.Add(statedElement);
            statedElement.SetState(State, false);
        }
    }
}