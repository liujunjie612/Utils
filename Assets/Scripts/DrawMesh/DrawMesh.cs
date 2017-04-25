using UnityEngine;
using System.Collections;

public class DrawMesh : MonoBehaviour {

    private MeshFilter mf;
    private Mesh m;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        m = mf.mesh;
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect (10,10,100,100),"Triangle"))
        {
            drawTriangle();
        }

        if (GUI.Button(new Rect(10, 120, 100, 100), "Square"))
        {
            drawSquare();
        }

        if (GUI.Button(new Rect(10, 230, 100, 100), "LingXing"))
        {
            drawLingXing();
        }

        if (GUI.Button(new Rect(10, 350, 100, 100), "Circle"))
        {
            drawCircle(2,20,Vector3.zero);
        }
    }

    /// <summary>
    /// 画三角形
    /// </summary>
    private void drawTriangle()
    {
        m.Clear();
        m.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(3, 0, 0), new Vector3(3, 3, 0) };
        m.triangles = new int[] { 0,1,2 };
    }

    /// <summary>
    /// 画正方形
    /// </summary>
    private void drawSquare()
    {
        m.Clear();
        m.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(3, 0, 0), new Vector3(3, 3, 0), new Vector3(0, 3, 0) };
        m.triangles = new int[] { 0,1,2,0,2,3 };
    }

    /// <summary>
    /// 画菱形
    /// </summary>
    private void drawLingXing()
    {
        m.Clear();
        m.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(3, 0, 0), new Vector3(3, 3, 0), new Vector3(0, -3, 0) };
        m.triangles = new int[] { 0, 1, 2, 0, 1, 3 };
    }

    /// <summary>  
    /// 画圆  
    /// </summary>  
    /// <param name="radius">圆的半径</param>  
    /// <param name="segments">圆的分割数</param>  
    /// <param name="centerCircle">圆心得位置</param>  
    private void drawCircle(float radius, int segments, Vector3 centerCircle)
    {
        //顶点  
        Vector3[] vertices = new Vector3[segments + 1];
        vertices[0] = centerCircle;
        float deltaAngle = Mathf.Deg2Rad * 360f / segments;
        float currentAngle = 0;
        for (int i = 1; i < vertices.Length; i++)
        {
            float cosA = Mathf.Cos(currentAngle);
            float sinA = Mathf.Sin(currentAngle);
            vertices[i] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);
            currentAngle += deltaAngle;
        }

        //三角形  
        int[] triangles = new int[segments * 3];
        for (int i = 0, j = 1; i < segments * 3 - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segments * 3 - 3] = 0;
        triangles[segments * 3 - 2] = 1;
        triangles[segments * 3 - 1] = segments;


        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }  
}
