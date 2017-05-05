using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cell: MonoBehaviour {

    public Text txt;

    public void SetData(CellVo s)
    {
        txt.text = s.id;
    }
}

public class CellVo
{
    public string id;
}
