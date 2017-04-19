using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum enScalePoint
{
    Left,
    Right,
    Up,
    Down,
    LeftUp,
    RightUp,
    LeftDown,
    RightDown,
}

public delegate void OnButtonDownDelegate(enScalePoint point);
public delegate void OnButtonUpDelegate();
public delegate void OnButtonDragDelegate();
public delegate void OnSelectOverDelegate(Vector2 pos, int width);

public class ImageSelectBox : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IDragHandler,IPointerUpHandler,IPointerExitHandler {

    public Image scaleLeft;
    public Image scaleRight;
    public Image scaleUp;
    public Image scaleDown;
    public Image scaleLeftUp;
    public Image scaleLeftDown;
    public Image scaleRightUp;
    public Image scaleRightDown;

    public OnSelectOverDelegate onSelectOver;

    private float boxWidth;
    private float boxHeight;

    private float chooserWidth = 100;
    private float chooserHeight = 100;

    private float minChooserWidth = 100;
    private float maxChooserWidth = 0;

    private Vector2 chooserPos = Vector2.zero;

    private float minChooserPosX = 0;
    private float maxChooserPosX = 0;
    private float minChooserPosY = 0;
    private float maxChooserPosY = 0;

    private Vector2 originChooserPos;
    private Vector2 originMousePos;
    private Vector2 currentMousePos;

    private float originWidth;

    // 选择框最小宽度
    private float minWidth = 100;

    
    private float maxWidth = 0;

    float upBorder;
    float downBorder;
    float leftBorder;
    float rightBorder;

    private int scaleH = 1;
    private int scaleV = 1;

    private int scaleDir = 1;

    private bool canMove = false;
    private bool canScale = false;

    private RectTransform rectTrans;

    private enScalePoint currScalePoint;

    Vector2 prevChangePivot;

