using System;
using UnityEngine;

namespace TS.Tools
{
    public static class HandExtend
    {
        #region 交互变量
        private static Transform leftHand;
        private static Transform rightHand;
        private static string defaultHand;
        private static string activeHand;
        private static bool isRight = true;
        private static bool isActive = false;
        private static HandType touchType;
        private static GameObject touchOBJ;
        private static GameObject tempOBJ;
        private static Transform tempHand;
        private static bool isPC = false;
        private static GameObject triggerOBJ;
        private static bool isDoubleClick = false;
        #endregion

        #region 私有变量封装
        /// <summary>
        /// 父物体：右手
        /// </summary>
        public static Transform RightHand { get => rightHand; set => rightHand = value; }
        /// <summary>
        /// 父物体：左手
        /// </summary>
        public static Transform LeftHand { get => leftHand; set => leftHand = value; }
        /// <summary>
        /// 枚举类：交互的类型
        /// </summary>
        public static HandType TouchType { get => touchType; set => touchType = value; }
        /// <summary>
        /// 布尔值：是否是右手？
        /// </summary>
        public static bool IsRight { get => isRight; set => isRight = value; }
        /// <summary>
        /// 布尔值：是否激活？
        /// </summary>
        public static bool IsActive { get => isActive; set => isActive = value; }
        /// <summary>
        /// 字符串：左右手父物体下，默认名称的子物体手势
        /// </summary>
        public static string DefaultHand { get => defaultHand; set => defaultHand = value; }
        /// <summary>
        /// 字符串：用于激活左右手父物体下，具体名称的子物体手势
        /// </summary>
        public static string ActiveHand { get => activeHand; set => activeHand = value; }
        /// <summary>
        /// 物体：交互的物体
        /// </summary>
        public static GameObject TouchOBJ { get => touchOBJ; set => touchOBJ = value; }
        /// <summary>
        /// 物体：交互时替代用的临时物体。如：执行 HandPut（放置）事件时，生成的虚影交互体
        /// </summary>
        public static GameObject TempOBJ { get => tempOBJ; set => tempOBJ = value; }
        /// <summary>
        /// 物体：记录手势的临时物体。如：隐藏整个右手时，遍历找到当前处于激活的子物体手势，做后处理使用
        /// </summary>
        public static Transform TempHand { get => tempHand; set => tempHand = value; }
        /// <summary>
        /// PC模式
        /// </summary>
        public static bool IsPC { get => isPC; set => isPC = value; }
        /// <summary>
        /// PC模式时采用射线点击，用于记录原本是物物碰撞的物体
        /// </summary>
        public static GameObject TriggerOBJ { get => triggerOBJ; set => triggerOBJ = value; }
        /// <summary>
        /// PC模式双击
        /// </summary>
        public static bool IsDoubleClick { get => isDoubleClick; set => isDoubleClick = value; }
        #endregion

        #region 交互方法
        /// <summary>
        /// 触碰物体，触碰后物体失活（obj：交互的物体；isRight：是否为右手？）
        /// </summary>
        public static void HandTrigger(this GameObject obj, bool isRight = true)
        {
            IsRight = isRight;
            TouchOBJ = obj;
            ExecuteTouch(delegate
            {
                TouchOBJ.TSHighlightShow();
                IsActive = false;
                if (IsRight) TouchType = HandType.TriggerRight;
                else TouchType = HandType.TriggerLeft;
            });
        }
        /// <summary>
        /// 交互物体，交互后物体激活（obj：交互的物体；isRight：是否为右手？）
        /// </summary>
        public static void HandTouch(this GameObject obj, bool isRight = true)
        {
            IsRight = isRight;
            TouchOBJ = obj;
            ExecuteTouch(delegate
            {
                TouchOBJ.TSHighlightShow();
                IsActive = true;
                if (IsRight) TouchType = HandType.TouchRight;
                else TouchType = HandType.TouchLeft;
            });
        }
        /// <summary>
        /// ①拿取物体，拿去后物体失活；
        /// <para>②若不执行拿取，即参数 hand 为空，视为执行交互物体，交互后物体失活。</para>
        /// <para>(obj：交互的物体；hand：子物体手名；isRight：是否为右手？)</para>
        /// </summary>
        public static void HandTake(this GameObject obj, string hand = null, bool isRight = true)
        {
            IsRight = isRight;
            ActiveHand = hand;
            TouchOBJ = obj;
            ExecuteTouch(delegate
            {
                TouchOBJ.TSHighlightShow();
                IsActive = false;
                if (ActiveHand == null)
                {
                    if (IsRight) TouchType = HandType.TouchRight;
                    else TouchType = HandType.TouchLeft;
                }
                else
                {
                    if (IsRight) TouchType = HandType.TakeRight;
                    else TouchType = HandType.TakeLeft;
                }
            });
        }
        /// <summary>
        /// 放置物体，放置后物体激活（obj：交互的物体；hand：子物体手名；isRight：是否为右手？）
        /// </summary>
        public static void HandPut(this GameObject obj, string hand, bool isRight = true)
        {
            IsRight = isRight;
            ActiveHand = hand;
            TouchOBJ = obj;
            TempOBJ = MonoBehaviour.Instantiate(obj, obj.transform.position, obj.transform.rotation, obj.transform.parent);
            TempOBJ.transform.localScale = obj.transform.localScale;
            TempOBJ.Hide();
            TempOBJ.name = $"{obj.name}";
            obj.name = $"{obj.name}透明";
            ExecuteTouch(delegate
            {
                TouchOBJ.TSHighlightShow();
                TouchOBJ.GetAddComponent<MaterialFit>();
                IsActive = false;
                if (IsRight) TouchType = HandType.PutRight;
                else TouchType = HandType.PutLeft;
            });
        }
        #endregion

