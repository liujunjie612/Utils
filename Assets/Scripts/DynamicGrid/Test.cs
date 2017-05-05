using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace DynamicGridFloader
{
    public class Test : MonoBehaviour
    {
        public DynamicGrid grid;
        private List<CellVo> data = new List<CellVo>();
        private int count = 0;

        void Awake()
        {
            iniData();
        }

        void Start()
        {
            grid.SetData(data);
        }

        private void iniData()
        {
            for(int i=0;i<100;i++)
            {
                CellVo v = new CellVo();
                v.id = i.ToString();
                data.Add(v);
            }
        }
        

        void Update()
        {
            if(Input.GetKeyUp(KeyCode.A))
            {
                CellVo v = new CellVo();
                v.id = "ffffff";
                data.Add(v);


                grid.SetData(data, false);
            }
        }
    }
}
