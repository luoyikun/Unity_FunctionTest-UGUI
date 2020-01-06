﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class test : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public UIDir _scrollDir;
    public Image _itemPrefab;
    public int _itemSpacing;//间隔
    public Vector3 _centerScale = Vector3.one * 1.2f;//中心放大

    private ScrollRect _scrollRect;
    private int _itemCount;
    private Image[] _items;
    private int _centerIndex;

    void Start()
    {
        InitLayout();
        InitItem(5);
        SetNearstItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        CheckNearstIndex();
        SetNearstItem();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_scrollDir == UIDir.Horizontal)
        {
            _scrollRect.horizontalNormalizedPosition = (float)_centerIndex / (_itemCount - 1);
        }
        else
        {
            _scrollRect.verticalNormalizedPosition = (float)(_itemCount - 1 - _centerIndex) / (_itemCount - 1);
        }
        //centerIndex显示回调 
    }

    public void InitItem(int count)
    {
        _itemCount = count;
        _items = new Image[count];
        for (int i = 0; i < count; i++)
        {
            _items[i] = Instantiate(_itemPrefab, _scrollRect.content);
            _items[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
        }
    }

    void InitLayout()
    {
        _scrollRect = GetComponent<ScrollRect>();
        HorizontalOrVerticalLayoutGroup layout;
        if (_scrollDir == UIDir.Horizontal)
        {
            layout = _scrollRect.content.gameObject.AddComponent<HorizontalLayoutGroup>();
            float margin = (_scrollRect.viewport.rect.width - _itemPrefab.rectTransform.rect.width) / 2;
            layout.padding.left = layout.padding.right = (int)margin;
            _scrollRect.content.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
            layout = _scrollRect.content.gameObject.AddComponent<VerticalLayoutGroup>();
            float margin = (_scrollRect.viewport.rect.height - _itemPrefab.rectTransform.rect.height) / 2;
            layout.padding.top = layout.padding.bottom = (int)margin;
            _scrollRect.content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.spacing = _itemSpacing;
        layout.childAlignment = TextAnchor.MiddleCenter;
    }

    void CheckNearstIndex()
    {
        float pos = _scrollDir == UIDir.Horizontal ? _scrollRect.horizontalNormalizedPosition : _scrollRect.verticalNormalizedPosition;
        float nearstDis = Mathf.Abs(0 - pos);
        int index = 0;
        for (int i = 1; i < _itemCount; i++)
        {
            float tempPos = (float)i / (_itemCount - 1);
            float tempDis = Mathf.Abs(tempPos - pos);
            if (tempDis < nearstDis)
            {
                nearstDis = tempDis;
                index = i;
            }
        }
        _centerIndex = _scrollDir == UIDir.Horizontal ? index : _itemCount - 1 - index;
    }

    void SetNearstItem()
    {
        Debug.Log(_centerIndex);
        for (int i = 0; i < _itemCount; i++)
        {
            if (_centerIndex == i)
            {
                _items[i].transform.localScale = _centerScale;
            }
            else
            {
                _items[i].transform.localScale = Vector3.one;
            }
        }
    }

}
