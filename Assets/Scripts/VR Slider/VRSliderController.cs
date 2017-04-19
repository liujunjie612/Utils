using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace VRSliderController
{
    public class VRSliderController : MonoBehaviour
    {

        public Slider slider;
        public RectTransform sliderT;
        public Canvas canvas;

        private float _refreshRule = 0.02f;   //这是用来纠正，防止鼠标有微弱抖动也会导致刷新（显然这对凝视是有交互障碍的，所以设置缓冲）
        private bool _enter = false;

        void Start()
        {
            EventTriggerListener.Get(slider.gameObject).onEnter = this.__onEnter;
            EventTriggerListener.Get(slider.gameObject).onExit = this.__onExit;
        }

        void Update()
        {
            if (_enter)
                setSlider();
        }


        private void setSlider()
        {
            //输出鼠标的UI位置
            Vector2 _pos = Vector2.one;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                        Input.mousePosition, canvas.worldCamera, out _pos);

            //这里算出鼠标相对slider的位置比例
            float v = (_pos.x - slider.transform.localPosition.x + sliderT.sizeDelta.x / 2) / sliderT.sizeDelta.x;
            v = Mathf.Clamp01(v);

            //交互缓冲
            if (Mathf.Abs(v - slider.value) > _refreshRule)
                slider.value = v;
        }

        private void __onEnter()
        {
            _enter = true;
        }

        private void __onExit()
        {
            _enter = false;
        }
    }
}
