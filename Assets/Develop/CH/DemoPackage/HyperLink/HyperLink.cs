using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HyperLink : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        // 示例文本（可以替换为 AI 返回的字符串）
        string rawText = "请访问 https://openai.com 或 https://unity.com 获取更多信息。";
        // 自动转换链接
        textMeshPro.text = ConvertUrlsToLinks(rawText);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 获取点击位置处的链接索引（Overlay 模式传 null）
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string url = linkInfo.GetLinkID();

            Debug.Log("Opening URL: " + url);
            Application.OpenURL(url);
        }
    }

    // 将文本中的 URL 替换成 TMP 的 <link> 标签格式
    private string ConvertUrlsToLinks(string text)
    {
        string pattern = @"(http[s]?://[^\s<>]+)";
        return Regex.Replace(text, pattern, match =>
        {
            string url = match.Value;
            return $"<link=\"{url}\"><color=yellow><u>{url}</u></color></link>";
        });
    }
}
