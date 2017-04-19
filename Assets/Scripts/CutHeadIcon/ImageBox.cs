#define FRED_DEBUG

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using System.Windows.Forms;


public class ImageBox : MonoBehaviour {
    private Texture2D _texture;
    private RawImage image;
    private RectTransform boxRectTrans;

    public RawImage showImage_100;
    public RawImage showImage_50;
    public GameObject emptyImg;
    public GameObject emptyImg_100;
    public GameObject emptyImg_50;

    // 最底框大小，图片容器
    public float maxWidth;    
    public float maxHeight;
    public ImageSelectBox imageSelectBox;

    // 拉伸图片使高或宽其中之一适应最底框
    private int finalBoxWidth;
    private int finalBoxHeight;

    // 缩放倍数，(好复杂，我也不知道当时是怎么算的了(^_^)
    private float scale;
    private Texture2D finalTexture;


    void Start()
    {
        image = GetComponent<RawImage>();
        boxRectTrans = GetComponent<RectTransform>();

        emptyImg.SetActive(true);
        emptyImg_100.SetActive(true);
        emptyImg_50.SetActive(true);
        imageSelectBox.onSelectOver = OnSelectConfirm;
        //image.texture = testTexture;
        //SetTexture(testTexture as Texture2D);
    }

    public void SetTexture(Texture2D _tex)
    {
        _texture = _tex;
        emptyImg.SetActive(false);
        emptyImg_100.SetActive(false);
        emptyImg_50.SetActive(false);

        float texWidth = _tex.width;
        float texHeight = _tex.height;

        // 调整图片框大小，适应底框
        float _mulX = maxWidth / texWidth;
        float _tmpHeight = texHeight * _mulX;
        if (_tmpHeight <= maxHeight)
        {
            finalBoxWidth = (int)maxWidth;
            finalBoxHeight = (int)_tmpHeight;
            scale = _mulX;
        }
        else
        {
            float _mulY = maxHeight / texHeight;
            float _tmpWidth = texWidth * _mulY;
            finalBoxWidth = (int)_tmpWidth;
            finalBoxHeight = (int)maxHeight;
            scale = _mulY;
        }

        boxRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalBoxWidth);
        boxRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalBoxHeight);
        image.texture = _tex;

        imageSelectBox.InitImageSelectBox(finalBoxWidth, finalBoxHeight);
    }

    //左转
    public void LeftRound()
    {
        Texture2D _pic = new Texture2D(_texture.height, _texture.width);
        for (int i = 0; i < _texture.width; i++)
        {
            for (int j = 0; j < _texture.height; j++)
            {
                _pic.SetPixel(_texture.height - j, i, _texture.GetPixel(i, j));
            }
        }
        _pic.Apply();
        SetTexture(_pic);
        imageSelectBox.onSelectOver = OnSelectConfirm;
    }

    public void RightRound()
    {
        Texture2D _pic = new Texture2D(_texture.height, _texture.width);
        for (int i = 0; i < _texture.width; i++)
        {
            for (int j = 0; j < _texture.height; j++)
            {
                _pic.SetPixel(j,_texture.width - i, _texture.GetPixel(i, j));
            }
        }
        _pic.Apply();
        SetTexture(_pic);
        imageSelectBox.onSelectOver = OnSelectConfirm;
    }


    void OnSelectConfirm(Vector2 pos, int width)
    {
        Vector2 realPos = ((pos - new Vector2(width / 2, width / 2)) + new Vector2(finalBoxWidth / 2, finalBoxHeight / 2)) / scale;
        int realWidth = (int)(width / scale);
        Texture2D newTexture = ImageCutter.CutTexture(image.texture as Texture2D, realPos,realWidth, realWidth);
        finalTexture = newTexture;
        showImage_100.texture = newTexture;
        showImage_50.texture = newTexture;
    }

    public Texture2D GetTexture()
    {
        return showImage_100.texture as Texture2D;
    }


    //void Test()
    //{
    //    OpenFileDialog ofd = new OpenFileDialog();
    //}

	
}
