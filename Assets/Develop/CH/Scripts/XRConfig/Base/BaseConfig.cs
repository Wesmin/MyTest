using UnityEngine;

namespace TS.Tools
{
    /// <summary>
    /// 项目适配脚本
    /// </summary>
    public static class BaseConfig
    {
        /// <summary>
        /// 当前流程类型
        /// </summary>
        public static StepType CurrentStepType = StepType.EasyStep;

        /// <summary>
        /// 下一步适配
        /// </summary>
        public static void NextStep()
        {
            //DirectionDetection.Instance.target = null;

            switch (CurrentStepType)
            {
                case StepType.EasyStep:
                    StepManager.NextStep();
                    break;
                case StepType.FlowStep:
                    OperationFlowManager.Instance.MoveToNextStep();
                    break;
                case StepType.CoroutineStep:
                    BaseTool.Instance.ChangeBoolState();
                    break;
            }
        }
        /// <summary>
        /// 手部适配
        /// </summary>
        public static void HnadConfig()
        {
             HandExtend.RightHand = BaseManager.Instance.RightHand.transform;
             HandExtend.LeftHand = BaseManager.Instance.LeftHand.transform;
             HandExtend.DefaultHand = DefaultHand;
             HandExtend.RightHand.GetAddComponent<HandTag>();
             HandExtend.LeftHand.GetAddComponent<HandTag>();
        }
        /// <summary>
        /// 震动适配
        /// </summary>
        public static void ShakeHandle(bool isRight = true)
        {
            if (isRight)
            {
                //Cyan_HandManager.i.ShakeHandle(HandMode.RightHand);
            }
            else
            {
                //Cyan_HandManager.i.ShakeHandle(HandMode.LeftHand);
            }
        }
        /// <summary>
        /// 高亮适配
        /// </summary>
        /// <param name="go"></param>
        public static void TSHighlightShow(this GameObject go)
        {
            go.HighlightShow();
        }
        public static void TSHighlightHide(this GameObject go)
        {
            go.HighlightHide();
        }

        /// <summary>
        /// 交互层级 射线检测或PC端使用
        /// </summary>
        public static string TouchLayer = "Interactable";
        /// <summary>
        /// 交互层级int  射线检测或PC端使用
        /// </summary>
        public static int TouchLayerInt = LayerMask.NameToLayer(TouchLayer);
        /// <summary>
        /// 默认手势
        /// </summary>
        public static string DefaultHand = "DefaultHand";
    }
}
public enum StepType
{
    EasyStep,
    FlowStep,
    CoroutineStep
}
