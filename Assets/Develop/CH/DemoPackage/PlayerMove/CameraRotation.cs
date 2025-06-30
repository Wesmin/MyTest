using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("相机旋转灵敏度调节值")]
    public float sensitivity = 200f;

    /// <summary>
    /// 父物体（移动和检测碰撞的物体）
    /// </summary>
    private Transform player;

    /// <summary>
    /// 鼠标坐标指
    /// </summary>
    private float mouseX, mouseY;

    /// <summary>
    /// 数值缓存
    /// </summary>
    private float tempFloat;

    private void Start()
    {
        player = transform.parent;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))  // 按住右键时开始旋转
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity * 0.02f;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity * 0.02f;

            // 使用插值减少晃动
            tempFloat -= mouseY;
            // 限制上下旋转角度
            tempFloat = Mathf.Clamp(tempFloat, -50f, 50f);

            // 处理父物体的水平旋转
            player.Rotate(Vector3.up * mouseX);

            // 设置相机的竖直旋转，保持当前水平旋转
            transform.localRotation = Quaternion.Euler(tempFloat, transform.localRotation.eulerAngles.y, 0);
        }
        else  // 松开右键时保持状态
        {
            // 保持当前竖直旋转，不恢复y轴的旋转
            transform.localRotation = Quaternion.Euler(tempFloat, transform.localRotation.eulerAngles.y, 0);
        }
    }
}
