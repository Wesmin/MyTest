using UnityEngine.EventSystems;
using UnityEngine;
using TS.Tools;
/// <summary>
/// PC模式下交互核心脚本，挂载到PC摄像机上
/// </summary>
[RequireComponent(typeof(PhysicsRaycaster))]
public class PcTouchItem : MonoBehaviour
{
    private float lastClickTime = 0f; // 上次点击的时间
    private float doubleClickThreshold = 0.3f; // 双击的时间间隔阈值（秒）

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int layerMask = LayerMask.GetMask(BaseConfig.TouchLayer); // 替换为你想检测的层的名称

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (HandExtend.IsDoubleClick)
                {
                    // 双击功能实现
                    if (Time.time - lastClickTime <= doubleClickThreshold)
                    {
                        HandleInteraction(hit);
                    }
                    // 更新上次点击时间
                    lastClickTime = Time.time;
                }
                else
                {
                    // 单击功能实现
                    HandleInteraction(hit);
                }
            }
        }
    }

    /// <summary>
    /// 处理交互的公共方法
    /// </summary>
    private void HandleInteraction(RaycastHit hit)
    {
        // VR模式下的所有手交互事件
        if (hit.collider.transform.GetComponent<HandTouch>())
        {
            hit.collider.transform.GetComponent<HandTouch>().ExecuteEnd();
        }
        // VR模式下的所有物品与物品交互事件
        else if (hit.collider.transform.GetComponent<TriggerTag>())
        {
            HandExtend.TriggerOBJ.GetComponent<HandTrigger>().ExecuteEnd(hit.collider);
        }
        //// VR模式下的所有手射线交互事件
        //else if (hit.collider.transform.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRSimpleInteractable>())
        //{
        //    HandRayHit.Instance.ExecuteEnd();
        //}
    }
}
