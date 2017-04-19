using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeadImageScalePoint : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler
{
    public enScalePoint point;
    public OnButtonDownDelegate onButtonDown;
    public OnButtonUpDelegate onButtonUp;
    public OnButtonDragDelegate onButtonDrag;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Libs.Utils.Log.Info("Enter: " + point);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Libs.Utils.Log.Info("Exit: " + point);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (onButtonDown != null)
        {
            onButtonDown(point);
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (onButtonUp != null)
        {
            onButtonUp();
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (onButtonDrag != null)
        {
            onButtonDrag();
        }
    }

    

	
}
