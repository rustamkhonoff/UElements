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
        }

        public abstract UniTask Show(ElementBase element, Action callback);
        public abstract UniTask Hide(ElementBase element, Action callback);

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