using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 移动方向，上下左右
/// </summary>
public enum MoveType
{
    up,
    down,
    left,
    right,
}

public enum Level
{
    Easy,
    Normal,
    Hard,
    Super,
}
public class GameManager : MonoBehaviour
{
    //复制对象
    public GameObject prefab;

    //区域的长宽
    public int width, height;

    public Level level = Level.Easy; 


    //贪吃蛇的列表
    private List<Grid> objList = new List<Grid>();
    //空白部分列表
    private List<Grid> emptyList = new List<Grid>();
    //贪吃蛇的下个目标点
    private Grid point;
    //移动方向
    private MoveType moveType = MoveType.up;
    //小方格的宽度
    private int gridSize = 10;
    //贪吃蛇移动速度
    private float speed = 0.125f;

    void Start()
    {
        switch (level)
        {
            case Level.Easy:
                speed = 1f;
                break;
            case Level.Normal:
                speed = 0.5f;
                break;
            case Level.Hard:
                speed = 0.1f;
                break;
            case Level.Super:
                speed = 0.08f;
                break;
            default:
                break;
        }

        Vector2 startPos = Vector2.zero - Vector2.right * width / 2 * gridSize - Vector2.up * height / 2 * gridSize;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(prefab) as GameObject;
                go.transform.SetParent(this.transform);
                go.transform.localPosition = startPos + Vector2.right * i * gridSize + Vector2.up * j * gridSize;
                Grid g = go.GetComponent<Grid>();
                g.x = i;
                g.y = j;

                emptyList.Add(g);
            }
        }

        Grid gg = getGridByXY(0, 0, emptyList);
        emptyList.Remove(gg);
        objList.Add(gg);
        getNextPoint();
        //refreshColor();

        InvokeRepeating("play", speed, speed);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            refreshMoveType(MoveType.up);
        else if (Input.GetKey(KeyCode.DownArrow))
            refreshMoveType(MoveType.down);
        else if (Input.GetKey(KeyCode.LeftArrow))
            refreshMoveType(MoveType.left);
        else if (Input.GetKey(KeyCode.RightArrow))
            refreshMoveType(MoveType.right);
    }

    /// <summary>
    /// 贪吃蛇移动时只能往两边变向
    /// </summary>
    /// <param name="t"></param>
    private void refreshMoveType(MoveType t)
    {
        switch (moveType)
        {
            case MoveType.up:
            case MoveType.down:
                if (t == MoveType.left || t == MoveType.right)
                    moveType = t;
                break;
            case MoveType.left:
            case MoveType.right:
                if (t == MoveType.up || t == MoveType.down)
                    moveType = t;
                break;
            default:
                break;
        }
    }

    private void play()
    {
        if (!checkToAdd())
            move();

        refreshColor();
    }

    /// <summary>
    /// 贪吃蛇移动路线
    /// </summary>
    private void move()
    {
        int x = objList[0].x;
        int y = objList[0].y;
        switch (moveType)
        {
            case MoveType.up:
                y++;
                break;
            case MoveType.down:
                y--;
                break;
            case MoveType.left:
                x--;
                break;
            case MoveType.right:
                x++;
                break;
            default:
                break;
        }

        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("GameOver");
            Time.timeScale = 0;
            return;
        }

        if (getGridByXY(x, y, objList) != null)
        {
            Debug.LogError("GameOver!");
            Time.timeScale = 0;
            return;
        }

        Grid G = getGridByXY(x, y, emptyList);
        emptyList.Remove(G);
        emptyList.Add(objList[objList.Count - 1]);
        objList.Insert(0, G);
        objList[objList.Count - 1].SetImageColor(Color.white);
        objList.RemoveAt(objList.Count - 1);
    }


    /// <summary>
    /// 根据x,y获取列表中的方格信息
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private Grid getGridByXY(int x, int y, List<Grid> list)
    {
        Grid g = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].x == x && list[i].y == y)
            {
                g = list[i];
                break;
            }
        }

        return g;
    }

    /// <summary>
    /// 获取下个方块
    /// </summary>
    private void getNextPoint()
    {
        int num = Random.Range(0, emptyList.Count);
        point = emptyList[num];
        emptyList.RemoveAt(num);
    }

    /// <summary>
    /// 检测贪吃蛇能否吃到方块， 在相邻的4个方块，并且方向要一致才可以吃
    /// </summary>
    /// <returns></returns>
    private bool checkToAdd()
    {
        if ((objList[0].x == point.x - 1 && objList[0].y == point.y && moveType == MoveType.right) ||
            (objList[0].x == point.x + 1 && objList[0].y == point.y && moveType == MoveType.left) ||
            (objList[0].y == point.y - 1 && objList[0].x == point.x && moveType == MoveType.up) ||
            (objList[0].y == point.y + 1 && objList[0].x == point.x && moveType == MoveType.down))
        {
            objList.Insert(0, point);
            getNextPoint();
            return true;
        }

        return false;
    }


    /// <summary>
    /// 刷新显示颜色
    /// </summary>
    /// <param name="add">是否吃了一个方块</param>
    private void refreshColor()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetImageColor(Color.red);
        }

        //for (int i = 0; i < emptyList.Count; i++)
        //{
        //    emptyList[i].SetImageColor(Color.white);
        //}

        if (point != null)
            point.SetImageColor(Color.green);
    }
}
