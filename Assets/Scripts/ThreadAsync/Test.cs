using UnityEngine;
using System.Collections;

namespace ThreadAsync
{
    public class Test : MonoBehaviour
    {
        public GameObject perfab;

        void Start()
        {

        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                clone();
            }

            if(Input.GetKeyDown(KeyCode.B))
            {
                //注意支线程不能调用Unity API函数
                Loom.RunAsync(clone);
            }
        }

       private void clone()
        {
           for(int i = 0;i<10000;i++)
           {
               Debug.Log("yyyyyyyyyyyyy");
           }
        }

    }
}
