using UnityEngine;
using System.Collections;

public class ClipObj : MonoBehaviour
{
    public Transform clipObj;//切片儿的大菜刀
    public Material mat;//超市打折的土豆
    void Update()
    {
        Vector3 cpos = clipObj.position;
        Vector4 cnor = clipObj.up;
        mat.SetVector("cPos", new Vector4(cpos.x, cpos.y, cpos.z, 1));//第四个元素为1,而不是0，可确保平移
        mat.SetVector("cNormal", new Vector4(cnor.x, cnor.y, cnor.z, 0));
    }
}