using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class AbstractCell: MonoBehaviour 
{
    private RectTransform _rectTransform;

    protected object _data;

    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = this.transform as RectTransform;

            return _rectTransform;
        }
    }

    public virtual object data
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;
            SetData(_data);
        }
    }
    protected virtual void SetData(object data)
    {
        
    }
}


