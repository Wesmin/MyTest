using System;
using UnityEngine;

/// <summary> 
/// 主要记录全局变量 
/// </summary>
public static class LC_PCVR_Center
{
    /// <summary>
    /// 事件 当切换至 PC时
    /// </summary>
    public static Action Event_GameMode_ChangeTo_PC;

    /// <summary>
    /// 事件  当切换至 VR时
    /// </summary>
    public static Action Event_GameMode_ChangeTo_VR;


    /// <summary>
    /// 暂且叫做临时场景名字，临时场景可以变化
    /// </summary>
    public static string CurrentSceneName;

    /// <summary>
    /// 记录全局的操作模式PC 或者 VR
    /// </summary>
    static LC_PCVR_Center_GameMode gameMode = LC_PCVR_Center_GameMode.VR;

    public static LC_PCVR_Center_GameMode GameMode
    {
        get => gameMode;
        set
        {
            gameMode = value;
            switch (gameMode)
            {
                case LC_PCVR_Center_GameMode.VR:
                    if (Event_GameMode_ChangeTo_VR != null)
                    {
                        Debug.Log("执行事件  VR");
                        Event_GameMode_ChangeTo_VR();
                    }

                    break;
                case LC_PCVR_Center_GameMode.PC:
                    if (Event_GameMode_ChangeTo_PC != null)
                    {
                        Debug.Log("执行事件  PC");
                        Event_GameMode_ChangeTo_PC();
                    }

                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 返回到的场景  目录场景
    /// </summary>
    public static string mainScene;
}


/// <summary>
/// LC_PCVR 系统 模式 类型
/// </summary>
public enum LC_PCVR_Center_GameMode
{
    VR,
    PC,
}


/// <summary>
/// VR下 触发条件
/// 主要触发器有2个  对应的三种情况
/// 1 VR下 LC_LeftHandTrigger 即左手
/// 2 VR下 LC_RightHandTrigger 即右手
/// 3 VR下 双手都可触发
/// </summary>
public enum LC_PCVR_Enum_VR_TriggerCondition
{
    /// <summary>
    /// "左手手柄上的碰撞器  所对应的碰撞体名字必须为  LC_LeftHandTrigger"
    /// </summary>
    [Header("左手手柄上的碰撞器  所对应的碰撞体名字必须为  LC_LeftHandTrigger")]
    LC_LeftHandTrigger,

    /// <summary>
    /// 右手手柄上的碰撞器   所对应的碰撞体名字必须为 LC_RightHandTrigger
    /// </summary>
    [Header("右手手柄上的碰撞器   所对应的碰撞体名字必须为 LC_RightHandTrigger")]
    LC_RightHandTrigger,

    /// <summary>
    /// 鼠标或手柄处发射的射线物体碰撞器  左手手柄上的碰撞器    右手手柄上的碰撞器
    /// </summary>
    [Header("鼠标或手柄处发射的射线物体碰撞器  左手手柄上的碰撞器    右手手柄上的碰撞器")]
    LC_BothHand,


    ///// <summary>
    ///// 鼠标或手柄处发射的射线物体碰撞器    左手手柄上的碰撞器
    ///// </summary>
    //[Header("鼠标或手柄处发射的射线物体碰撞器    左手手柄上的碰撞器")]
    //RP_LHP,


    ///// <summary>
    ///// 鼠标或手柄处发射的射线物体碰撞器    右手手柄上的碰撞器
    ///// </summary>
    //[Header("鼠标或手柄处发射的射线物体碰撞器    右手手柄上的碰撞器")]
    //RP_RLP,

    ///// <summary>
    ///// 左手手柄上的碰撞器    右手手柄上的碰撞器"
    ///// </summary>
    //[Header("左手手柄上的碰撞器    右手手柄上的碰撞器")]
    //LHP_RHP,
}