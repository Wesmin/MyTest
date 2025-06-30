using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TS.Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform mainCameraTransform;

        void Start()
        {
            mainCameraTransform = Camera.main.transform;
        }

        void Update()
        {
            // 让面板始终朝向相机位置
            transform.LookAt(mainCameraTransform);

            // 仅允许Y轴旋转，限制其他轴旋转为原始值
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        }
    }
}