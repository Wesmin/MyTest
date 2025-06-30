using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static PlayerMove Instance;

    [Header("行走区域层级名称")]
    public string walkZoneLayerName;

    [Header("移动速度")]
    public float moveSpeed = 2;

    [Header("是否可以移动，在碰撞区域可以移动")]
    public bool CanMove = true;

    [Header("是否在碰撞器范围内（行走区域内）")]
    public bool inArea = false;

    [Header("行走区域")]
    [SerializeField] List<Collider> colliders = new List<Collider>();

    Ray bodyRay;

    RaycastHit bodyHit;

    private float horizontalMove, verticalMove;

    private Vector3 dir;

    /// <summary>
    /// 下一步移动到的点
    /// </summary>
    Vector3 nextStepPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (CanMove)
        {
            //移动
            horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
            verticalMove = Input.GetAxis("Vertical") * moveSpeed;
            dir = transform.forward * verticalMove + transform.right * horizontalMove;

            //计算 下一步要移动到的位置
            nextStepPosition = this.transform.position + dir * 0.02f;

            //判断下一步要移动到的位置是否在可移动范围内
            for (int i = 0; i < colliders.Count; i++)
            {
                inArea = false;
                if (colliders[i].bounds.Contains(nextStepPosition))
                {
                    inArea = true;
                    break;
                }
            }

            //如果在范围内 发生移动
            if (inArea && (Mathf.Abs(horizontalMove) > 0.1f || Mathf.Abs(verticalMove) > 0.1f))
            {
                this.transform.position = nextStepPosition;
            }


            //高度位置调整
            //原理： 以一点为起点，向下发射射线，此物体的高度为射线照射到的点的高度

            //设置射线  射线起始点为此物体的y轴向上 +0.25 的高度 
            bodyRay = new Ray(
                new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z),
                Vector3.down);
            //射线照射的层级 最高落差为200
            if (Physics.Raycast(bodyRay, out bodyHit, 200, LayerMask.GetMask(walkZoneLayerName)))
            {
                float a = this.transform.position.y - bodyHit.point.y;

                if (Mathf.Abs(a) > 0.05f)
                {
                    this.transform.DOMoveY(bodyHit.point.y, 0.2f);
                }
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == walkZoneLayerName)
        {
            colliders.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == walkZoneLayerName)
        {
            if (colliders.Contains(other))
            {
                colliders.Remove(other);
            }
        }
    }
}