using System;
using System.Collections;
using System.Collections.Generic;
using UElements;
using UnityEngine;

public class InfiniteScrollItemContent<TModel> : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;

    public void SetSize(Vector2 vector2)
    {
        _parent.sizeDelta = vector2;
    }

    public void SetContent(GameObject o)
    {
        o.transform.SetParent(_parent);
    }
}

public class InfiniteScrollView<TModel> : MonoBehaviour
{
    [SerializeField] private ElementRequest _itemRequest;
    [SerializeField] private RectTransform _viewport;

    private Func<int, TModel> m_modelFunc;
    private Vector2 m_itemSize;

    public void Initialize(Func<int, TModel> modelFunc)
    {
        m_modelFunc = modelFunc;

        int x = 0;
    }

    private void Create(TModel model, int index) { }
}