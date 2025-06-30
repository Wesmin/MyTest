using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("相机挂载此脚本，Player为相机父物体")]
    public Transform Player;
    [Header("相机旋转灵敏度调节值")]
    public float sensitivity = 5f;
    [Header("移动速度")]
    public float movespeed = 0.05F;
    [Header("相机上下旋转角度")]
    public float AngleUP = 50f;
    public float AngleDown = 50f;

    //鼠标XY
    private float mouseX, mouseY;
    //数值缓存
    private float tempFloat;


    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            //使用插值减少晃动
            tempFloat -= mouseY;
            //限制上下旋转角度
            tempFloat = Mathf.Clamp(tempFloat, -AngleUP, AngleDown);

            //父物体旋转
            Player.Rotate(Vector3.up * mouseX);
            //相机自身上下旋转
            transform.localRotation = Quaternion.Euler(tempFloat, 0, 0);
        }
    }

    /// <summary>
    /// GUI提示
    /// </summary>
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 65;
        fontStyle.fontStyle = FontStyle.Bold;
        fontStyle.normal.textColor = Color.cyan;

        GUI.Label(new Rect(20, 20, 50, 60),"鼠标右键旋转镜头", fontStyle);
    }
}
