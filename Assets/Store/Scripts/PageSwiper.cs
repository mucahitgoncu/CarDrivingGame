using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public float percentThreshold = 0.5f;
    Store store;
    void Start()
    {
        store = transform.GetComponentInParent<Store>();
    }
    public void OnBeginDrag(PointerEventData data)
    {
    }
    public void OnDrag(PointerEventData data)
    {

    }
    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            store.SwipeButtons((int)percentage);
        }
    }
}
