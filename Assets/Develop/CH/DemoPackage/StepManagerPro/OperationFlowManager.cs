using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 流程管理器
/// </summary>
public class OperationFlowManager : MonoBehaviour
{
   

    /// <summary>
    /// 流程交互锁
    /// </summary>
    private bool processInteractionLock = true;

    public bool ProcessInteractionLock
    {
        get => processInteractionLock;
        set => processInteractionLock = value;
    }
    /// <summary>
    /// 流程字典
    /// </summary>
    private readonly Dictionary<string, List<Action>> flowDic = new Dictionary<string, List<Action>>(); // 存储流程的字典

    /// <summary>
    /// 当前步骤索引
    /// </summary>
    private int currentStepIndex;

    /// <summary>
    /// 当前流程名字
    /// </summary>
    private string currentFlowName;

    /// <summary>
    /// 单例
    /// </summary>
    public static OperationFlowManager Instance;
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 获取当前流程名称
    /// </summary>
    public string GetCurrentFlowName()
    {
        return currentFlowName;
    }
    /// <summary>
    /// 获取当前流程索引
    /// </summary>
    public int GetCurrentStepIndex()
    {
        return currentStepIndex;
    }

    /// <summary>
    /// 启动指定流程
    /// </summary>
    /// <param name="flowName"> 流程名字 </param>
    public void StartFlow(string flowName)
    {
        currentFlowName = flowName;
        if (flowDic.TryGetValue(flowName, out List<Action> steps))
        {
            currentStepIndex = 0;
            ExecuteCurrentStep(steps);
        }
        else
        {
            LogInvalidFlowNameError();
        }
    }

    /// <summary>
    /// 执行当前步骤
    /// </summary>
    /// <param name="flow"> 流程list </param>
    private void ExecuteCurrentStep(List<Action> steps)
    {
        if (steps != null && currentStepIndex < steps.Count)
        {
            Debug.Log($"当前流程：{currentFlowName}，当前步骤索引：{currentStepIndex}，当前步骤名称：{steps[currentStepIndex]?.Method.Name}");
            steps[currentStepIndex]?.Invoke();
        }
    }

    /// <summary>
    /// 执行指定步骤
    /// </summary>
    /// <param name="flow"> 流程list </param>
    /// <param name="index"> 步骤index </param>
    private void ExecuteCurrentStep(List<Action> steps, int index)
    {
        if (steps != null && currentStepIndex < steps.Count)
        {
            Debug.Log($"当前流程：{currentFlowName}，当前步骤索引：{currentStepIndex}，当前步骤名称：{steps[currentStepIndex]?.Method.Name}");
            steps[index]?.Invoke();
        }
    }

    /// <summary>
    /// 进入下一步
    /// </summary>
    /// <param name="flowName"></param>
    public void MoveToNextStep()
    {
        if (flowDic.TryGetValue(currentFlowName, out List<Action> steps))
        {
            currentStepIndex++;
            ExecuteCurrentStep(steps);
        }
        else
        {
            LogInvalidFlowNameError();
        }
    }
    /// <summary>
    /// 跳转到指定的步骤
    /// </summary>
    public void MoveToNameStep(Action item)
    {
        if (flowDic.TryGetValue(currentFlowName, out List<Action> steps))
        {
            currentStepIndex = steps.IndexOf(item) - 1;
            MoveToNextStep();
        }
    }

    /// <summary>
    /// 添加流程
    /// </summary>
    /// <param name="flowName">流程key</param>
    /// <param name="flow">流程list</param>
    public void AddFlow(string flowName, List<Action> steps)
    {
        if (!flowDic.ContainsKey(flowName))
        {
            flowDic.Add(flowName, steps);
        }
        else
        {
            Debug.LogError("已存在相同名称的流程!");
        }
    }
    /// <summary>
    /// 添加步骤
    /// </summary>
    public void AddStep(int index, Action ac)
    {
        flowDic.TryGetValue(currentFlowName, out List<Action> steps);
        if (steps != null) steps.Insert(index, ac);
    }
    /// <summary>
    /// 错误输出
    /// </summary>
    private void LogInvalidFlowNameError()
    {
        Debug.LogError("无效流程名称!");
    }
}