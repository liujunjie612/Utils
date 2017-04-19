using UnityEngine;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class WindowDll
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }
}

public class CutHeadIcon : MonoBehaviour
{

    public UnityEngine.UI.Button choice;
    public UnityEngine.UI.Button save;
    public UnityEngine.UI.Button cancel;
    public UnityEngine.UI.Button close;
    public UnityEngine.UI.Button min;
    public UnityEngine.UI.Button leftRound;
    public UnityEngine.UI.Button rightRound;
    public GameObject roundImage;
    public GameObject dragItem;

    public GameObject cutImageBox;
    private ImageBox _imageBox;

    public RectTransform image;
    public RectTransform upImg;
    public RectTransform downImg;
    public RectTransform leftImg;
    public RectTransform rightImg;
    private Vector2 _pos;
    private bool _refreshRectTranform;

    public GameObject loadingImg;

    private Vector2 _leftUpCorner = new Vector2(0, 1);
    private Vector2 _leftDownCorner = new Vector2(0, 0);
    private Vector2 _rightUpCorner = new Vector2(1, 1);
    private Vector2 _rightDownCorner = new Vector2(1, 0);
   
    void Start()
    {
        _imageBox = cutImageBox.GetComponentInChildren<ImageBox>();
        dragItem.SetActive(false);
        upImg.gameObject.SetActive(false);
        downImg.gameObject.SetActive(false);
        leftImg.gameObject.SetActive(false);
        rightImg.gameObject.SetActive(false);
        leftRound.interactable = false;
        rightRound.interactable = false;

        choice.onClick.AddListener(OpenFile);
        save.onClick.AddListener(SaveHead);
        leftRound.onClick.AddListener(LeftRound);
        rightRound.onClick.AddListener(RightRound);
       
    }

    //加载头像图片
    public void OpenFile()
    {

        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "图片文件(*.jpg*.png*.jpeg)\0*.jpg;*.png;*.jpeg";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径

        ofn.title = "选择图片";

        ofn.defExt = "PNG";//显示文件的类型
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (WindowDll.GetOpenFileName(ofn))
        {
            this.loadingImg.SetActive(true);
            StartCoroutine(GetTexture(ofn.file));//加载图片到panle
        }
    }

    void Update()
    {
        if (this._refreshRectTranform)
        {
            this._pos = image.anchoredPosition;
            if (image.pivot == this._leftUpCorner)
            {
                this._pos.x += image.sizeDelta.x / 2;
                this._pos.y -= image.sizeDelta.y / 2;
            }
            else if (image.pivot == this._leftDownCorner)
            {
                this._pos.x += image.sizeDelta.x / 2;
                this._pos.y += image.sizeDelta.y / 2;
            }
            else if (image.pivot == this._rightUpCorner)
            {
                this._pos.x -= image.sizeDelta.x / 2;
                this._pos.y -= image.sizeDelta.y / 2;
            }
            else if (image.pivot == this._rightDownCorner)
            {
                this._pos.x -= image.sizeDelta.x / 2;
                this._pos.y += image.sizeDelta.y / 2;
            }
            upImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200 - this._pos.y - image.sizeDelta.y / 2);
            downImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200 + this._pos.y - image.sizeDelta.y / 2);
            leftImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200 + this._pos.x - image.sizeDelta.x / 2);
            leftImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, image.sizeDelta.y);
            rightImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200 - this._pos.x - image.sizeDelta.x / 2);
            rightImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, image.sizeDelta.y);
            this._pos.x = 0;
            leftImg.anchoredPosition = this._pos;
            rightImg.anchoredPosition = this._pos;
        }
    }


    IEnumerator GetTexture(string url)
    {
        WWW wwwTexture = new WWW("file://" + url);

        yield return wwwTexture;

        string type = url.Substring(url.Length - 4, 4);
        type = type.ToLower();
        if (type == ".jpg" || type == ".png" || type == "jpeg")
        {
            System.IO.FileInfo f = new System.IO.FileInfo(url);
            if(f.Length / 1024 > 500)
            {
                this.loadingImg.SetActive(false);
                Debug.LogError("图片太大啦！请上传小于500K的图片");
            }
            else
            {
                Texture2D t = wwwTexture.texture;
                _imageBox.SetTexture(wwwTexture.texture);
                dragItem.SetActive(true);
                upImg.gameObject.SetActive(true);
                downImg.gameObject.SetActive(true);
                leftImg.gameObject.SetActive(true);
                rightImg.gameObject.SetActive(true);
                leftRound.interactable = true;
                rightRound.interactable = true;
                this._refreshRectTranform = true;
                this.loadingImg.SetActive(false);
            }
        }
        else
        {
            this.loadingImg.SetActive(false);
            Debug.LogError("请选用 .jpeg/ .jpg/ .png 格式的图片！");
        }
    }

    private void SaveHead()
    {
        StartCoroutine(IUploadHeadImage());
    }

    //上传头像到服务器 
    IEnumerator IUploadHeadImage()
    {
        Texture2D _head = _imageBox.showImage_100.texture as Texture2D;

        if (_head == null)
        {
            yield break;
        }

        loadingImg.SetActive(true);
        save.interactable = false;

        //if (Model.infoUploadProxy.base64Token == "")
        //{
        //    Alert.Show("", "上传失败", Alert.TypeEnum.OkOnly);
        //    loadingImg.SetActive(false);
        //    save.interactable = true;
        //    yield break;
        //}

       

        byte[] _bs = _head.EncodeToPNG();
        WWWForm _wwwform = new WWWForm();
        Dictionary<string, string> headers = _wwwform.headers;

        //_wwwform.AddField("uuid", Model.playerProxy.player.uuid);
        //_wwwform.AddField("src_type", "fac");
        //_wwwform.AddField("file1", Model.playerProxy.player.uuid + ".png");
        //_wwwform.AddBinaryData("img", _bs);


        _wwwform.AddField("token", "这里是token");
        _wwwform.AddBinaryData("file", _bs);

        WWW _www = new WWW("url", _wwwform);
        yield return _www;
        if (_www.error != null)
        {
            Debug.LogError("上传失败");
            loadingImg.SetActive(false);
            save.interactable = true;
            yield break;
        }
        
        //_head 即是头像
    }

    private void LeftRound()
    {
        _imageBox.LeftRound();
    }

    private void RightRound()
    {
        _imageBox.RightRound();
    }
}

