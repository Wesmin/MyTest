using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    [Header("相机挂载此脚本，Player为相机父物体")]
    public Transform Player;       
    [Header("相机旋转灵敏度调节值")]
    public float sensitivity=5f;
    [Header("移动速度")]
    public float movespeed=0.02F;
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

        if (Input.GetKey(KeyCode.A))
        {
            Player.transform.position += Player.transform.right * -movespeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Player.transform.position += Player.transform.right * movespeed;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Player.transform.position += Player.transform.forward * movespeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Player.transform.position += Player.transform.forward * -movespeed;
        }
    }
}
