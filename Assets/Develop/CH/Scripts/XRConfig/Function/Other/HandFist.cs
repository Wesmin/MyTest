using System.Collections;
using System.Collections.Generic;
using TS.Tools;
using UnityEngine;

public class HandFist : MonoBehaviour
{
    public static HandFist Instance;
    private void Awake()
    {
        Instance = this;
    }


    public void AddFist()
    {
        XRCenterDeviceInput.Instance.handLeft.EventHandler_Trigger_Click += delegate
        {
            //GameManager.instance.LefttHand.transform.Find("DefaultHand/√…∆§").NormalPlay("”“ ÷Œ’»≠");
        };
        XRCenterDeviceInput.Instance.handLeft.EventHandler_Trigger_Release += delegate
        {
            //GameManager.instance.LefttHand.transform.Find("DefaultHand/√…∆§").ReversePlay("”“ ÷Œ’»≠");
        };

        XRCenterDeviceInput.Instance.handRight.EventHandler_Trigger_Click += delegate
        {
            //GameManager.instance.RightHand.transform.Find("DefaultHand/√…∆§").NormalPlay("”“ ÷Œ’»≠");
        };
        XRCenterDeviceInput.Instance.handRight.EventHandler_Trigger_Release += delegate
        {
            //GameManager.instance.RightHand.transform.Find("DefaultHand/√…∆§").ReversePlay("”“ ÷Œ’»≠");
        };
    }
}
