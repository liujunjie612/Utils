using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStarPath
{
    public class Grid : MonoBehaviour
    {
        //平面的大小
        public Vector2 gridSize;

        //节点集合，会把平面划分成节点分布
        private Node[,] grid;

        //节点的半径
        public float nodeRadius;

        //节点的直径
        private float nodeDiameter;

        //障碍物所在的层级
        public LayerMask whatLayer;

        //根据平面的大小以及节点的半径可以算出平面上一行和一列的节点的个数
        public int gridCntX, gridCntY;

        public Transform player;

        //路径节点数列
        public List<Node> path = new List<Node>();

        void Start()
        {
            //直径可以根据半径得出
            nodeDiameter = nodeRadius * 2;

            //算出平面中节点的行数和列数
            gridCntX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
            gridCntY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

            //初始化节点集合
            grid = new Node[gridCntX, gridCntY];

            creatGrid();
        }

        void OnDrawGizmos()
        {
            //画边框
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

            if (grid == null)
                return;

            //把所有的节点画出来
            foreach (var node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }

            //画出路径
            if (path != null)
            {
                foreach (var node in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }

            //标出玩家的位置
            Node playerNode = GetFromPostion(player.position);
            if (playerNode != null && playerNode.walkable)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(playerNode.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        private void creatGrid()
        {
            //首先计算出起始点位置
            Vector3 startPoint = transform.position - (gridSize.x / 2) * Vector3.right - (gridSize.y / 2) * Vector3.forward;

            for (int i = 0; i < gridCntX; i++)
            {
                for (int j = 0; j < gridCntY; j++)
                {
                    //获取每个节点的位置
                    Vector3 worPos = startPoint + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);

                    //检测该节点是否可以行走
                    bool walkable = !Physics.CheckSphere(worPos, nodeRadius, whatLayer);

                    //初始化每个节点
                    grid[i, j] = new Node(walkable, worPos, i, j);
                }
            }
        }

        /// <summary>
        /// 根据position获取该位置的节点
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Node GetFromPostion(Vector3 pos)
        {
            //因为grid的中心点是原点(0,0,0)，所以(pos.x + gridSize.x / 2)为相对grid的长度
            float percentX = (pos.x + gridSize.x / 2) / gridSize.x;
            float percentY = (pos.z + gridSize.y / 2) / gridSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            //总长度为gridCntX，因为x为索引，范围为0 - gridCntX-1，所以要gridCntX-1
            int x = Mathf.RoundToInt((gridCntX - 1) * percentX);
            int y = Mathf.RoundToInt((gridCntY - 1) * percentY);

            return grid[x, y];
        }

        /// <summary>
        /// 获取某个节点的相邻的节点(即周围8个)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<Node> GetNeibourhood(Node node)
        {
            List<Node> neibourhood = new List<Node>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int tempX = node.gridX + i;
                    int tempY = node.gridY + j;

                    if (tempX < gridCntX && tempX >= 0 && tempY >= 0 && tempY < gridCntY)
                        neibourhood.Add(grid[tempX, tempY]);
                }
            }

            return neibourhood;
        }
    }
}
