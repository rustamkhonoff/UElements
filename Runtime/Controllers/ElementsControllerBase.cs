using System;
using System.Collections.Generic;
using UnityEngine;

namespace UElements
{
    [RequireComponent(typeof(ElementBase))] [DefaultExecutionOrder(-1)]
    public abstract class ElementsControllerBase : MonoBehaviour, IElementController, IDisposable
    {
        protected ElementBase Element { get; private set; }

        private void Awake()
        {
            Element = GetComponent<ElementBase>();
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
    }
}