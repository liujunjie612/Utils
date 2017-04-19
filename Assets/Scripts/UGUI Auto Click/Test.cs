using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UGUIAutoClick
{
    public class Test : MonoBehaviour
    {
        public Button btn_1;
        public Canvas canvas;

        public Slider slider;

        void Start()
        {
            btn_1.onClick.AddListener(this.__onClick);
        }

        
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                ExecuteEvents.Execute<IPointerClickHandler>(btn_1.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                //输出鼠标的UI位置
                Vector2 _pos = Vector2.one;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                            Input.mousePosition, canvas.worldCamera, out _pos);

                PointerEventData date = new PointerEventData(EventSystem.current);
                date.position = _pos;
                ExecuteEvents.Execute<IPointerClickHandler>(slider.gameObject, date, ExecuteEvents.pointerClickHandler);
            }
        }

        private void __onClick()
        {
            Debug.Log("Click!!!");
        }
        

    }
}
