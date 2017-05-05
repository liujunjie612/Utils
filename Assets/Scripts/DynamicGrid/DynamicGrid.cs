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
        public float space;
        public GameObject prefab;

        private ScrollRect _scrollRect;
        private RectTransform _scrollRectRectT;
        private RectTransform _content;
        private int _previousTopIndex = -1;
        private int _pageCount;

        private List<Cell> _activeList = new List<Cell>();
        private List<Cell> _catchList = new List<Cell>();
        private List<CellVo> _dataList = new List<CellVo>();

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

            _pageCount = Mathf.FloorToInt(_scrollRectRectT.sizeDelta.y / (cellY + space)) + 3;
            Vector2 size = new Vector2(cellX, cellY);
            for (int i = 0; i < _pageCount; i++)
            {
                GameObject go = Instantiate(prefab);
                go.GetComponent<RectTransform>().sizeDelta = size;
                go.transform.SetParent(_content);
                go.transform.localScale = Vector3.one;
                go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                go.SetActive(false);

                Cell c = go.GetComponent<Cell>();
                _catchList.Add(c);
            }
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="goUp">是否移到最顶端</param>
        public void SetData(List<CellVo> data, bool goUp = true)
        {
            _dataList = data;

            //计算content高度
            float height = (cellY + space) * _dataList.Count - space;
            Vector2 size = _content.sizeDelta;
            size.y = height;
            _content.sizeDelta = size;

            if (goUp)
                _content.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="index">刷新数据的其实标识点</param>
        private void refreshData(int index)
        {
            if (_previousTopIndex == index || index < 0)
                return;

            _previousTopIndex = index;
            catchAllCell();
            for (int i = 0; i < _pageCount; i++)
            {
                if (index + i >= _dataList.Count)
                    break;
                Cell c = activeACell();
                c.GetComponent<RectTransform>().anchoredPosition = getCellPos(index + i);
                c.SetData(_dataList[index + i]);
            }
        }

        /// <summary>
        /// 获取最上面显示的Cell的索引
        /// </summary>
        /// <returns></returns>
        private int getTopIndex()
        {
            int index = 0;
            float topPos = _content.anchoredPosition.y;
            index = Mathf.FloorToInt(topPos / (cellY + space));
            return index;
        }

        /// <summary>
        /// 获取第N个Cell的位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 getCellPos(int index)
        {
            Vector2 pos = Vector2.zero;
            pos.y = index * (cellY + space) * -1f;
            return pos;
        }

        /// <summary>
        /// 激活一个Cell
        /// </summary>
        /// <returns></returns>
        private Cell activeACell()
        {
            Cell c = null;
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
        /// 缓存一个Cell
        /// </summary>
        /// <param name="c"></param>
        private void catchACell(Cell c)
        {
            _catchList.Add(c);
            _activeList.Remove(c);
            c.gameObject.SetActive(false);
        }

        /// <summary>
        /// 缓存所有Cell
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
