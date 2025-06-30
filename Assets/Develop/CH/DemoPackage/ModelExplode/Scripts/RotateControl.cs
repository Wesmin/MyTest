using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControl : MonoBehaviour
{
    private Vector3 previousMousePosition;
    private bool isRightMousePressed = false;

    void Update()
    {
        // 检测鼠标右键是否按下
        if (Input.GetMouseButtonDown(1)) // 右键按下
        {
            isRightMousePressed = true;
            previousMousePosition = Input.mousePosition; // 记录初始鼠标位置
        }

        // 检测鼠标右键是否松开
        if (Input.GetMouseButtonUp(1)) // 右键松开
        {
            isRightMousePressed = false;
        }

        // 如果鼠标右键按下并且移动
        if (isRightMousePressed)
        {
            // 获取鼠标当前的位置
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;

            // 基于鼠标移动来旋转物体
            float rotationSpeed = 0.2f; // 控制旋转的速度
            float rotationX = deltaMousePosition.x * rotationSpeed;

            // 只绕 Y 轴旋转
            transform.Rotate(Vector3.up, -rotationX, Space.World); // 绕世界空间的 Y 轴旋转

            // 更新鼠标位置
            previousMousePosition = Input.mousePosition;
        }
    }
}
