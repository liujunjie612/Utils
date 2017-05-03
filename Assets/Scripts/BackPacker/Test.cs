using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BackPackerFloder
{
    public class Test : MonoBehaviour
    {
        public Canvas canvas;

        //位置修正
        private Vector2 _fixedPos;
        private Vector2 _lastPos;

        void Start()
        {
            EventTriggerListener.Get(this.gameObject).onDrag = __onDrag;
            EventTriggerListener.Get(this.gameObject).onBeginDrag = __onBeginDrag;
            EventTriggerListener.Get(this.gameObject).onEndDrag = __onEndDrag;
            EventTriggerListener.Get(this.gameObject).onClick = __onEndDrag;
        }

        private void __onDrag()
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                   Input.mousePosition, canvas.worldCamera, out pos);
            this.transform.localPosition = pos + _fixedPos;
        }

        private void __onBeginDrag()
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                   Input.mousePosition, canvas.worldCamera, out pos);
            _lastPos = this.gameObject.transform.localPosition;
            _fixedPos = _lastPos - pos;
            this.GetComponent<RectTransform>().SetAsLastSibling();
        }

        private void __onEndDrag()
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector3.forward);
            if(hit == null || hit.collider == null)
            {
                this.transform.localPosition = _lastPos;
                return;
            }
            if (hit.collider.gameObject.tag == "Grid")
            {
                this.gameObject.transform.localPosition = hit.collider.gameObject.transform.localPosition;
                hit.collider.gameObject.transform.localPosition = _lastPos;
            }
            else
            {
                //如果不是格子或没有检测到物体，则将物品放回到原来的格子内
                this.transform.localPosition = _lastPos;
            }
        }
    }
}
