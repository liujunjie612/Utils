using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RawImageCircleFloder
{

    public class Test : MonoBehaviour
    {

        public RawImageCircle img;

        void Start()
        {
            EventTriggerListener.Get(img.gameObject).onClick = this.__onClick;
        }

        private void __onClick()
        {
            Debug.Log("Click");
        }
    }
}
