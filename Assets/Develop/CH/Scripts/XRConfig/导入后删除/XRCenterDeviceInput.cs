using System;
using TS.Tools;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Pico 设备 按钮的侦测 所有按钮状态 数值 都可通过此脚本获取
/// </summary>
public class XRCenterDeviceInput : MonoBehaviour
{
    public Action action_ServiceStartSuccess;
    public Action<string> action_ControllerStateChanged;
    public Action<string> action_ControllerStatusChange;


    public static XRCenterDeviceInput Instance;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 左手  按钮状态
    /// </summary>
    public XRHandEvent handLeft;

    /// <summary>
    /// 右手  按钮状态
    /// </summary>
    public XRHandEvent handRight;


    #region 头部-------------------------------------------------------------------------------

    private Vector3 headStatePosition;
    Vector3 oldHeadStatePosition;

    public Vector3 HeadStatePosition
    {
        get => headStatePosition;
        set
        {
            headStatePosition = value;
            EventHandler_HeadPositionChanged?.Invoke(this, new XREventArgs() { ValueVec3 = value });
        }
    }

    public event EventHandler<XREventArgs> EventHandler_HeadPositionChanged;

    #endregion



    void Update()
    {
        //头部
        oldHeadStatePosition = headStatePosition;
        InputDevices.GetDeviceAtXRNode(XRNode.Head)
            .TryGetFeatureValue(CommonUsages.devicePosition, out headStatePosition);
        if (oldHeadStatePosition != headStatePosition)
        {
            HeadStatePosition = headStatePosition;
        }
    }
}