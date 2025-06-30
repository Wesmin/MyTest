using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollViewController : MonoBehaviour
{
    public ScrollRect scrollView; // 右侧的ScrollView
    public List<Button> buttons; // 左侧的按钮列表
    public List<RectTransform> contentItems; // 右侧ScrollView中的内容项列表
    public Sprite normalSprite; // 按钮的正常状态图片
    public Sprite highlightedSprite; // 按钮的高亮状态图片

    private void Start()
    {
        // 为每个按钮添加点击事件监听器
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // 捕获按钮点击时的索引
            buttons[i].onClick.AddListener(() => scrollView.normalizedPosition = scrollView.UpdateTargetPos(contentItems[index]));
        }
    }

    private void Update()
    {
        OnScrollValueChanged();
    }

    /// <summary>
    /// ScrollView面板滑动时，找到与当前滚动位置最接近的内容项
    /// </summary>
    private void OnScrollValueChanged()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        // 找到与当前滚动位置最接近的内容项
        for (int i = 0; i < contentItems.Count; i++)
        {
            float distance = Mathf.Abs(scrollView.content.anchoredPosition.y + contentItems[i].anchoredPosition.y);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // 根据当前滚动位置更新按钮的图片
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == closestIndex)
            {
                buttons[i].image.sprite = highlightedSprite;
            }
            else
            {
                buttons[i].image.sprite = normalSprite;
            }
        }
    }
}
public static class ScrollRectExtension
{
    /// <summary>
    /// ScrollRect滚动到指定位置。
    /// 调用示例：ScrollRect.normalizedPosition = ScrollRect.UpdateTargetPos(RectTransform);
    /// </summary>
    public static Vector2 UpdateTargetPos(this ScrollRect scrollRect, RectTransform item)
    {
        // 获取item在ScrollRect的RectTransform中的局部位置
        Vector3 itemCurrentLocalPostion = scrollRect.GetComponent<RectTransform>().InverseTransformPoint(ConvertLocalPosToWorldPos(item));
        // 获取viewport在ScrollRect的RectTransform中的局部位置
        Vector3 viewportLocalPosition = scrollRect.GetComponent<RectTransform>().InverseTransformPoint(ConvertLocalPosToWorldPos(scrollRect.viewport));

        // 计算item当前位置和目标位置之间的差异
        Vector3 diff = viewportLocalPosition - itemCurrentLocalPostion;
        diff.z = 0.0f; // 忽略z轴差异

        // 将diff.y设置为使item顶部与viewport顶部对齐
        diff.y += (scrollRect.viewport.rect.height / 2) - (item.rect.height * (1 - item.pivot.y));

        // 计算新的标准化位置
        var newNormalizedPosition = new Vector2(
            diff.x / (scrollRect.content.rect.width - scrollRect.viewport.rect.width),
            diff.y / (scrollRect.content.rect.height - scrollRect.viewport.rect.height)
        );

        // 更新ScrollRect的标准化位置
        newNormalizedPosition = scrollRect.normalizedPosition - newNormalizedPosition;

        // 确保标准化位置在0和1之间
        newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
        newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

        // 有DOTween时使用
        // DOTween.To(() => scrollRect.normalizedPosition, x => scrollRect.normalizedPosition = x, newNormalizedPosition, 1f);
        // 无DOTween时使用
        scrollRect.normalizedPosition = newNormalizedPosition;

        return newNormalizedPosition; // 返回新的标准化位置
    }

    /// <summary>
    /// 将RectTransform的局部位置转换为世界位置
    /// </summary>
    private static Vector3 ConvertLocalPosToWorldPos(RectTransform target)
    {
        // 计算枢轴偏移
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);

        // 计算局部位置
        var localPosition = target.localPosition + pivotOffset;

        // 将局部位置转换为世界位置
        return target.parent.TransformPoint(localPosition);
    }
}
