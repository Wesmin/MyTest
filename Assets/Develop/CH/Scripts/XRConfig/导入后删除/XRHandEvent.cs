using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class XRHandEvent : MonoBehaviour
{
    public XRNode deviceType;

    /// <summary>
    ///  摇杆 是否按压下去
    /// </summary>
    private bool buttonState_TouchPadClick = false;

    bool old_buttonState_TouchPadClick;

    public bool ButtonState_TouchPadClick
    {
        get => buttonState_TouchPadClick;
        set
        {
            buttonState_TouchPadClick = value;
            if (buttonState_TouchPadClick)
            {
                EventHandlerTouchPadClick?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
            else
            {
                EventHandlerTouchPadRelease?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
        }
    }

    /// <summary>
    /// 事件  TouchPad Click 
    /// </summary>
    public event EventHandler<XREventArgs> EventHandlerTouchPadClick;

    /// <summary>
    /// 事件  TouchPad release
    /// </summary>
    public event EventHandler<XREventArgs> EventHandlerTouchPadRelease;


    // <summary>
    ///  摇杆 坐标  摇杆前后 为 Y轴  左右 为 X轴  
    /// </summary>
    private Vector2 buttonState_TouchPad2DAxis = Vector2.zero;

    Vector2 old_BttonState_TouchPad2DAxis;

    public Vector2 ButtonState_TouchPad2DAxis
    {
        get => buttonState_TouchPad2DAxis;
        set
        {
            buttonState_TouchPad2DAxis = value;

            if (buttonState_TouchPad2DAxis != Vector2.zero)
            {
                //Debug.Log(deviceType + "buttonState_TouchPad2DAxis:" + buttonState_TouchPad2DAxis);
                EventHandler_TouchPad2DAxis_Changed?.Invoke(this, new XREventArgs() { ValueVec2 = value });
            }
            else
            {
                //Debug.Log(deviceType + "buttonState_TouchPad2DAxis release");
                EventHandler_TouchPad2DAxis_Release?.Invoke(this, new XREventArgs() { ValueVec2 = value });
            }
        }
    }


    /// <summary>
    /// 事件 TouchPad2DAxisChanged
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_TouchPad2DAxis_Changed;

    /// <summary>
    /// 事件 TouchPad2DAxis release
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_TouchPad2DAxis_Release;


    /// <summary>
    ///  抓取按钮
    /// </summary>
    private bool buttonState_Grip = false;

    bool old_ButtonState_Grip;

    public bool ButtonState_Grip
    {
        get => buttonState_Grip;
        set
        {
            buttonState_Grip = value;
            if (buttonState_Grip)
            {
                EventHandler_Grip_Click?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
            else
            {
                EventHandler_Grip_Release?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
        }
    }


    /// <summary>
    /// 事件 抓取键  按下
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_Grip_Click;

    /// <summary>
    /// 事件 抓取键  松开
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_Grip_Release;


    // <summary>
    ///  抓取按钮 力度值
    /// </summary>
    private float buttonState_GripValue;

    float old_ButtonState_GripValue;

    public float ButtonState_GripValue
    {
        get => buttonState_GripValue;
        set
        {
            buttonState_GripValue = value;

            if (buttonState_GripValue != 0)
            {
                EventHandler_GripValueChanged?.Invoke(this, new XREventArgs() { ValueFloat = value });
                //Debug.Log(deviceType + " grip value:" + buttonState_GripValue);
            }
        }
    }

    /// <summary>
    /// 事件 抓取键 数值 发生变化
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_GripValueChanged;

    /// <summary>
    ///  扳机键
    /// </summary>
    private bool buttonState_Trigger = false;

    bool old_ButtonState_Trigger;

    public bool ButtonState_Trigger
    {
        get => buttonState_Trigger;
        set
        {
            buttonState_Trigger = value;
            if (buttonState_Trigger)
            {
                EventHandler_Trigger_Click?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
            else
            {
                EventHandler_Trigger_Release?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
        }
    }

    /// <summary>
    /// 事件  扳机键 clicked
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_Trigger_Click;

    /// <summary>
    /// 事件  扳机键 release
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_Trigger_Release;


    // <summary>
    ///  扳机键 力度值
    /// </summary>
    private float buttonState_TriggerValue;

    float old_ButtonState_TriggerValue;

    public float ButtonState_TriggerValue
    {
        get => buttonState_TriggerValue;
        set
        {
            buttonState_TriggerValue = value;
            if (buttonState_TriggerValue != 0)
            {
                EventHandler_TriggerValueChanged?.Invoke(this, new XREventArgs() { ValueFloat = value });
            }
        }
    }


    /// <summary>
    /// 事件 扳机键 数值 发生变化
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_TriggerValueChanged;


    // <summary>
    ///   菜单键
    /// </summary>
    public bool buttonState_Menu;

    bool old_ButtonState_Menu;

    public bool ButtonState_Menu
    {
        get => buttonState_Menu;
        set
        {
            buttonState_Menu = value;
            if (buttonState_Menu == true)
            {
                EventHandler_Menu_Click?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
            else
            {
                EventHandler_Menu_Release?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
        }
    }


    /// <summary>
    /// 事件 菜单键 按下
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_Menu_Click;

    /// <summary>
    /// 事件 菜单键 松开
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_Menu_Release;


    // <summary>
    ///   Y\B键
    /// </summary>
    private bool buttonState_PrimaryButton;

    bool old_ButtonState_PrimaryButton;

    public bool ButtonState_PrimaryButton
    {
        get => buttonState_PrimaryButton;
        set
        {
            buttonState_PrimaryButton = value;
            if (buttonState_PrimaryButton)
            {
                EventHandler_PrimaryButton_Click?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
            else
            {
                EventHandler_PrimaryButton_Release?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
        }
    }


    /// <summary>
    /// 事件 Y\B键 按下
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_PrimaryButton_Click;

    /// <summary>
    /// 事件 Y\B键 松开
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_PrimaryButton_Release;


    // <summary>
    ///   X\A键
    /// </summary>
    private bool buttonState_SecondButton;

    bool old_ButtonState_SecondButton = false;

    public bool ButtonState_SecondButton
    {
        get => buttonState_SecondButton;
        set
        {
            buttonState_SecondButton = value;
            if (buttonState_SecondButton)
            {
                EventHandler_SecondButton_Click?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
            else
            {
                EventHandler_SecondButton_Release?.Invoke(this, new XREventArgs() { ValueBool = value });
            }
        }
    }

    /// <summary>
    /// 事件 X\A键 按下
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_SecondButton_Click;

    /// <summary>
    /// 事件 X\A键 松开
    /// </summary>
    public event EventHandler<XREventArgs> EventHandler_SecondButton_Release;


    void StateLinster_Bool(InputFeatureUsage<bool> cu, bool update, bool target)
    {
        update = target;
        InputDevices.GetDeviceAtXRNode(deviceType).TryGetFeatureValue(cu, out update);
        if (update != target)
        {
            target = update;
        }
    }

    void Update()
    {
        // 按钮 侦测
        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.primary2DAxisClick, out old_buttonState_TouchPadClick);
        if (old_buttonState_TouchPadClick != ButtonState_TouchPadClick)
        {
            ButtonState_TouchPadClick = old_buttonState_TouchPadClick;
        }


        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.primary2DAxis, out old_BttonState_TouchPad2DAxis);
        if (old_BttonState_TouchPad2DAxis != ButtonState_TouchPad2DAxis)
        {
            ButtonState_TouchPad2DAxis = old_BttonState_TouchPad2DAxis;
        }


        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.gripButton, out old_ButtonState_Grip);
        if (ButtonState_Grip != old_ButtonState_Grip)
        {
            ButtonState_Grip = old_ButtonState_Grip;
        }


        InputDevices.GetDeviceAtXRNode(deviceType).TryGetFeatureValue(CommonUsages.grip, out old_ButtonState_GripValue);
        if (old_ButtonState_GripValue != ButtonState_GripValue)
        {
            ButtonState_GripValue = old_ButtonState_GripValue;
        }


        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.triggerButton, out old_ButtonState_Trigger);
        if (old_ButtonState_Trigger != ButtonState_Trigger)
        {
            ButtonState_Trigger = old_ButtonState_Trigger;
        }


        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.trigger, out old_ButtonState_TriggerValue);
        if (old_ButtonState_TriggerValue != ButtonState_TriggerValue)
        {
            ButtonState_TriggerValue = old_ButtonState_TriggerValue;
        }


        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.menuButton, out old_ButtonState_Menu);
        if (old_ButtonState_Menu != ButtonState_Menu)
        {
            ButtonState_Menu = old_ButtonState_Menu;
        }


        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.primaryButton, out old_ButtonState_PrimaryButton);
        if (old_ButtonState_PrimaryButton != ButtonState_PrimaryButton)
        {
            ButtonState_PrimaryButton = old_ButtonState_PrimaryButton;
        }

        InputDevices.GetDeviceAtXRNode(deviceType)
            .TryGetFeatureValue(CommonUsages.secondaryButton, out old_ButtonState_SecondButton);
        if (old_ButtonState_SecondButton != ButtonState_SecondButton)
        {
            ButtonState_SecondButton = old_ButtonState_SecondButton;
        }
    }
}