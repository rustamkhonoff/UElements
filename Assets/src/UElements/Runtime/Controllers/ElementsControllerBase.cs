using System;
using Cysharp.Threading.Tasks;
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
            OnAwake();
        }

        protected virtual void OnAwake() { }

        public abstract UniTask Show(ElementBase element);
        public abstract UniTask Hide(ElementBase element);
        public virtual void Dispose() { }
        protected virtual void OnReset() { }

        private void OnDestroy()
        {
            Dispose();
            Element.ElementController = new DefaultElementController();
        }

        private void Reset()
        {
            Element = GetComponent<ElementBase>();
            OnReset();
        }
    }
}