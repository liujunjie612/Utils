using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ScreenShot
{
    public class Test : MonoBehaviour
    {
        public Button captureBtn_1;
        public Button captureBtn_2;
        public Button captureBtn_3;

        void Start()
        {
            captureBtn_1.onClick.AddListener(this.__onClick1);
            captureBtn_2.onClick.AddListener(this.__onClick2);
            captureBtn_3.onClick.AddListener(this.__onClick3);
        }

        private void __onClick1()
        {
            ScreenShot.GetInstance().CaptureScreenshot();
        }

        private void __onClick2()
        {
            ScreenShot.GetInstance().CaptureScreenshot(new Rect(200, 120, 200, 200));
        }

        private void __onClick3()
        {
            ScreenShot.GetInstance().CaptureScreenshot(Camera.main, new Rect(100, 100, 300, 300));
        }

    }
}
