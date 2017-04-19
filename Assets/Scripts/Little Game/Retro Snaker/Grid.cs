using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Grid:MonoBehaviour
{
    public int x, y;
    public Image img;
	
    public void SetImageColor(Color c)
    {
        img.color = c;
    }
}
