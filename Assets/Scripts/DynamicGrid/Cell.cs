using UnityEngine;
using System.Collections;
using UGUIExt;
using UnityEngine.UI;

public class Cell : AbstractCell {

    public Text txt;

    protected override void paintData()
    {
        string s = data as string;
        txt.text = s;
    }
}
