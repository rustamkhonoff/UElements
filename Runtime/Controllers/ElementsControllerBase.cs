using System;
using UnityEngine;

namespace UElements
{
    [RequireComponent(typeof(ElementBase))] [DefaultExecutionOrder(-1)]
    public abstract class ElementsControllerBase : MonoBehaviour, IElementController, IDisposable
    {
        [field: SerializeField] protected ElementBase Element { get; private set; }

        private void Awake()
        {
            Element.ElementController = this;
        }

        public abstract void Show(Action callback);
        public abstract void Hide(Action callback);

        public virtual void Dispose() { }

        private void OnDestroy()
        {
            Dispose();
            Element.ElementController = new DefaultElementController();
        }

        protected virtual void OnReset() { }

        private void Reset()
        {
            Element = GetComponent<ElementBase>();
            OnReset();
        }
    }
}