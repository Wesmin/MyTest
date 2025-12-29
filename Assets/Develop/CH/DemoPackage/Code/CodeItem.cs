using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TS.Tools
{
    /// <summary>
    /// TipBar单项显示控件
    /// </summary>
    public class CodeItem : MonoBehaviour
    {
        public TMP_Text text;
        public Image bgImage;
        public Sprite normalSprite;
        public Sprite selectedSprite;

        public void SetText(string s)
        {
            if (text != null)
                text.text = s;
        }
        public string GetText()
        {
            return text != null ? text.text : "";
        }
        public void SetSelected(bool selected)
        {
            if (bgImage != null)
                bgImage.sprite = selected ? selectedSprite : normalSprite;
        }
    }
}
