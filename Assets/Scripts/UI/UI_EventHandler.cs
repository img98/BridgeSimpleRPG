using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null; //콜백 방식을 쓰기위해 Action만듬
    public Action<PointerEventData> OnDragHandler = null;


    //IBeginDragHandler는 삭제했다. 필요하면 추가하자.
    public void OnDrag(PointerEventData eventData) //드래그가 지속되는 동안
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }
}
