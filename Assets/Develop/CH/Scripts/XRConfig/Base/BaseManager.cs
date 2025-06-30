using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TS.Tools;
using UnityEngine;
using UnityEngine.XR;

public class BaseManager : MonoBehaviour
{
    [Header("XR基础适配")]
    public Transform XROrigin;
    public Transform VRMainCamera;
    public Transform LeftHand;
    public Transform RightHand;
    [Header("TS交互适配")]
    public Transform LeftHandEvent;
    public Transform RightHandEvent;
    public Transform XRRoot;


    public static BaseManager Instance;
    private void Awake()
    {
        Instance = this;
        Init();
    }

    void Init()
    {
        //基础配置
        LeftHandEvent.GetAddComponent<XRHandEvent>();
        RightHandEvent.GetAddComponent<XRHandEvent>();
        LeftHandEvent.GetComponent<XRHandEvent>().deviceType = XRNode.LeftHand;
        RightHandEvent.GetComponent<XRHandEvent>().deviceType = XRNode.RightHand;
        XRRoot.GetAddComponent<XRCenterDeviceInput>();
        XRRoot.GetComponent<XRCenterDeviceInput>().handLeft = LeftHandEvent.GetComponent<XRHandEvent>();
        XRRoot.GetComponent<XRCenterDeviceInput>().handRight = RightHandEvent.GetComponent<XRHandEvent>();
    }

    void Start()
    {
        //VRMainCamera = LC_PCVR_Manager.instance.MainCamera_VR.transform;
    }
}
