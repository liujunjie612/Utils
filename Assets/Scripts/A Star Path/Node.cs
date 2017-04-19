using UnityEngine;
using System.Collections;

namespace AStarPath
{
    public class Node
    {
        //节点索引
        public int gridX, gridY;

        //节点所在位置
        public Vector3 worldPos;

        //节点是否可以行走
        public bool walkable;

        //评估参数： gConst-到起点的距离  hCost-到终点 的距离
        public int gCost;
        public int hCost;

        //评估和：越小则最优
        public int fConst
        {
            get { return gCost + hCost; }
        }

        //用来寻路结束后反向寻找路径节点
        public Node parent;

        public Node(bool walkable, Vector3 pos, int x, int y)
        {
            this.walkable = walkable;
            this.worldPos = pos;
            this.gridX = x;
            this.gridY = y;
        }
    }
}
