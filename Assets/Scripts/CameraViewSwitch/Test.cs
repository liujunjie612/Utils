using UnityEngine;
using System.Collections;

namespace CameraViewSwitchFloder
{
    public class Test : MonoBehaviour
    {
        public Transform startPos;
        public Transform endPos;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                CameraViewSwitch.Instacne.GetData(startPos, endPos, SwitchType.MoveSpeed);
            }
        }
    }
}