        #region 交互条件与后处理事件
        /// <summary>
        /// 交互条件：为物体添加 HandTouch 交互的条件
        /// </summary>
        /// <param name="action"></param>
        public static void ExecuteTouch(Action action)
        {
            if (!TouchOBJ.GetComponent<BoxCollider>())
            {
                // MeshColliderFit 存在问题，需完善
                if (!TouchOBJ.GetComponent<MeshColliderFit>())
                {
                    TouchOBJ.AddComponent<MeshColliderFit>();
                    if (!TouchOBJ.GetComponent<HandTouch>())
                    {
                        TouchOBJ.AddComponent<HandTouch>();
                        TouchOBJ.Show();
                        action?.Invoke();
                    }
                    else
                    {
                        Debug.LogError($"{TouchOBJ} has been added <HandTouch> once");
                    }
                }
                else
                {
                    Debug.LogError($"{TouchOBJ} has been added <MeshColliderFit> once");
                }
                
                Debug.LogError($"{TouchOBJ} miss <BoxCollider> ");
            }
            else
            {
                if (!TouchOBJ.GetComponent<HandTouch>())
                {
                    TouchOBJ.AddComponent<HandTouch>();
                    TouchOBJ.Show();
                    action?.Invoke();
                }
                else
                {
                    Debug.LogError($"{TouchOBJ} has been added <HandTouch> once");
                }
            }
        }
        /// <summary>
        /// 后处理事件：交互完成后切换手势
        /// </summary>
        public static void HandChange()
        {
            switch (TouchType)
            {
                case HandType.TriggerRight:
                case HandType.TouchRight:
                case HandType.PutRight:
                    HandShow(DefaultHand, true);
                    BaseConfig.ShakeHandle();
                    break;
                case HandType.TakeRight:
                    HandShow(ActiveHand, true);
                    BaseConfig.ShakeHandle(false);
                    break;


                case HandType.TriggerLeft:
                case HandType.TouchLeft:
                case HandType.PutLeft:
                    HandShow(DefaultHand, false);
                    BaseConfig.ShakeHandle();
                    break;
                case HandType.TakeLeft:
                    HandShow(ActiveHand, false);
                    BaseConfig.ShakeHandle(false);
                    break;
            }
        }
        #endregion

        #region 手势
        /// <summary>
        /// 打开具体子物体手，默认打开“右手”下名为“DefaultHand”的子物体手
        /// </summary>
        public static void HandShow(string str = "DefaultHand", bool isRight = true)
        {
            if (isRight)
            {
                for (int i = 0; i < RightHand.childCount; i++)
                {
                    RightHand.GetChild(i).Hide();
                }
                RightHand.Find(str).Show();
                HandRecord(true); //记录激活的手
            }
            else
            {
                for (int i = 0; i < LeftHand.childCount; i++)
                {
                    LeftHand.GetChild(i).Hide();
                }
                LeftHand.Find(str).Show();
                HandRecord(false);//记录激活的手
            }
        }

        /// <summary>
        /// 关闭所有手势，0右手（默认），1左手，2双手
        /// </summary>
        public static void HandHide(int indexType = 0)
        {
            switch (indexType)
            {
                case 0:
                    HandRecord(true);//记录激活的手
                    for (int i = 0; i < RightHand.childCount; i++) { RightHand.GetChild(i).Hide(); }
                    break;
                case 1:
                    HandRecord(false);//记录激活的手
                    for (int i = 0; i < LeftHand.childCount; i++) { LeftHand.GetChild(i).Hide(); }
                    break;
                case 2:
                    for (int i = 0; i < RightHand.childCount; i++) { RightHand.GetChild(i).Hide(); }
                    for (int i = 0; i < LeftHand.childCount; i++) { LeftHand.GetChild(i).Hide(); }
                    break;
            }
        }

        /// <summary>
        /// 打开默认手势
        /// </summary>
        public static void HandShowDefault()
        {
            HandShow(DefaultHand, true);
            HandShow(DefaultHand, false);
        }
        /// <summary>
        /// 打开记录的手势
        /// </summary>
        public static void HandShowRecord()
        {
            TempHand.Show();
        }

        /// <summary>
        /// 记录当前激活的手势
        /// </summary>
        private static Transform HandRecord(bool isRight = true)
        {
            if (isRight)
            {
                foreach (Transform item in RightHand)
                {
                    if (item.IsActive())
                    {
                        TempHand = item;
                        break;
                    }
                }
            }
            else
            {
                foreach (Transform item in leftHand)
                {
                    if (item.IsActive())
                    {
                        TempHand = item;
                        break;
                    }
                }
            }
            return TempHand;
        }

        /// <summary>
        /// 找到具体名称的手势
        /// </summary>
        public static Transform GetHand(string str, bool isRight = true)
        {
            Transform temp = null;
            if (isRight)
                temp = RightHand.Find(str);
            else
                temp = leftHand.Find(str);
            return temp;
        }

        #endregion

        #region 手中物体扩展
        /// <summary>
        /// 手中物体与物体的触碰方法
        /// </summary>
        public static void AddTriggerTag(this GameObject go)
        {
            DirectionDetection.Instance.target = go.transform;
            go.SetActive(true);
            go.TSHighlightShow();
            go.GetAddComponent<TriggerTag>();
        }
        #endregion
    }
    /// <summary>
    /// Trigger触碰、Touch手柄交互、Take拿物品、Put放物品
    /// </summary>
    public enum HandType
    {
        None,
        TriggerLeft,
        TriggerRight,
        TouchLeft,
        TouchRight,
        TakeLeft,
        TakeRight,
        PutLeft,
        PutRight,
    }
}