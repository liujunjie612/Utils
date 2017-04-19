using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UGUIExt;

namespace DynamicGrid_
{
    public class Test : MonoBehaviour
    {
        public DynamicGrid grid;
        private List<string> data = new List<string>();
        private int count = 0;

        void Awake()
        {
            iniData();
        }

        void Start()
        {
            count = data.Count;

            grid.FillItem += (int index, AbstractCell item) =>
            {
                //这里我们可以填写和修改item prefab
                //改变文字，图像等
                //通过索引，我们可以从JSON数组获取数据，例如
                item.data = data[index];
            };

            grid.PullLoad += (DynamicGrid.Direction obj) =>
            {
                //这里我们监听拖拽刷新事件并处理它
                //它可以将数据从服务器加载到JSON对象并附加到列表
                //做到这一点，调用ApplyDataTo函数，其中arg1 =通用项追加后计数，arg2 = count追加，arg3 =追加方向（顶部或底部）
                Debug.Log("Ffff" + obj);
                iniData();
                count += 100;
                grid.ApplyDataTo(count, 100, obj);
            };

            //函数初始化无限滚动
            grid.InitData(count);
        }

        private void iniData()
        {
            for(int i=0;i<100;i++)
            {
                data.Add(i.ToString());
            }
        }
        
    }
}
