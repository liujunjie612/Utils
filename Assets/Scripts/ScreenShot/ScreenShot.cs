using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace ScreenShot
{
    public class ScreenShot : MonoBehaviour
    {
        public static ScreenShot _instance;

        public static ScreenShot GetInstance()
        {
            if (_instance == null)
                _instance = new ScreenShot();

            return _instance;
        }

        /// <summary>
        /// 截取某一帧的整个游戏的画面，即全屏截图
        /// </summary>
        public void CaptureScreenshot()
        {
            string picName = string.Format("{5}/截图{0}-{1}-{2}_{3}_{4}.png", DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, Application.streamingAssetsPath);
            Application.CaptureScreenshot(picName, 0);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// 截图的区域，左下角为原点
        /// </summary>
        /// <param name="rect">截图的范围</param>
        /// <returns></returns>
        public Texture2D CaptureScreenshot(Rect rect)
        {
            // 先创建一个的空纹理，大小可根据实现需要来设置
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            // 读取屏幕像素信息并存储为纹理数据
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            // 然后将这些纹理数据，成一个png图片文件
            byte[] bytes = screenShot.EncodeToPNG();

            string picName = string.Format("截图{0}-{1}-{2}_{3}_{4}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);

#if UNITY_EDITOR || UNITY_STANDALONE
            string fileName = Application.streamingAssetsPath + "/" + picName;
#elif UNITY_ANDROID
        string fileName = "/sdcard/DICM/Camera/" + picName;
#endif

            File.WriteAllBytes(fileName, bytes);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            return screenShot;
        }


        /// <summary>
        /// 截取某个摄像机的图像
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="rect">截取范围</param>
        /// <returns></returns>
        public Texture2D CaptureScreenshot(Camera camera, Rect rect)
        {
            // 建一个RenderTexture对象  
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 1);
            // 临时设置摄相机的targetTexture为rt, 并手动渲染相关相机  
            camera.targetTexture = rt;
            camera.Render();

            // 激活这个rt, 并从中中读取像素
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            // 注：这个时候，它是从RenderTexture.active中读取像素  
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            // 重置相关参数，以使用camera继续在屏幕上显示  
            camera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rt);

            // 将这些纹理数据，生成一个png图片文件  
            byte[] bytes = screenShot.EncodeToPNG();

            string picName = string.Format("截图{0}-{1}-{2}_{3}_{4}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);

#if UNITY_EDITOR || UNITY_STANDALONE
            string fileName = Application.streamingAssetsPath + "/" + picName;
#elif UNITY_ANDROID
        string fileName = "/sdcard/DICM/Camera/" + picName;
#endif

            File.WriteAllBytes(fileName, bytes);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            return screenShot;
        }
    }
}
