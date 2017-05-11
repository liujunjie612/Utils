using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CellItem : AbstractCell {

    public Text txt;

    protected override void SetData(object data)
    {
        CellVo vo = data as CellVo;
        txt.text = vo.id;
    }
}
