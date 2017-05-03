//===== AdvancePikachu 2017 ========
//文件功能描述；相机聚焦
//创建表示；AdvancePikachu 2017/4/20
//========================================================

using UnityEngine;
using System.Collections;

public enum SwitchType
{
    None,
    MoveSpeed,
    MoveImmediately
}

public class CameraViewSwitch : MonoBehaviour
{

    private static CameraViewSwitch instance;

    public static CameraViewSwitch Instacne
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "CameraViewSwitch";
                instance = go.AddComponent<CameraViewSwitch>();
            }
            return instance;
        }
    }

    public float speed = 10f;

    SwitchType switchType;
    bool isBreakSwitch = true;
    Transform startTransform;
    Transform targetTransform;
    Vector3 endPosition;
    Quaternion endQuaternion;
    Vector3 relativeEulers = new Vector3(20, 0, 0);
    Vector3 relativePositon = new Vector3(-50, 30, 0);

    /// <summary>
    /// Gets the start transform.
    /// </summary>
    /// <returns>The start transform.</returns>
    /// <param name="start">Start.</param>
    Transform GetStartTransform(Transform start)
    {
        startTransform = start;
        return startTransform;
    }

    /// <summary>
    /// Gets the target transform.
    /// </summary>
    /// <returns>The target transform.</returns>
    /// <param name="target">Target.</param>
    Transform GetTargetTransform(Transform target)
    {
        targetTransform = target;
        GetEndPosition();
        GetEndQuaternion();
        return targetTransform;
    }

    /// <summary>
    /// Gets the relative position.
    /// </summary>
    /// <returns>The relative position.</returns>
    /// <param name="forword">Forword.</param>
    /// <param name="up">Up.</param>
    /// <param name="right">Right.</param>
    public Vector3 GetRelativePosition(float forword = -50f, float up = 30f, float right = 0f)
    {
        relativePositon = new Vector3(forword, up, right);
        return relativePositon;
    }

    /// <summary>
    /// Gets the relative eulers.
    /// </summary>
    /// <returns>The relative eulers.</returns>
    /// <param name="eulerX">Euler x.</param>
    /// <param name="eulerY">Euler y.</param>
    /// <param name="eulerZ">Euler z.</param>
    public Vector3 GetRelativeEulers(float eulerX = 20f, float eulerY = 0, float eulerZ = 0)
    {
        relativeEulers = new Vector3(eulerX, eulerY, eulerZ);
        return relativeEulers;
    }

    /// <summary>
    /// Gets the end position.
    /// </summary>
    /// <returns>The end position.</returns>
    Vector3 GetEndPosition()
    {
        endPosition = targetTransform.position +
        targetTransform.forward * relativePositon.x +
        targetTransform.up * relativePositon.y +
        targetTransform.right * relativePositon.z;
        return endPosition;
    }

    /// <summary>
    /// Gets the end quaternion.
    /// </summary>
    /// <returns>The end quaternion.</returns>
    Quaternion GetEndQuaternion()
    {
        endQuaternion = Quaternion.Euler(targetTransform.eulerAngles + relativeEulers);
        return endQuaternion;
    }

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <param name="start">Start.</param>
    /// <param name="target">Target.</param>
    /// <param name="i">The index.</param>
    public void GetData(Transform start, Transform target, SwitchType type)
    {
        if (target != null)
            isBreakSwitch = false;
        else
        {
            isBreakSwitch = true;
            return;
        }

        GetStartTransform(start);
        GetTargetTransform(target);
        switchType = type;
        if (type == SwitchType.MoveImmediately)
            ViewChangeImmediately();
    }

    /// <summary>
    /// Views the change.
    /// </summary>
    void ViewChange()
    {
        if (!isBreakSwitch)
        {
            startTransform.position = Vector3.Slerp(startTransform.position, endPosition, Time.deltaTime * speed);
            startTransform.rotation = Quaternion.Slerp(startTransform.rotation, endQuaternion, Time.deltaTime * speed);

            if (Vector3.Distance(startTransform.position, endPosition) <= 0.5f &&
                Quaternion.Angle(startTransform.rotation, endQuaternion) <= 0.5f)
            {
                Debug.Log("Camera Arrived at the specified location!");
                isBreakSwitch = true;
                switchType = SwitchType.None;
            }
        }
        else
            return;
    }

    /// <summary>
    /// Views the change immediately.
    /// </summary>
    void ViewChangeImmediately()
    {
        if (!isBreakSwitch)
        {
            startTransform.position = Vector3.Slerp(startTransform.position, endPosition, Time.time);
            startTransform.rotation = Quaternion.Slerp(startTransform.rotation, endQuaternion, Time.time);

            if (Vector3.Distance(startTransform.position, endPosition) <= 0.5f &&
                Quaternion.Angle(startTransform.rotation, endQuaternion) <= 0.5f)
            {
                Debug.Log("Camera Arrived at the specified location!");
                isBreakSwitch = true;
            }
        }
        else
            return;
    }

    /// <summary>
    /// Breaks the switch.
    /// </summary>
    /// <returns><c>true</c>, if switch was broken, <c>false</c> otherwise.</returns>
    /// <param name="isBreak">If set to <c>true</c> is break.</param>
    public bool BreakSwitch(bool isBreak = true)
    {
        isBreakSwitch = isBreak;
        return isBreakSwitch;
    }

    void Update()
    {
        if (switchType == SwitchType.MoveSpeed)
            ViewChange();
    }
}