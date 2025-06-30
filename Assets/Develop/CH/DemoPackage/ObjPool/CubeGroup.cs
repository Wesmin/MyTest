using UnityEngine;

public class CubeGroup : MonoBehaviour
{
    // 定义间距
    public float spacingX = 1f;
    public float spacingY = 1f;
    public float spacingZ = 1f;

    private void Update()
    {
        ArrangeObjects();
    }

    void ArrangeObjects()
    {
        // 获取子物体数量
        int childCount = transform.childCount;
        // 计算第一个子物体的初始位置
        Vector3 startPosition = new Vector3(
            -spacingX * (childCount - 1) / 2f,
            spacingY * (childCount - 1) / 2f,
            -spacingZ * (childCount - 1) / 2f
        );

        for (int i = 0; i < childCount; i++)
        {
            // 获取当前子物体
            Transform child = transform.GetChild(i);
            // 计算当前子物体的位置
            Vector3 position = startPosition + new Vector3(
                i * spacingX,
                -i * spacingY,
                i * spacingZ
            );
            // 设置子物体的位置
            child.localPosition = position;
        }
    }
}
