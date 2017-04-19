using UnityEngine;
using System.Collections;

namespace Drag
{
    public class DragTab : MonoBehaviour
    {

        public GameObject dragInterface;   //要拖动的窗口，窗口的Pivot设为左上角

        public Canvas _canvas;
        private Vector2 _posMiss;   //位置偏移量
        private Vector2 _pos;       //鼠标的2D坐标

        private float width;      //窗体的宽度
        private float height;     //窗体的高度

        private int _applicationWidth;  //应用的宽度
        private int _applicationHeight; //应用的高度

        void Start()
        {
            width = GetComponent<RectTransform>().sizeDelta.x;
            height = GetComponent<RectTransform>().sizeDelta.y;

            _applicationWidth = Screen.width;
            _applicationHeight = Screen.height;

            EventTriggerListener.Get(this.gameObject).onDrag = onDragInterface;
            EventTriggerListener.Get(this.gameObject).onDown = onDragDown;
        }



        //鼠标Down时计算鼠标和物体位置的偏移量
        private void onDragDown()
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                Input.mousePosition, _canvas.worldCamera, out pos))
            {

                _posMiss = dragInterface.GetComponent<RectTransform>().anchoredPosition - pos;
            }
        }

        //鼠标拖动时，物体位置变化
        private void onDragInterface()
        {
            Vector2 p = Vector2.zero;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                Input.mousePosition, _canvas.worldCamera, out _pos))
            {

                p = _pos + _posMiss;
            }

            if (p.x > _applicationWidth - width)
            {
                p.x = _applicationWidth - width;

            }
            else if (p.x < 0)
            {
                p.x = 0;
            }
            if (p.y > 0)
            {
                p.y = 0;
            }
            else if (p.y < height - _applicationHeight)
            {
                p.y = height - _applicationHeight;
            }

            dragInterface.GetComponent<RectTransform>().anchoredPosition = p;

        }
    }
}
