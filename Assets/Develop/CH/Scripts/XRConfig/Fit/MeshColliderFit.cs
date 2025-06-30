using UnityEngine;
namespace TS.Tools
{
    public class MeshColliderFit : MonoBehaviour
    {
        public bool IsFit;
        void OnValidate()
        {
            // 在编辑模式下，检测 IsFit 值的变化
            if (IsFit)
            {
                FitColliderToMesh();
                IsFit = false; // 重置布尔值
            }
        }
        /// <summary>
        /// 根据Mesh自适应临界边框生成BoxCollider
        /// </summary>
        public void FitColliderToMesh()
        {
            // 获取物体及其子物体中的所有 MeshRenderer
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            if (meshRenderers.Length == 0)
            {
                Debug.LogWarning("在物体及其子物体中未找到任何 MeshRenderers。");
                return;
            }

            // 初始化最小和最大边界
            Vector3 minBounds = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maxBounds = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // 遍历所有的 MeshRenderer 以找到全局边界
            foreach (MeshRenderer renderer in meshRenderers)
            {
                // 获取renderer的世界边界
                Bounds bounds = renderer.bounds;

                // 生成边界的8个角点
                Vector3[] corners = new Vector3[8];
                corners[0] = bounds.min;
                corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
                corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
                corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
                corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
                corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
                corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
                corners[7] = bounds.max;

                // 将每个角点转换到根物体的局部空间，并更新最小和最大边界
                foreach (Vector3 corner in corners)
                {
                    Vector3 localCorner = transform.InverseTransformPoint(corner);
                    minBounds = Vector3.Min(minBounds, localCorner);
                    maxBounds = Vector3.Max(maxBounds, localCorner);
                }
            }

            // 计算 BoxCollider 的中心和大小
            Vector3 center = (minBounds + maxBounds) / 2;
            Vector3 size = maxBounds - minBounds;

            // 检查是否已有 BoxCollider
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                // 如果没有，添加新的 BoxCollider
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            // 设置 BoxCollider 的中心和大小
            boxCollider.center = center;
            boxCollider.size = size;
        }
    }
}