using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace TS.Tools
{
    public class CodeMgr : MonoBehaviour
    {
        public TMP_InputField inputText;
        private string lastInfo;
        private string inputInfo;
        public Transform tipBar;
        public GameObject codeItemPrefab;


        private List<string> keys = new List<string>
        {
            "public", "private", "protected", "internal",
            "static", "readonly", "const", "enum", "struct",
            "class", "interface", "abstract", "override", "virtual",
            "void", "int", "float", "double", "bool", "string", "char",
            "return", "new", "foreach", "for", "while", "do",
            "if", "else", "switch", "case", "break", "continue",
            "using", "namespace", "try", "catch", "finally", "throw",
            //"in", "out", "ref", "params", "get", "set", "this", "base",
            "null", "true", "false"
        };

        private List<string> types = new List<string>
        {
            "MonoBehaviour", "ScriptableObject",
            "Transform", "GameObject", "RectTransform",
            "Vector2", "Vector3", "Vector4",
            "Quaternion", "Color", "Color32",
            "Rigidbody", "Rigidbody2D",
            "Collider", "Collider2D",
            "BoxCollider", "SphereCollider", "CapsuleCollider",
            "Animator", "Animation",
            "Camera", "Light", "Material", "Shader",
            "AudioSource", "AudioClip",
            "Sprite", "Texture2D",
            "Time", "Mathf", "Physics", "Physics2D",
            "Input", "KeyCode",
            "Canvas", "Button", "Image", "Text", "TMP_InputField",
            "List", "Dictionary"
        };

        private string space = " ";

        private string keyColor = "#4AA3FF";
        private string typeColor = "#66CC66";


        void Start()
        {
            lastInfo = inputText.text;
            inputText.onValueChanged.AddListener(OnInputChanged);
        }

        void Update()
        {
            if (tipBar.IsActive())
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {

                }
            }
        }

        private bool TryHandleDeleteRichText_NoRegex(string newText)
        {
            // 未初始化
            if (string.IsNullOrEmpty(lastInfo))
            {
                lastInfo = newText;
                return false;
            }

            // 必须是删除
            if (newText.Length >= lastInfo.Length) return false;

            // 找删除字符的位置：找第一个不同字符
            int minLen = Mathf.Min(newText.Length, lastInfo.Length);
            int diffIndex = -1;
            for (int i = 0; i < minLen; i++)
            {
                if (newText[i] != lastInfo[i])
                {
                    diffIndex = i;
                    break;
                }
            }
            if (diffIndex == -1)
                diffIndex = newText.Length;

            // 防御
            if (diffIndex < 0 || diffIndex >= lastInfo.Length)
            {
                lastInfo = newText;
                return false;
            }

            // 必须是删除 '>'
            if (lastInfo[diffIndex] != '>')
            {
                lastInfo = newText;
                return false;
            }

            //---------------------------------------------------
            // 1) 找光标之前的最近一个空格
            //---------------------------------------------------
            int caret = inputText.caretPosition; // 删除后的光标
            int searchPos = Mathf.Clamp(caret, 0, lastInfo.Length);

            int lastSpace = -1;
            for (int i = searchPos - 1; i >= 0; i--)
            {
                if (lastInfo[i] == ' ')
                {
                    lastSpace = i;
                    break;
                }
            }

            int segmentStart = (lastSpace == -1) ? 0 : lastSpace + 1;
            if (segmentStart >= lastInfo.Length)
            {
                lastInfo = newText;
                return false;
            }

            //---------------------------------------------------
            // 2) 截取空格后面的内容
            //---------------------------------------------------
            string segment = lastInfo.Substring(segmentStart);

            //---------------------------------------------------
            // 3) 判断是否包含 keyColor 或 typeColor
            //---------------------------------------------------
            if (!(segment.Contains($"<color={keyColor}>") ||
                  segment.Contains($"<color={typeColor}>")))
            {
                lastInfo = newText;
                return false;
            }

            //---------------------------------------------------
            // 4) 清理标签（不用正则！）
            //---------------------------------------------------
            string cleaned = segment;

            // 去掉开标签（只替换你定义的颜色）
            cleaned = cleaned.Replace($"<color={keyColor}>", "");
            cleaned = cleaned.Replace($"<color={typeColor}>", "");

            // 去掉闭合标签
            cleaned = cleaned.Replace("</color>", "");

            //---------------------------------------------------
            // 5) 用 cleaned 替换 segment
            //---------------------------------------------------
            string replaced = lastInfo.Remove(segmentStart, segment.Length)
                                      .Insert(segmentStart, cleaned);

            //---------------------------------------------------
            // 6) 再执行一次退格（删除 cleaned 最后一个字符）
            //---------------------------------------------------
            int deleteIndex = segmentStart + cleaned.Length - 1;
            if (deleteIndex < 0 || deleteIndex >= replaced.Length)
                deleteIndex = replaced.Length - 1;

            string finalText = replaced.Remove(deleteIndex, 1);

            //---------------------------------------------------
            // 7) 更新 UI
            //---------------------------------------------------
            inputText.text = finalText;

            int newCaret = Mathf.Clamp(deleteIndex, 0, finalText.Length);
            inputText.caretPosition = newCaret;
            inputText.stringPosition = newCaret;

            lastInfo = finalText;

            return true; // 告诉调用者：已经处理完，不要继续后续自动着色逻辑
        }


        // 输入变化
        private void OnInputChanged(string text)
        {
            // 先处理退格逻辑
            if (text.Length < lastInfo.Length)
            {
                if (TryHandleDeleteRichText_NoRegex(text))
                {
                    return; // 必须 return，否则后面你的自动补全逻辑会继续运行导致错误
                }
            }


            inputInfo = text.Substring(lastInfo.Length);
            Debug.Log(inputInfo);

            bool bo = false;

            // 先回收到对象池
            foreach (Transform item in tipBar)
                item.Hide();

            // 获取输入内容长度
            int inputLength = inputInfo.Length;
            // 检索关键词
            foreach (var key in keys.Concat(types))
            {
                // 输入为空不匹配
                if (inputLength == 0)
                {
                    bo = false;
                    break;
                }

                // item 也为空跳过
                if (string.IsNullOrEmpty(key))
                    continue;

                // item 长度不够 → 不符合
                if (key.Length < inputLength)
                    continue;

                // 截取左侧等长字符串
                string sub = key.Substring(0, inputLength);

                // 判断是否完全相等（你可改成 ToLower 比较不区分大小写）
                if (sub == inputInfo)
                {
                    Transform codeItem = null;

                    // 先找一个未激活的子物体来复用
                    for (int i = 0; i < tipBar.childCount; i++)
                    {
                        if (!tipBar.GetChild(i).gameObject.activeSelf)
                        {
                            codeItem = tipBar.GetChild(i);
                            break;
                        }
                    }

                    // 如果没有未激活的，创建新的
                    if (codeItem == null)
                    {
                        codeItem = Instantiate(codeItemPrefab, tipBar).transform;
                    }

                    // 使用这个子物体
                    codeItem.Show();
                    codeItem.GetComponent<CodeItem>().SetText(key);

                    bo = true;
                }
            }


            // 如果是关键词自动
            foreach (var key in keys)
            {
                if(inputInfo == key)
                {
                    string str = $"<color={keyColor}>{inputInfo}</color>{space}"; // 当前输入阶段的内容
                    inputText.text = inputText.text.Replace($"{inputInfo}", str); // 加上之前的内容
                    lastInfo = inputText.text; // 设置成旧内容
                    SetCaretPos(lastInfo.Length + 1);
                    bo = false;
                    break;
                }
            }
            foreach (var type in types)
            {
                if (inputInfo == type)
                {
                    string str = $"<color={typeColor}>{inputInfo}</color> ";
                    inputText.text = inputText.text.Replace($"{inputInfo}", str);
                    lastInfo = inputText.text; 
                    SetCaretPos(lastInfo.Length + 1);
                    bo = false;
                    break;
                }
            }


            // 非关键类型内容
            if (inputInfo.Contains($"{space}"))
            {
                lastInfo = inputText.text;
                bo = false;
            }
            

            tipBar.SetActive(bo);

        }

        /// <summary>
        /// 设置光标位置
        /// </summary>
        void SetCaretPos(int index)
        {
            inputText.caretPosition = index;
            inputText.stringPosition = index;
            //inputText.selectionFocusPosition = index;
        }
    }
}