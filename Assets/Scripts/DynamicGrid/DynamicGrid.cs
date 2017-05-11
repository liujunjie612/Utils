using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DynamicGridFloader
{
    public class DynamicGrid : MonoBehaviour
    {

        public float cellX;
        public float cellY;
        public float spaceX;
        public float spaceY;
        public int countX;
        public GameObject prefab;

        private ScrollRect _scrollRect;
        private RectTransform _scrollRectRectT;
        private RectTransform _content;
        private int _previousTopIndex = -1;
        private int _pageCountY;
        private bool _firstIni = true;

        private List<AbstractCell> _activeList = new List<AbstractCell>();
        private List<AbstractCell> _catchList = new List<AbstractCell>();
        private object[] _dataList;

        void Awake()
        {
            creat();
        }

        void Update()
        {
            int currentIndex = getTopIndex();
            if (currentIndex != _previousTopIndex)
            {
                refreshData(currentIndex);
            }
        }

        /// <summary>
        /// 创建物体
        /// </summary>
        private void creat()
        {
            _scrollRect = this.GetComponent<ScrollRect>();
            _scrollRectRectT = _scrollRect.GetComponent<RectTransform>();
            _content = _scrollRect.content;

            _pageCountY = Mathf.FloorToInt(_scrollRectRectT.sizeDelta.y / (cellY + spaceY)) + 3;
            Vector2 size = new Vector2(cellX, cellY);
            for (int i = 0; i < _pageCountY; i++)
            {
                for (int j = 0; j < countX; j++)
                {
                    GameObject go = Instantiate(prefab);
                    go.GetComponent<RectTransform>().sizeDelta = size;
                    go.transform.SetParent(_content);
                    go.transform.localScale = Vector3.one;
                    go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    go.SetActive(false);

                    AbstractCell c = go.GetComponent<AbstractCell>();
                    _catchList.Add(c);
                }
            }
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="goUp">是否移到最顶端</param>
        public void SetData(object[] data, bool goUp = true)
        {
            _dataList = data;

            //计算content高度
            float height = (cellY + spaceY) * ((_dataList.Length + countX - 1) / countX) - spaceY;
            float width = (cellX + spaceX) * countX - spaceX;
            Vector2 size = new Vector2(width, height);
            _content.sizeDelta = size;

            if (goUp)
                _content.anchoredPosition = Vector2.zero;

            if (!_firstIni && _dataList.Length < _pageCountY * countX)
                refreshData(0, true);

            _firstIni = false;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="index">刷新数据的其实标识点</param>
        /// <param name="ignore">当重新赋值时，data的长度小于可显示的长度，这是要刷新一下数据，因为索引值不会变</param>
        private void refreshData(int index, bool ignore = false)
        {
            if ((_previousTopIndex == index || index < 0) && !ignore)
                return;

            _previousTopIndex = index;
            catchAllCell();
            for (int i = 0; i < _pageCountY; i++)
            {
                for (int j = 0; j < countX; j++)
                {
                    int num = (index + i) * countX + j;
                    if (num >= _dataList.Length)
                        break;
                    AbstractCell c = activeACell();
                    c.rectTransform.anchoredPosition = getCellPos(index + i, j);
                    c.data = _dataList[num];
                }
            }
        }

        /// <summary>
        /// 获取最上面显示的AbstractCell的索引
        /// </summary>
        /// <returns></returns>
        private int getTopIndex()
        {
            int index = 0;
            float topPos = _content.anchoredPosition.y;
            index = Mathf.FloorToInt(topPos / (cellY + spaceY));
            return index;
        }

        /// <summary>
        /// 获取第N个AbstractCell的位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 getCellPos(int indexY, int indexX)
        {
            Vector2 pos = Vector2.zero;
            pos.y = indexY * (cellY + spaceY) * -1f;
            pos.x = indexX * (cellX + spaceX);
            return pos;
        }

        /// <summary>
        /// 激活一个AbstractCell
        /// </summary>
        /// <returns></returns>
        private AbstractCell activeACell()
        {
            AbstractCell c = null;
            if (_catchList.Count > 0)
            {
                c = _catchList[_catchList.Count - 1];
                _catchList.RemoveAt(_catchList.Count - 1);
                _activeList.Add(c);
            }
            else
            {

            }

            c.gameObject.SetActive(true);
            return c;
        }

        /// <summary>
        /// 缓存一个AbstractCell
        /// </summary>
        /// <param name="c"></param>
        private void catchACell(AbstractCell c)
        {
            _catchList.Add(c);
            _activeList.Remove(c);
            c.gameObject.SetActive(false);
        }

        /// <summary>
        /// 缓存所有AbstractCell
        /// </summary>
        private void catchAllCell()
        {
            for (int i = _activeList.Count - 1; i >= 0; i--)
            {
                catchACell(_activeList[i]);
            }
        }
    }
}
