using System;
using UnityEngine;

namespace TS.Tools
{
    public class HandTouch : MonoBehaviour
    {
        bool isTouchRight = false, isTouchLeft = false;
        bool isTakeRight = false, isTakeLeft = false;
        bool isPutRight = false, isPutLeft = false;
        bool isTriggerRight = false, isTriggerLeft = false;

        private void Awake()
        {
            HandEventConfig();
            //DirectionDetection.Instance.target = transform;

            if (HandExtend.IsPC) transform.ChangeLayer(BaseConfig.TouchLayer);
        }
        /// <summary>
        /// 交互事件适配
        /// </summary>
        public void HandEventConfig(bool isAdd = true)
        {
            if (isAdd)
            {
                if (HandExtend.IsRight) XRCenterDeviceInput.Instance.handRight.EventHandler_Trigger_Click += RightTrigger_Click;
                else XRCenterDeviceInput.Instance.handLeft.EventHandler_Trigger_Click += LeftTrigger_Click;
            }
            else
            {
                if (HandExtend.IsRight) XRCenterDeviceInput.Instance.handRight.EventHandler_Trigger_Click -= RightTrigger_Click;
                else XRCenterDeviceInput.Instance.handLeft.EventHandler_Trigger_Click -= LeftTrigger_Click;
            }
        }
        /// <summary>
        /// 右手版机
        /// </summary>
        private void RightTrigger_Click(object sender, XREventArgs e)
        {
            if (isTouchRight == true) { isTouchRight = false; ExecuteEnd(); }
            if (isTakeRight == true) { isTakeRight = false; ExecuteEnd(); }
            //if (isPutRight == true) { isPutRight = false; ExecuteEnd(); }
        }
        /// <summary>
        /// 左手版机
        /// </summary>
        private void LeftTrigger_Click(object sender, XREventArgs e)
        {
            if (isTouchLeft == true) { isTouchLeft = false; ExecuteEnd(); }
            if (isTakeLeft == true) { isTakeLeft = false; ExecuteEnd(); }
            //if (isPutLeft == true) { isPutLeft = false; ExecuteEnd(); }
        }
        /// <summary>
        /// 执行后事件
        /// </summary>
        /// <param name="isActive"></param>
        public void ExecuteEnd()
        {
            HandEventConfig(false);
            HandExtend.HandChange();//切换手
            HandExtend.TouchOBJ.TSHighlightHide();
            HandExtend.TouchOBJ.SetActive(HandExtend.IsActive);
            switch (HandExtend.TouchType)
            {
                case HandType.TakeLeft:
                case HandType.TakeRight:
                    break;

                case HandType.PutRight:
                case HandType.PutLeft:
                    HandExtend.TempOBJ.Show();
                    Destroy(HandExtend.TouchOBJ);
                    break;
            }
            BaseConfig.NextStep();
            Destroy(transform.GetComponent<HandTouch>());
            //强制卸载未使用的资源
            Resources.UnloadUnusedAssets();
            //执行垃圾回收
            System.GC.Collect();
        }
        /// <summary>
        /// 放回 区分左右手
        /// </summary>
        private void HandPut(Transform temp)
        {
            if (isPutRight && temp == HandExtend.RightHand) ExecuteEnd();
            if (isPutLeft && temp == HandExtend.LeftHand) ExecuteEnd();
        }
        /// <summary>
        /// 触碰 区分左右手
        /// </summary>
        private void HandTrigger(Transform temp)
        {
            if (isTriggerRight && temp == HandExtend.RightHand) ExecuteEnd();
            if (isTriggerLeft && temp == HandExtend.LeftHand) ExecuteEnd();
        }
        /// <summary>
        /// 碰撞检测
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            TriggerEvent(other, true);
        }
        private void OnTriggerExit(Collider other)
        {
            TriggerEvent(other, false);
        }
        /// <summary>
        /// Trigger碰撞执行的内容
        /// </summary>
        /// <summary>
        /// Trigger碰撞执行的内容
        /// </summary>
        private void TriggerEvent(Collider other, bool isActive)
        {
            if (other.GetComponent<HandTag>() != null)
            {
                if (HandExtend.IsRight)
                {
                    switch (HandExtend.TouchType)
                    {
                        case HandType.TriggerRight:
                            isTriggerRight = isActive;
                            HandTrigger(other.transform);
                            break;
                        case HandType.TouchRight:
                            isTouchRight = isActive;
                            BaseConfig.ShakeHandle();
                            break;
                        case HandType.TakeRight:
                            isTakeRight = isActive;
                            BaseConfig.ShakeHandle();
                            break;
                        case HandType.PutRight:
                            isPutRight = isActive;
                            HandPut(other.transform);//直接放回，不扣扳机
                            break;
                    }
                }
                else
                {
                    switch (HandExtend.TouchType)
                    {
                        case HandType.TriggerLeft:
                            isTriggerLeft = isActive;
                            HandTrigger(other.transform);
                            break;
                        case HandType.TouchLeft:
                            isTouchLeft = isActive;
                            BaseConfig.ShakeHandle(false);
                            break;
                        case HandType.TakeLeft:
                            isTakeLeft = isActive;
                            BaseConfig.ShakeHandle(false);
                            break;
                        case HandType.PutLeft:
                            isPutLeft = isActive;
                            HandPut(other.transform);//直接放回，不扣扳机
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// 项目适配功能一：物体有标签，拿取时有吸附手，有箭头指引
        /// </summary>
        private void HandTake(Transform hand, bool isActive)
        {
            Action action = () =>
            {
                if (transform.Find("Hand")) transform.Find("Hand").SetActive(isActive);
                if (transform.Find("Arrow")) transform.Find("Arrow").SetActive(!isActive);
                if (transform.Find("Tag")) transform.Find("Tag").SetActive(!isActive);
            };

            if (HandExtend.IsRight == true && hand == HandExtend.RightHand)
            {
                if (isTakeRight) HandExtend.HandHide(0);
                else HandExtend.HandShowRecord();
                action.Invoke();
            }

            if (HandExtend.IsRight == false && hand == HandExtend.LeftHand)
            {
                if (isTakeLeft) HandExtend.HandHide(1);
                else HandExtend.HandShowRecord();
                action.Invoke();
            }

            //以下放置到ExecuteEnd中
            if (transform.Find("Hand")) transform.Find("Hand").SetActive(false);
            if (transform.Find("Arrow")) transform.Find("Arrow").SetActive(false);
            if (transform.Find("Tag")) transform.Find("Tag").SetActive(true);
            //以下放置到Awake中
            if (transform.Find("Arrow")) transform.Find("Arrow").SetActive(true);
        }
    }
}
