using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TS.Tools;

public class CodeInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public Transform tipBar;
    public GameObject codeItemPrefab;

    private List<GameObject> codeItemPool = new List<GameObject>();
    private int selectedIndex = -1;
    private string lastPlainText = "";
    private bool hasSelectedTip = false;

    private readonly string[] csKeywords = new string[]
    {
        "public", "private", "protected", "static", "class", "void", "int", "float",
        "bool", "string", "return", "new", "foreach", "for", "while", "if", "else"
    };

    private readonly string[] unityTypes = new string[]
    {
        "Transform", "GameObject", "Vector3", "Quaternion", "MonoBehaviour"
    };

    private string keywordColor = "#4AA3FF";
    private string typeColor = "#66CC66";

    private bool suppressCallback = false;

    void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);

        // 初始化lastPlainText
        lastPlainText = RemoveColorTags(inputField.text);
    }

    void Update()
    {
        if (!inputField.isFocused) return;

        // TipBar 上下选择
        if (tipBar.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveSelection(1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.Return) && selectedIndex >= 0)
            {
                ConfirmTip();
                return; // 防止重复处理回车
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && selectedIndex >= 0)
            {
                ConfirmTip();
                return;
            }
        }

        // 检测直接按回车键的情况（非提交）
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // 如果提示栏没有激活，则处理自动缩进
            if (!tipBar.gameObject.activeSelf)
            {
                ProcessAutoIndent();
            }
        }

        // Backspace触发提示
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnInputChanged(inputField.text);
        }
    }

    // 输入变化
    private void OnInputChanged(string text)
    {
        if (suppressCallback) return;

        // 获取纯文本
        string plainText = RemoveColorTags(text);

        // 检查是否是换行操作
        if (plainText.Length > lastPlainText.Length && plainText.Length > 0 && plainText[plainText.Length - 1] == '\n')
        {
            ProcessAutoIndent();
            return;
        }

        suppressCallback = true;

        // 保存当前光标在纯文本中的位置
        int caretInPlainText = GetCaretInPlainText(text, inputField.caretPosition);

        // 应用高亮
        string colored = ApplyHighlighting(plainText);

        // 计算高亮后的光标位置
        int newCaretPos = GetCaretInColoredText(colored, caretInPlainText);

        inputField.text = colored;
        inputField.caretPosition = newCaretPos;

        // 更新 TipBar
        UpdateTipBar();

        suppressCallback = false;

        // 保存当前纯文本
        lastPlainText = plainText;
    }

    // 去除颜色标签
    private string RemoveColorTags(string text)
    {
        return Regex.Replace(text, @"<color=[^>]*>|</color>", "");
    }

    // 获取光标在纯文本中的位置
    private int GetCaretInPlainText(string coloredText, int coloredCaretPos)
    {
        int plainPos = 0;
        bool inTag = false;

        for (int i = 0; i < coloredText.Length && i < coloredCaretPos; i++)
        {
            if (coloredText[i] == '<')
            {
                inTag = true;
            }
            else if (coloredText[i] == '>')
            {
                inTag = false;
                continue;
            }

            if (!inTag)
            {
                plainPos++;
            }
        }

        return plainPos;
    }

    // 获取高亮文本中的光标位置
    private int GetCaretInColoredText(string coloredText, int plainCaretPos)
    {
        int plainPos = 0;

        for (int i = 0; i < coloredText.Length; i++)
        {
            if (coloredText[i] == '<')
            {
                // 跳过整个标签
                int tagEnd = coloredText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    i = tagEnd;
                    continue;
                }
            }

            if (plainPos >= plainCaretPos)
            {
                return i;
            }

            plainPos++;
        }

        return coloredText.Length;
    }

    private string ApplyHighlighting(string plainText)
    {
        string result = plainText;

        // 先处理较长的关键词，避免较短的关键词被错误替换
        var allKeywords = csKeywords.Concat(unityTypes).Distinct().OrderByDescending(k => k.Length).ToArray();

        foreach (string keyword in allKeywords)
        {
            // 确定颜色
            string color = csKeywords.Contains(keyword) ? keywordColor : typeColor;

            // 构建正则表达式，匹配单词边界
            string pattern = $@"\b{Regex.Escape(keyword)}\b";
            string replacement = $"<color={color}>{keyword}</color>";

            result = Regex.Replace(result, pattern, replacement);
        }

        return result;
    }

    // 自动缩进处理方法
    private void ProcessAutoIndent()
    {
        string plainText = RemoveColorTags(inputField.text);
        int caretInPlainText = GetCaretInPlainText(inputField.text, inputField.caretPosition);

        if (caretInPlainText <= 0 || caretInPlainText > plainText.Length) return;

        // 查找当前行的起始位置
        int lineStart = plainText.LastIndexOf('\n', caretInPlainText - 1);
        if (lineStart < 0) lineStart = 0;
        else lineStart++; // 跳过换行符

        // 查找前一行的起始位置
        int prevLineStart = plainText.LastIndexOf('\n', lineStart - 1);
        if (prevLineStart < 0) prevLineStart = 0;
        else prevLineStart++;

        // 获取前一行的内容
        string prevLine = plainText.Substring(prevLineStart, lineStart - prevLineStart);

        // 检查前一行是否有 {
        if (prevLine.Contains("{"))
        {
            // 在光标位置插入两个全角空格
            string beforePlain = plainText.Substring(0, caretInPlainText - 1);
            string afterPlain = plainText.Substring(caretInPlainText - 1);

            beforePlain += "　　"; // 两个全角空格

            // 应用高亮
            string newColored = ApplyHighlighting(beforePlain + afterPlain);

            // 计算新光标位置
            int newCaretPos = GetCaretInColoredText(newColored, beforePlain.Length);

            suppressCallback = true;
            inputField.text = newColored;
            inputField.caretPosition = newCaretPos;
            suppressCallback = false;

            // 更新纯文本
            lastPlainText = beforePlain + afterPlain;
        }
    }

    private string GetCurrentWord()
    {
        string plainText = RemoveColorTags(inputField.text);
        int caretInPlainText = GetCaretInPlainText(inputField.text, inputField.caretPosition);

        if (caretInPlainText == 0 || caretInPlainText > plainText.Length) return "";

        // 从光标位置向前查找单词起始位置
        int start = caretInPlainText - 1;
        while (start >= 0)
        {
            char c = plainText[start];
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                start--;
            }
            else
            {
                break;
            }
        }

        start++; // 调整到单词起始位置

        // 从起始位置向后查找单词结束位置
        int end = caretInPlainText;
        while (end < plainText.Length)
        {
            char c = plainText[end];
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                end++;
            }
            else
            {
                break;
            }
        }

        if (start >= 0 && start < end && end <= plainText.Length)
        {
            return plainText.Substring(start, end - start);
        }

        return "";
    }

    private void UpdateTipBar()
    {
        // 如果刚刚选择了提示，就隐藏tipbar
        if (hasSelectedTip)
        {
            tipBar.gameObject.SetActive(false);
            hasSelectedTip = false;
            return;
        }

        string word = GetCurrentWord();

        if (string.IsNullOrEmpty(word))
        {
            tipBar.gameObject.SetActive(false);
            return;
        }

        List<string> matches = new List<string>();

        // 匹配所有以当前单词开头的关键词
        foreach (string kw in csKeywords)
        {
            if (kw.StartsWith(word, System.StringComparison.OrdinalIgnoreCase))
            {
                matches.Add(kw);
            }
        }

        if (matches.Count > 0)
        {
            ShowTipBar(matches);
            PositionTipBar();
        }
        else
        {
            tipBar.gameObject.SetActive(false);
        }
    }

    private void ShowTipBar(List<string> matches)
    {
        tipBar.gameObject.SetActive(true);

        for (int i = 0; i < matches.Count; i++)
        {
            GameObject item;
            if (i < codeItemPool.Count)
            {
                item = codeItemPool[i];
                item.SetActive(true);
            }
            else
            {
                item = Instantiate(codeItemPrefab, tipBar);
                // 添加点击事件
                Button btn = item.GetComponent<Button>();
                if (btn != null)
                {
                    int index = i;
                    btn.onClick.AddListener(() => OnTipItemClicked(index));
                }
                codeItemPool.Add(item);
            }

            item.GetComponent<CodeItem>().SetText(matches[i]);
            item.GetComponent<CodeItem>().SetSelected(i == selectedIndex);
        }

        for (int i = matches.Count; i < codeItemPool.Count; i++)
            codeItemPool[i].SetActive(false);

        // 如果没有选择，默认选择第一个
        if (selectedIndex < 0 || selectedIndex >= matches.Count)
        {
            selectedIndex = 0;
            if (codeItemPool.Count > 0 && codeItemPool[0].activeSelf)
                codeItemPool[0].GetComponent<CodeItem>().SetSelected(true);
        }
    }

    private void PositionTipBar()
    {
        TMP_Text textComp = inputField.textComponent;
        textComp.ForceMeshUpdate();

        // 使用字符信息定位提示栏
        int caretIndex = Mathf.Clamp(inputField.caretPosition, 0, textComp.textInfo.characterCount - 1);

        if (textComp.textInfo.characterCount > 0 && caretIndex >= 0 && caretIndex < textComp.textInfo.characterCount)
        {
            var charInfo = textComp.textInfo.characterInfo[caretIndex];
            Vector3 localPos = charInfo.bottomLeft;
            Vector3 worldPos = textComp.transform.TransformPoint(localPos);
            tipBar.position = worldPos + new Vector3(0, -50, 0);
        }
    }

    private void MoveSelection(int dir)
    {
        // 找到所有激活的item
        List<int> activeIndices = new List<int>();
        for (int i = 0; i < codeItemPool.Count; i++)
        {
            if (codeItemPool[i].activeSelf)
                activeIndices.Add(i);
        }

        if (activeIndices.Count == 0)
        {
            selectedIndex = -1;
            return;
        }

        // 取消当前选择
        if (selectedIndex >= 0 && selectedIndex < codeItemPool.Count && codeItemPool[selectedIndex].activeSelf)
            codeItemPool[selectedIndex].GetComponent<CodeItem>().SetSelected(false);

        // 计算新的选择索引
        int currentActiveIndex = activeIndices.IndexOf(selectedIndex);
        if (currentActiveIndex < 0) currentActiveIndex = 0;

        int newActiveIndex = currentActiveIndex + dir;
        if (newActiveIndex >= activeIndices.Count) newActiveIndex = 0;
        if (newActiveIndex < 0) newActiveIndex = activeIndices.Count - 1;

        selectedIndex = activeIndices[newActiveIndex];

        // 应用新的选择
        if (selectedIndex >= 0 && selectedIndex < codeItemPool.Count)
            codeItemPool[selectedIndex].GetComponent<CodeItem>().SetSelected(true);
    }

    private void OnTipItemClicked(int index)
    {
        if (index >= 0 && index < codeItemPool.Count && codeItemPool[index].activeSelf)
        {
            // 设置选择
            if (selectedIndex >= 0 && selectedIndex < codeItemPool.Count && codeItemPool[selectedIndex].activeSelf)
                codeItemPool[selectedIndex].GetComponent<CodeItem>().SetSelected(false);

            selectedIndex = index;
            codeItemPool[selectedIndex].GetComponent<CodeItem>().SetSelected(true);

            // 确认选择
            ConfirmTip();
        }
    }

    private void ConfirmTip()
    {
        if (selectedIndex < 0 || selectedIndex >= codeItemPool.Count || !codeItemPool[selectedIndex].activeSelf)
            return;

        CodeItem item = codeItemPool[selectedIndex].GetComponent<CodeItem>();
        if (item == null) return;

        string keyword = item.GetText();
        string plainText = RemoveColorTags(inputField.text);
        int caretInPlainText = GetCaretInPlainText(inputField.text, inputField.caretPosition);

        // 获取当前单词的起始和结束位置（在纯文本中）
        int wordStart = -1, wordEnd = -1;

        // 查找单词起始位置
        int searchPos = caretInPlainText - 1;
        while (searchPos >= 0)
        {
            char c = plainText[searchPos];
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                wordStart = searchPos;
                searchPos--;
            }
            else
            {
                break;
            }
        }

        if (wordStart < 0) wordStart = caretInPlainText;
        else wordStart++; // 调整到单词起始位置

        // 查找单词结束位置
        searchPos = caretInPlainText;
        while (searchPos < plainText.Length)
        {
            char c = plainText[searchPos];
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                wordEnd = searchPos + 1;
                searchPos++;
            }
            else
            {
                break;
            }
        }

        if (wordEnd < 0) wordEnd = caretInPlainText;

        // 确保wordStart <= wordEnd
        if (wordStart > wordEnd)
        {
            int temp = wordStart;
            wordStart = wordEnd;
            wordEnd = temp;
        }

        // 构建新文本
        string before = plainText.Substring(0, wordStart);
        string after = plainText.Substring(wordEnd);
        string newPlainText = before + keyword + " " + after;

        // 应用高亮
        string newColored = ApplyHighlighting(newPlainText);

        // 计算新光标位置
        int newCaretInPlainText = before.Length + keyword.Length + 1;
        int newCaretPos = GetCaretInColoredText(newColored, newCaretInPlainText);

        suppressCallback = true;
        inputField.text = newColored;
        inputField.caretPosition = newCaretPos;
        suppressCallback = false;

        // 标记已选择提示，这样UpdateTipBar会隐藏tipbar
        hasSelectedTip = true;
        tipBar.gameObject.SetActive(false);

        // 更新纯文本
        lastPlainText = newPlainText;
    }
}