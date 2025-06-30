using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TS.Tools
{
    public class LogTest : MonoBehaviour
    {
        void Start()
        {
            // 检查当前场景中是否存在 EventSystem
            EventSystem eventSystem = FindObjectOfType<EventSystem>();

            if (eventSystem == null)
            {
                // 创建一个新的 GameObject 并添加 EventSystem 组件
                GameObject eventSystemGameObject = new GameObject("EventSystem");
                eventSystemGameObject.AddComponent<EventSystem>();
                eventSystemGameObject.AddComponent<StandaloneInputModule>();
            }

            InvokeRepeating("RandomEvent", 0, 1f);
        }

        void RandomEvent()
        {
            int index = UnityEngine.Random.Range(0, 100);
            if (0 <= index && index < 20) Debug.Log(index);
            if (20 <= index && index < 40) Debug.LogWarning(index);
            if (40 <= index && index < 60) Debug.LogError(index);
            if (60 <= index && index < 80) Debug.Assert(transform == null, index);
            if (80 <= index && index < 100)
            {
                try
                {
                    GenerateException();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
        void GenerateException()
        {
            // 生成并抛出一个示例异常
            throw new InvalidOperationException("异常");
        }
    }
}