    void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        
        InitScalePoint();
    }


    public void InitImageSelectBox(int _boxWidth, int _boxHeight)
    {
        boxWidth = _boxWidth;
        boxHeight = _boxHeight;
        rectTrans.anchoredPosition = Vector2.zero;
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minWidth);


        maxChooserWidth = boxWidth > boxHeight ? boxHeight : boxWidth;

        // calculate the box border width
        upBorder = boxHeight / 2;
        downBorder = -boxHeight / 2;
        leftBorder = -boxWidth / 2;
        rightBorder = boxWidth / 2;

        OnSelectOver();
    }

    // 初始化选择框的8个缩放点
    void InitScalePoint()
    {
        HeadImageScalePoint control;

        
        control = scaleLeft.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleRight.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleDown.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleUp.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleLeftUp.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleLeftDown.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleRightUp.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;

        control = scaleRightDown.GetComponent<HeadImageScalePoint>();
        control.onButtonDown = OnPointerDownWithScale;
        control.onButtonUp = OnPointerUpWithScale;
        control.onButtonDrag = OnPointerDragWithScale;
    }

    


    // 更新选择框的大小
    void UpdateChooserScale()
    {
        Vector2 offset = currentMousePos - originMousePos;
        float realOffset = 0;
        switch (currScalePoint)
        {
            case enScalePoint.Left:
            case enScalePoint.Right:
            case enScalePoint.LeftUp:
            case enScalePoint.LeftDown:
            case enScalePoint.RightUp:
            case enScalePoint.RightDown:
                {
                    realOffset = offset.x;
                }
                break;
            case enScalePoint.Down:
            case enScalePoint.Up:
                {
                    realOffset = offset.y;
                }
                break;
        }
        float targetWidth = originWidth + realOffset * scaleDir;
        targetWidth = Mathf.Clamp(targetWidth, this.minWidth, this.maxWidth);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetWidth);
    }


    // 更新选择框的位置
    void UpdateChooserPosition()
    {
        Vector2 offset = currentMousePos - originMousePos;
        Vector2 targetPos = originChooserPos + offset;
        targetPos = new Vector2(Mathf.Clamp(targetPos.x, this.minChooserPosX, this.maxChooserPosX),Mathf.Clamp(targetPos.y,this.minChooserPosY,this.maxChooserPosY));
        rectTrans.anchoredPosition = targetPos;
    }


    // 根据当前选择框大小，计算活动范围
    void UpdateLimitedPosition()
    {
        float chooserWidth = rectTrans.rect.width;
        float halfChooserWidth = chooserWidth / 2;
        float minBoxX = -boxWidth / 2;
        float maxBoxX = boxWidth / 2;
        float minBoxY = -boxHeight / 2;
        float maxBoxY = boxHeight / 2;

        this.minChooserPosX = minBoxX + halfChooserWidth;
        this.maxChooserPosX = maxBoxX - halfChooserWidth;
        this.minChooserPosY = minBoxY + halfChooserWidth;
        this.maxChooserPosY = maxBoxY - halfChooserWidth;
    }

    // 计算选择框最当前方向缩放的最大的宽度
    void UpdateLimitedScale()
    {
        switch (currScalePoint)
        {
            case enScalePoint.Left:
                {
                    float upFreeSpace = upBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.y);
                    float leftFreeSpace = Mathf.Abs(leftBorder - (rectTrans.anchoredPosition.x - rectTrans.rect.width / 2));
                    this.maxWidth = rectTrans.rect.width + (upFreeSpace > leftFreeSpace ? leftFreeSpace : upFreeSpace);
                    rectTrans.pivot = new Vector2(1, 0);
                    scaleDir = -1;
                }
                break;
            case enScalePoint.Right:
                {
                    float downFreeSpace = Mathf.Abs(downBorder - (rectTrans.anchoredPosition.y - rectTrans.rect.width / 2));
                    float rightFreeSpace = rightBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.x);
                    this.maxWidth = rectTrans.rect.width + (downFreeSpace > rightFreeSpace ? rightFreeSpace : downFreeSpace);
                    rectTrans.pivot = new Vector2(0, 1);
                }
                break;
            case enScalePoint.Up:
                {
                    float upFreeSpace = upBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.y);
                    float leftFreeSpace = Mathf.Abs(leftBorder - (rectTrans.anchoredPosition.x - rectTrans.rect.width / 2));
                    this.maxWidth = rectTrans.rect.width + (upFreeSpace > leftFreeSpace ? leftFreeSpace : upFreeSpace);
                    rectTrans.pivot = new Vector2(1, 0);
                }
                break;
            case enScalePoint.Down:
                {
                    float downFreeSpace = Mathf.Abs(downBorder - (rectTrans.anchoredPosition.y - rectTrans.rect.width / 2));
                    float rightFreeSpace = rightBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.x);
                    this.maxWidth = rectTrans.rect.width + (downFreeSpace > rightFreeSpace ? rightFreeSpace : downFreeSpace);
                    rectTrans.pivot = new Vector2(0, 1);
                    scaleDir = -1;
                }
                break;
            case enScalePoint.LeftUp:
                {
                    float upFreeSpace = upBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.y);
                    float leftFreeSpace = Mathf.Abs(leftBorder - (rectTrans.anchoredPosition.x - rectTrans.rect.width / 2));
                    this.maxWidth = rectTrans.rect.width + (upFreeSpace > leftFreeSpace ? leftFreeSpace : upFreeSpace);
                    rectTrans.pivot = new Vector2(1, 0);
                    scaleDir = -1;
                }
                break;
            case enScalePoint.RightUp:
                {
                    float rightFreeSpace = rightBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.x);
                    float upFreeSpace = upBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.y);
                    this.maxWidth = rectTrans.rect.width + (rightFreeSpace > upFreeSpace ? upFreeSpace : rightFreeSpace);
                    rectTrans.pivot = new Vector2(0, 0);
                }
                break;
            case enScalePoint.LeftDown:
                {
                    float leftFreeSpace = Mathf.Abs(leftBorder - (rectTrans.anchoredPosition.x - rectTrans.rect.width / 2));
                    float downFreeSpace = Mathf.Abs(downBorder - (rectTrans.anchoredPosition.y - rectTrans.rect.width / 2));
                    this.maxWidth = rectTrans.rect.width + (leftFreeSpace > downFreeSpace ? downFreeSpace : leftFreeSpace);
                    rectTrans.pivot = new Vector2(1,1);
                    scaleDir = -1;
                }
                break;
            case enScalePoint.RightDown:
                {
                    float downFreeSpace = Mathf.Abs(downBorder - (rectTrans.anchoredPosition.y - rectTrans.rect.width / 2));
                    float rightFreeSpace = rightBorder - (rectTrans.rect.width / 2 + rectTrans.anchoredPosition.x);
                    this.maxWidth = rectTrans.rect.width + (downFreeSpace > rightFreeSpace ? rightFreeSpace : downFreeSpace);
                    rectTrans.pivot = new Vector2(0,1);
                }
                break;

        }
    }


    // 缩放完后，清理缩放时设置的属性
    public void ClearScaleEnv()
    {
        rectTrans.pivot = new Vector2(0.5f, 0.5f);
        scaleDir = 1;
        CorrectScalePosition(rectTrans.pivot);
    }

    // 缩放时改变Pivot时Position会自动改变，这里是校对到正确的位置
    public void CorrectScalePosition(Vector2 pivotChange)
    {
        if (pivotChange.x == 0.5f && pivotChange.y == 0.5f)
        {
            float xOffset = prevChangePivot.x == 0 ? rectTrans.rect.width / 2 : prevChangePivot.x == 1 ? -rectTrans.rect.width / 2 : 0;
            float yOffset = prevChangePivot.y == 0 ? rectTrans.rect.width / 2 : prevChangePivot.y == 1 ? -rectTrans.rect.width / 2 : 0;
            rectTrans.anchoredPosition += new Vector2(xOffset, yOffset);
        }
        else
        {
            float xOffset = pivotChange.x == 0 ? -rectTrans.rect.width / 2 : pivotChange.x == 1 ? rectTrans.rect.width / 2 : 0;
            float yOffset = pivotChange.y == 0 ? -rectTrans.rect.width / 2 : pivotChange.y == 1 ? rectTrans.rect.width / 2 : 0;
            rectTrans.anchoredPosition += new Vector2(xOffset, yOffset);
            prevChangePivot = pivotChange;
        }
    }
   
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }


    // 拖动动作鼠标按下设置的环境
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // public
        originMousePos = Input.mousePosition;
        currentMousePos = originMousePos;

        canMove = true;

        // about move
        originChooserPos = rectTrans.anchoredPosition;
        UpdateLimitedPosition();
    }


    // 缩放时鼠标按下设置的环境
    public void OnPointerDownWithScale(enScalePoint clickPoint)
    {

        originMousePos = Input.mousePosition;
        currentMousePos = originMousePos;

        canScale = true;
        // about scale
        currScalePoint = clickPoint;
        originWidth = rectTrans.rect.width;
        UpdateLimitedScale();
        CorrectScalePosition(rectTrans.pivot);
    }


    // 缩放时鼠标弹起设置的环境
    public void OnPointerUpWithScale()
    {
        ClearScaleEnv();
        OnSelectOver();
    }

    // 缩放时鼠标拖动执行的操作
    public void OnPointerDragWithScale()
    {
        currentMousePos = Input.mousePosition;
        if (canScale)
        {
            UpdateChooserScale();
        }
    }

    // 拖动时鼠标弹起设置的环境
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        canMove = false;
        OnSelectOver();
    }


    // 拖动时执行的操作
    public virtual void OnDrag(PointerEventData eventData)
    {
        currentMousePos = Input.mousePosition;

        if (canMove)
        {
            UpdateChooserPosition(); 
        }
    }

    void OnSelectOver()
    {
        if (onSelectOver != null)
        {
            onSelectOver(rectTrans.anchoredPosition,(int)rectTrans.rect.width);
        }
    }
	
}
