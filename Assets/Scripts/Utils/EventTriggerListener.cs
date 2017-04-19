using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler,
IPointerExitHandler, IPointerUpHandler, ISelectHandler, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void VoidDelegate();
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onBeginDrag;

    public VoidDelegate onDrag;
    public VoidDelegate onEndDrag;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp();
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect();
    }
    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag();
    }

    public void OnDrag(PointerEventData data)
    {
        if (onDrag != null) onDrag();
    }

}
