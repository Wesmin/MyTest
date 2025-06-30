using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 简易流程管理器
/// </summary>
public static class StepManager
{
    /// <summary>
    /// 步骤索引
    /// </summary>
    public static int Index = 1;
    /// <summary>
    /// 步骤事件，用于封装成事件方法
    /// </summary>
    private static List<Action> Step = new List<Action>();
    /// <summary>
    /// 执行下一步骤
    /// </summary>
    public static void NextStep()
    {
        //检查是否包含当前步骤
        if (Index - 1 < Step.Count)
        {
            //执行当前步骤内的事件
            Step[Index - 1].Invoke();
            Debug.Log(string.Format("<color=yellow>{0}</color>", $"执行步骤：{Index}"));
        }
        else
        {
            Debug.Log(string.Format("<color=red>{0}</color>", "超出步骤管理器"));
        }
        Index++;
    }
    /// <summary>
    /// 添加步骤
    /// </summary>
    public static void StepAdd(Action action)
    {
        Step.Add(action);
    }
    /// <summary>
    /// 清除步骤
    /// </summary>
    public static void StepClear()
    {
        Step.Clear();
        Index = 1;
    }
    /// <summary>
    /// 设置步骤：下一步骤将从指定数值步骤开始
    /// </summary>
    public static void StepSet(int index)
    {
        if (index >= 1 && index <= Step.Count)
        {
            Index = index;
            Debug.Log(string.Format("<color=yellow>{0}</color>", $"步骤索引设置为：{Index}"));
        }
        else
        {
            Debug.Log(string.Format("<color=red>{0}</color>", $"无效的步骤索引：{index}"));
        }
    }
}


