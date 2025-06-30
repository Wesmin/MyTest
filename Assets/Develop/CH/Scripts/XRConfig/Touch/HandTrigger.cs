using UnityEngine;

namespace TS.Tools
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshColliderFit))]
    public class HandTrigger : MonoBehaviour
    {
        /// <summary>
        /// 首次添加脚本时，对依赖代码的管理 的判断
        /// </summary>
        private bool isRequire = true;
        /// <summary>
        /// 编辑器
        /// </summary>
        private void OnValidate()
        {
            if (isRequire)
            {
                isRequire = false;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.GetComponent<MeshColliderFit>().FitColliderToMesh();
            }
        }
        /// <summary>
        /// 左右手判断 用于执行手柄震动
        /// </summary>
        public bool isRight = true;
        /// <summary>
        /// 记录此刻开启碰撞的物体
        /// </summary>
        private void OnEnable()
        {
            HandExtend.TriggerOBJ = this.gameObject;
        }
        /// <summary>
        /// 碰撞检测
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BoxCollider>() == null)
            {
                Debug.Log("BoxCollider is missing");
            }
            else
            {
                if (other.GetComponent<TriggerTag>() != null)
                {
                    ExecuteEnd(other);
                }
            }
        }
        /// <summary>
        /// 结束执行
        /// </summary>
        public void ExecuteEnd(Collider other)
        {
            BaseConfig.ShakeHandle(isRight);
            Destroy(other.GetComponent<TriggerTag>());
            Resources.UnloadUnusedAssets();
            other.gameObject.TSHighlightHide();
            other.gameObject.SetActive(HandExtend.IsActive);
            BaseConfig.NextStep();
        }
    }
}

