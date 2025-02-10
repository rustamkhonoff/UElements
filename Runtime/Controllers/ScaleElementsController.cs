using System;
using DG.Tweening;
using UnityEngine;

namespace UElements
{
    public class ScaleElementsController : ElementsControllerBase
    {
        public override void Show(Action callback)
        {
            ElementBase.transform.DOScale(Vector3.one, 0.25f).ChangeStartValue(Vector3.zero).OnComplete(() => callback?.Invoke());
        }

        public override void Hide(Action callback)
        {
            ElementBase.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => callback?.Invoke());
        }

        public override void Dispose()
        {
            ElementBase.transform.DOKill();
        }
    }
}