using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace TS.Tools
{
    public class TSTool : MonoBehaviour { }
    /// <summary>
    /// TS工具扩展
    /// </summary>
    public static class TSExtension
    {
        #region GameObjectExtension
        /// <summary>
        /// 打开物体
        /// </summary>
        public static void Show(this GameObject go)
        {
            go.SetActive(true);
        }
        /// <summary>
        /// 关闭物体
        /// </summary>
        public static void Hide(this GameObject go)
        {
            go.SetActive(false);
        }
        /// <summary>
        /// 获取组件，不存在则添加
        /// </summary>
        public static T GetAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                component = go.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 判断对象是否在scene中active，
        /// 返回obj.activeInHierarchy
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsActive(this GameObject obj)
        {
            return obj.activeInHierarchy;
        }
        #endregion

        #region TransformExtension
        /// <summary>
        /// 打开物体
        /// </summary>
        public static void Show(this Transform tf)
        {
            tf.gameObject.SetActive(true);
        }
        /// <summary>
        /// 关闭物体
        /// </summary>
        public static void Hide(this Transform tf)
        {
            tf.gameObject.SetActive(false);
        }
        /// <summary>
        /// 打开/关闭物体
        /// </summary>
        public static void SetActive(this Transform tf, bool isActive)
        {
            tf.gameObject.SetActive(isActive);
        }
        /// <summary>
        /// 遍历所有ChildTransform，执行ChildTransform的Action
        /// </summary>
        public static void EachChild(this Transform tf, Action<Transform> action)
        {
            for (int i = tf.childCount - 1; i >= 0; i--) action(tf.GetChild(i));
        }
        /// <summary>
        /// 添加组件
        /// </summary>
        public static T AddComponent<T>(this Transform tf) where T : Component
        {
            return tf.gameObject.AddComponent(typeof(T)) as T;
        }
        /// <summary>
        /// 获取组件，不存在则添加
        /// </summary>
        public static T GetAddComponent<T>(this Transform tf) where T : Component
        {
            T component = tf.GetComponent<T>();
            if (component == null)
            {
                component = tf.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        public static Transform OnButtonClick(this Transform tf, UnityAction callback)
        {
            var button = GetAddComponent<Button>(tf);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { callback.Invoke(); });
            return tf;
        }


        /// <summary>
        /// 改变对象的层级
        /// </summary>
        /// <param name="layerName">层级名称</param>
        public static void ChangeLayer(this Transform tf, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (tf)
            {
                foreach (Transform traform in tf.GetComponentsInChildren<Transform>(true))
                {
                    traform.gameObject.layer = layer;
                }
            }
        }

        /// <summary>
        /// 判断对象是否在scene中active，
        /// 返回tf.gameObject.activeInHierarchy
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static bool IsActive(this Transform tf)
        {
            return tf.gameObject.activeInHierarchy;
        }

        /// <summary>
        /// 设置父物体
        /// </summary>
        /// <param name="tf"></param>
        public static void SetParent(this Transform tf, Transform target)
        {
            tf.parent = target;
            tf.localPosition = Vector3.zero;
            tf.localEulerAngles = Vector3.zero;
            tf.localScale = Vector3.one;
        }

        /// <summary>
        /// 查找子孙物体 name查找的子物体名字
        /// </summary>
        public static Transform FindInAllChild(this Transform temp, string name)
        {
            var list = temp.GetComponentsInChildren<Transform>(true);
            for (var i = 0; i < list.Length; i++)
            {
                var t = list[i];
                if (t.gameObject.name == name)
                {
                    return t;
                }
            }
            return null;
        }

        #endregion

        #region AnimationExtension
        /// <summary>
        /// 正播动画
        /// </summary>
        /// <param name="str">动画名称</param>
        public static void PlayNormal(this Transform tf, string str)
        {
            Animation ani = tf.GetComponent<Animation>();
            ani[str].time = 0;
            ani[str].speed = 1f;
            ani.CrossFade(str);
        }
        /// <summary>
        /// 倒播动画
        /// </summary>
        ///  /// <param name="str">动画名称</param>
        public static void PlayReverse(this Transform tf, string str)
        {
            Animation ani = tf.GetComponent<Animation>();
            ani[str].time = ani[str].clip.length;
            ani[str].speed = -1f;
            ani.CrossFade(str);
        }
        /// <summary>
        /// 停止在某一帧
        /// </summary>
        /// <param name="str">动画名称</param>
        public static void StopFrame(this Transform tf, string str, float timer)
        {
            Animation ani = tf.GetComponent<Animation>();
            ani[str].time = timer;
            ani[str].speed = 0f;
            ani.CrossFade(str);
        }
        #endregion

        #region StringExtension
        /// <summary>
        /// 字符串切割
        /// </summary>
        public static string StringSub(this string str, bool front = true)
        {
            int cut = str.LastIndexOf("_");
            if (cut != -1)
            {
                if (front) str = str.Substring(0, cut);
                else str = str.Substring(cut + 1);
            }
            return str;
        }
        #endregion

        #region TextExtension
        /// <summary>
        /// 文本赋值并刷新文本
        /// </summary>
        public static Text TextRefresh(this Text tx, string content)
        {
            tx.text = content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tx.transform.parent.GetComponent<RectTransform>());
            tx.transform.GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
            return tx;
        }
        /// <summary>
        /// 文本赋值并刷新文本
        /// </summary>
        public static TMP_Text TextRefresh(this TMP_Text tx, string content)
        {
            tx.text = content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tx.transform.parent.GetComponent<RectTransform>());
            tx.transform.GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
            return tx;
        }
        #endregion

        #region RectTransformExtension

        /// <summary>
        /// 设置锚点
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="preset"></param>
        public static void SetAnchorPreset(this RectTransform rt, AnchorPresets preset)
        {
            switch (preset)
            {
                case AnchorPresets.TopLeft:
                    rt.anchorMin = new Vector2(0, 1);
                    rt.anchorMax = new Vector2(0, 1);
                    rt.pivot = new Vector2(0, 1);
                    break;
                case AnchorPresets.TopCenter:
                    rt.anchorMin = new Vector2(0.5f, 1);
                    rt.anchorMax = new Vector2(0.5f, 1);
                    rt.pivot = new Vector2(0.5f, 1);
                    break;
                case AnchorPresets.TopRight:
                    rt.anchorMin = new Vector2(1, 1);
                    rt.anchorMax = new Vector2(1, 1);
                    rt.pivot = new Vector2(1, 1);
                    break;
                case AnchorPresets.MiddleLeft:
                    rt.anchorMin = new Vector2(0, 0.5f);
                    rt.anchorMax = new Vector2(0, 0.5f);
                    rt.pivot = new Vector2(0, 0.5f);
                    break;
                case AnchorPresets.MiddleCenter:
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case AnchorPresets.MiddleRight:
                    rt.anchorMin = new Vector2(1, 0.5f);
                    rt.anchorMax = new Vector2(1, 0.5f);
                    rt.pivot = new Vector2(1, 0.5f);
                    break;
                case AnchorPresets.BottomLeft:
                    rt.anchorMin = new Vector2(0, 0);
                    rt.anchorMax = new Vector2(0, 0);
                    rt.pivot = new Vector2(0, 0);
                    break;
                case AnchorPresets.BottomCenter:
                    rt.anchorMin = new Vector2(0.5f, 0);
                    rt.anchorMax = new Vector2(0.5f, 0);
                    rt.pivot = new Vector2(0.5f, 0);
                    break;
                case AnchorPresets.BottomRight:
                    rt.anchorMin = new Vector2(1, 0);
                    rt.anchorMax = new Vector2(1, 0);
                    rt.pivot = new Vector2(1, 0);
                    break;
            }
        }

        #endregion
    }

    /// <summary>
    /// 锚点类型
    /// </summary>
    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
}
