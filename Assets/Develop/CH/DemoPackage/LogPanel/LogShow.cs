using System;
using UnityEngine;
using UnityEngine.UI;

namespace TS.Tools 
{
    public class LogShow : MonoBehaviour
    {
        #region LOG控制管理
        private int tKeyPressCount = 0;
        private float lastTKeyPressTime = 0f;
        private float timeThreshold = 1.0f; //设置两次按键的时间阈值
        private bool isShow = true;

        private void Start()
        {
            // XR手柄事件注册 右手柄Y键
            XRCenterDeviceInput.Instance.handRight.EventHandler_PrimaryButton_Click += delegate { ExcEvent();};
        }

        void Update()
        {
            // PC模式 用户连续按下对应按键 控制LOG面板
            if (Input.GetKeyDown(KeyCode.T)) ExcEvent();
        }

        void ExcEvent()
        {
            KeyCodeCallBack(delegate
            {
                if (isShow)
                {
                    isShow = false;
                    LogItm.SetActive(true);
                }
                else
                {
                    isShow = true;
                    LogItm.SetActive(false);
                }
            });
        }

        void KeyCodeCallBack(Action action = null)
        {
            //获取当前时间
            float currentTime = Time.time;

            //判断两次T键按下的时间间隔是否小于阈值
            if (currentTime - lastTKeyPressTime < timeThreshold)
            {
                //在这里执行连续按下T键后的逻辑
                tKeyPressCount++;

                //判断是否达到你希望的次数
                if (tKeyPressCount >= 8)
                {
                    tKeyPressCount = 1;
                    action?.Invoke();// 回调
                }
            }
            else
            {
                tKeyPressCount = 1; //重置计数器
            }

            //更新上一次按下T键的时间戳
            lastTKeyPressTime = currentTime;
        }
        #endregion

        #region LOG业务逻辑

        public GameObject LogItm; // Log日志面板
        public ScrollRect scrollRect; 
        public Text consoleText; 
        public Button clearButton;
        private string logOutput = ""; // 存储日志内容


        void OnEnable()
        {
            // 注册日志消息接收事件
            Application.logMessageReceived += HandleLog;
            // 注册清空按钮点击事件
            if (clearButton != null)
            {
                clearButton.onClick.AddListener(ClearConsole);
            }
        }

        void OnDisable()
        {
            // 注销日志消息接收事件
            Application.logMessageReceived -= HandleLog;
            // 注销清空按钮点击事件
            if (clearButton != null)
            {
                clearButton.onClick.RemoveListener(ClearConsole);
            }
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            // 获取当前时间并格式化
            string timeStamp = System.DateTime.Now.ToString("HH:mm:ss");

            // 标记颜色
            string colorTag;
            // 分类处理
            switch (type)
            {
                case LogType.Log:
                    colorTag = FormatTextWithColor($"【!】一般 [{timeStamp}]：", Color.white);
                    break;
                case LogType.Warning:
                    colorTag = FormatTextWithColor($"【!】警告 [{timeStamp}]：", Color.yellow);
                    break;
                case LogType.Error:
                    colorTag = FormatTextWithColor($"【!】错误 [{timeStamp}]：", Color.red);
                    break;
                case LogType.Assert:
                    colorTag = FormatTextWithColor($"【!】断言 [{timeStamp}]：", Color.red);
                    break;
                case LogType.Exception:
                    colorTag = FormatTextWithColor($"【!】异常 [{timeStamp}]：", Color.red);
                    break;
                default:
                    colorTag = FormatTextWithColor($"【!】一般 [{timeStamp}]：", Color.white);
                    break;
            }

            // 格式化堆栈跟踪信息
            string formattedStackTrace = FormatStackTrace(stackTrace);

            // 格式化日志信息，包含日志消息、堆栈跟踪和日志类型
            string logEntry = $"{colorTag}{logString}\n{formattedStackTrace}\n";

            // 追加新的日志内容
            logOutput += logEntry;

            // 更新UI文本对象的内容
            if (consoleText != null)
            {
                consoleText.text = logOutput;
                LayoutRebuilder.ForceRebuildLayoutImmediate(consoleText.transform.parent.GetComponent<RectTransform>());

                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        string FormatTextWithColor(string text, Color color)
        {
            // 修改颜色
            string hexColor = ColorUtility.ToHtmlStringRGB(color);
            return $"<color=#{hexColor}>{text}</color>";
        }

        string FormatStackTrace(string stackTrace)
        {
            string[] lines = stackTrace.Split('\n');
            string formattedStackTrace = "";

            // 格式化堆栈跟踪信息
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string formattedLine = line;

                // 查找文件路径和行号
                int fileIndex = line.IndexOf("(at ");
                if (fileIndex >= 0)
                {
                    int endIndex = line.IndexOf(")", fileIndex);
                    if (endIndex >= 0)
                    {
                        string filePathAndLineNumber = line.Substring(fileIndex + 4, endIndex - fileIndex - 4);
                        string coloredFilePath = FormatTextWithColor(filePathAndLineNumber, Color.blue);
                        formattedLine = line.Substring(0, fileIndex + 4) + coloredFilePath + line.Substring(endIndex);
                    }
                }

                formattedStackTrace += formattedLine + "\n";
            }

            return formattedStackTrace;
        }

        void ClearConsole()
        {
            logOutput = ""; // 清空日志内容
            if (consoleText != null)
            {
                consoleText.text = "";
                LayoutRebuilder.ForceRebuildLayoutImmediate(consoleText.transform.parent.GetComponent<RectTransform>());
            }
        }
        #endregion
    }
}