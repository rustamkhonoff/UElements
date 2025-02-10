using System;
using UnityEngine;

namespace UElements
{
    [RequireComponent(typeof(ElementBase))] [DefaultExecutionOrder(-1)]
    public abstract class ElementsControllerBase : MonoBehaviour, IElementController, IDisposable
    {
        protected ElementBase ElementBase { get; private set; }

        private void Awake()
        {
            ElementBase = GetComponent<ElementBase>();
            ElementBase.ElementController = this;
        }

        public abstract void Show(Action callback);
        public abstract void Hide(Action callback);

        public virtual void Dispose() { }

        private void OnDestroy()
        {
            Dispose();
            ElementBase.ElementController = new DefaultElementController();
        }
    }
}