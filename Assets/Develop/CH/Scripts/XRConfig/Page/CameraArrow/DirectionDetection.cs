using UnityEngine;
namespace TS.Tools
{
    public class DirectionDetection : MonoBehaviour
    {
        public static DirectionDetection Instance;
        private void Awake()
        {
            Instance = this;
        }


        public Transform target;
        public Transform detectionAngleGo;
        public float rotateLerp = 0.1f;
        public float detectionAngle = 0;

        //public GameObject Arrow;

        private void OnValidate()
        {
            detectionAngleGo = detectionAngleGo == null ? this.transform : detectionAngleGo;
        }
        private void LateUpdate()
        {
            if (target == null) return;
            //映射角度
            Vector3 dirTemp = transform.forward.normalized;
            Vector3 tranDirTemp = transform.forward;
            Vector3 dir = PlanarMapping(dirTemp, transform.position, target.position) - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tranDirTemp, dir), rotateLerp);
            //计算夹角
            Vector3 forwardDirection = detectionAngleGo.forward;
            Vector3 toTargetDirection = (target.position - detectionAngleGo.position).normalized;

            // 计算两个向量之间的角度
            detectionAngle = Vector3.Angle(forwardDirection, toTargetDirection);

        }
        Vector3 PlanarMapping(Vector3 mNormal, Vector3 mTargetPoint, Vector3 mCurrentPoint)
        {
            Vector3 projection = mCurrentPoint - Vector3.Dot(mCurrentPoint - mTargetPoint, mNormal) * mNormal;
            return projection;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            DrawCircle();
        }
        float radius = 0.1f;
        int segments = 20;
        float arrowLength = 0.01f;
        private void DrawCircle()
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position;

            Quaternion rotation = transform.rotation;

            float angleIncrement = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * angleIncrement;
                Vector3 point = center + rotation * Quaternion.Euler(0, 0, angle) * (Vector3.right * radius);

                float nextAngle = (i + 1) * angleIncrement;
                Vector3 nextPoint = center + rotation * Quaternion.Euler(0, 0, nextAngle) * (Vector3.right * radius);
                if (i != 1)
                {
                    Gizmos.DrawLine(point, nextPoint);
                }
                if (i == 0)
                {
                    // 绘制箭头
                    Vector3 arrowTip = center + rotation * Quaternion.Euler(0, 0, angle + angleIncrement / 2) * (Vector3.right * (radius + arrowLength));
                    Gizmos.DrawLine(nextPoint, arrowTip);
                    arrowTip = center + rotation * Quaternion.Euler(0, 0, angle + angleIncrement / 2) * (Vector3.right * (radius + -arrowLength));
                    Gizmos.DrawLine(nextPoint, arrowTip);
                }
            }
        }
#endif
    }
}