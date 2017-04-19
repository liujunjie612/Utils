using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollCircle : ScrollRect {

    protected float radius;
    protected float contentDadius;

    void Start()
    {
        //计算摇杆底图的半径
        radius = (transform as RectTransform).sizeDelta.x * 0.5f;
        //计算摇杆的半径
        contentDadius = (this.content.transform as RectTransform).sizeDelta.x * 0.5f;
    }

    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnDrag(eventData);

        var contentPostion = this.content.anchoredPosition;
        if (contentPostion.magnitude > radius - contentDadius)
        {
            contentPostion = contentPostion.normalized * (radius - contentDadius);
            SetContentAnchoredPosition(contentPostion);
        }
    }
}
