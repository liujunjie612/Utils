using UnityEngine;
using System.Collections;

namespace DragRound
{
    public class DragRound : MonoBehaviour
    {

        //旋转速度
        public float speed = 2;

        //目标物体
        public Transform target;

        private bool _bRound = false;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _bRound = true;
            else if (Input.GetMouseButtonUp(0))
                _bRound = false;

            if (_bRound)
            {
                round();
            }
        }

        private void round()
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            target.Rotate(Vector3.down, x * speed, Space.World);
            target.Rotate(Vector3.right, y * speed, Space.World);
        }
    }
}
