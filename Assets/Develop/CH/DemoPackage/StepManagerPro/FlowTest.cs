using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowTest : MonoBehaviour
{
    /// <summary>
    /// 演示：下一步的按钮
    /// </summary>
    public Button nextBtn;

    /// <summary>
    /// 步骤
    /// </summary>
    private List<Action> steps = new List<Action>();
    /// <summary>
    /// 步骤添加
    /// </summary>
    private void InitializeSteps()
    {
        // 获取所有以"Step"开头并以“private”的私有方法
        System.Reflection.MethodInfo[] methods = GetType()
            .GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        foreach (System.Reflection.MethodInfo method in methods)
        {
            if (method.Name.StartsWith("Step"))
            {
                steps.Add((Action)System.Delegate.CreateDelegate(typeof(Action), this, method));
            }
        }
        // 将步骤列表添加到流程管理器中
        OperationFlowManager.Instance.AddFlow("测试流程", steps);
    }
    private void Start()
    {
        //步骤添加
        InitializeSteps();

        //开始步骤
        OperationFlowManager.Instance.StartFlow("测试流程");

        nextBtn.onClick.AddListener(OperationFlowManager.Instance.MoveToNextStep);
    }

   

    /* =============== 以下为流程步骤，自上而下执行 =============== */
    private void Step_0()
    {
        OperationFlowManager.Instance.MoveToNameStep(Step_51);
    }
    #region 预留50步骤
    private void Step_1()
    {

    }
    private void Step_2()
    {

    }
    private void Step_3()
    {

    }
    private void Step_4()
    {

    }
    private void Step_5()
    {

    }
    private void Step_6()
    {

    }
    private void Step_7()
    {

    }
    private void Step_8()
    {

    }
    private void Step_9()
    {

    }
    private void Step_10()
    {

    }
    private void Step_11()
    {

    }
    private void Step_12()
    {

    }
    private void Step_13()
    {

    }
    private void Step_14()
    {

    }
    private void Step_15()
    {

    }
    private void Step_16()
    {

    }
    private void Step_17()
    {

    }
    private void Step_18()
    {

    }
    private void Step_19()
    {

    }
    private void Step_20()
    {

    }
    private void Step_21()
    {

    }
    private void Step_22()
    {

    }
    private void Step_23()
    {

    }
    private void Step_24()
    {

    }
    private void Step_25()
    {

    }
    private void Step_26()
    {

    }
    private void Step_27()
    {

    }
    private void Step_28()
    {

    }
    private void Step_29()
    {

    }
    private void Step_30()
    {

    }
    private void Step_31()
    {

    }
    private void Step_32()
    {

    }
    private void Step_33()
    {

    }
    private void Step_34()
    {

    }
    private void Step_35()
    {

    }
    private void Step_36()
    {

    }
    private void Step_37()
    {

    }
    private void Step_38()
    {

    }
    private void Step_39()
    {

    }
    private void Step_40()
    {

    }
    private void Step_41()
    {

    }
    private void Step_42()
    {

    }
    private void Step_43()
    {

    }
    private void Step_44()
    {

    }
    private void Step_45()
    {

    }
    private void Step_46()
    {

    }
    private void Step_47()
    {

    }
    private void Step_48()
    {

    }
    private void Step_49()
    {

    }
    private void Step_50()
    {

    }
    #endregion
    private void Step_51()
    {
        
    }
    private void Step_52()
    {
       
    }
    private void Step_53()
    {
        
    }
    private void Step_54()
    {

    }
    private void Step_55()
    {

    }
    private void Step_56()
    {

    }
    private void Step_57()
    {

    }
    private void Step_58()
    {

    }
    private void Step_59()
    {

    }
    private void Step_60()
    {
        Debug.Log("流程结束");
    }
}
