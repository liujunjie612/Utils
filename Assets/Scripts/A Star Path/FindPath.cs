using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStarPath
{
    public class FindPath : MonoBehaviour
    {
        //玩家和终点的位置
        public Transform player, endPoint;

        private Grid _grid;

        void Start()
        {
            _grid = GetComponent<Grid>();
        }

        void Update()
        {
            findingPath(player.position, endPoint.position);
        }

        /// <summary>
        /// 寻找路径
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        private void findingPath(Vector3 startPos, Vector3 endPos)
        {
            //获取起点和终点的节点信息
            Node startNode = _grid.GetFromPostion(startPos);
            Node endNode = _grid.GetFromPostion(endPos);

            //建立开启列表和关闭列表，并把起点加到开启列表里面
            List<Node> openList = new List<Node>();
            HashSet<Node> closeList = new HashSet<Node>();
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                //首先获取到开启列表里面最优的节点
                Node currentNode = openList[0];
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].fConst < currentNode.fConst ||
                        openList[i].fConst == currentNode.fConst && openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }

                //然后把该节点加入到关闭列表中
                openList.Remove(currentNode);
                closeList.Add(currentNode);

                //如果当前节点为终点说明寻路完成
                if (currentNode == endNode)
                {
                    generatePath(startNode, endNode);
                    return;
                }

                //刷新该节点附近一圈的节点的估值信息
                foreach (var node in _grid.GetNeibourhood(currentNode))
                {
                    if (!node.walkable || closeList.Contains(node))
                        continue;

                    int newCost = currentNode.gCost + getDistanceNodes(currentNode, node);
                    if (newCost < node.gCost || !openList.Contains(node))
                    {
                        node.gCost = newCost;
                        node.hCost = getDistanceNodes(node, endNode);
                        node.parent = currentNode;

                        if (!openList.Contains(node))
                            openList.Add(node);
                    }
                }
            }
        }


        /// <summary>
        /// 获取两节点之间距离，即估价
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int getDistanceNodes(Node a, Node b)
        {
            int cntX = Mathf.Abs(a.gridX - b.gridX);
            int cntY = Mathf.Abs(a.gridY - b.gridY);

            if (cntX >= cntY)
                return 14 * cntY + 10 * (cntX - cntY);
            else
                return 14 * cntX + 10 * (cntY - cntX);
        }

        /// <summary>
        /// 当寻路完成后反向获取路径节点信息
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private void generatePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node temp = endNode;

            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.parent;
            }

            path.Reverse();
            _grid.path = path;
        }
    }
}
