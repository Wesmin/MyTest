using UnityEngine;
namespace TS.Tools
{
    [RequireComponent(typeof(DirectionDetection))]
    public class PointingToAObject : MonoBehaviour
    {
        //public float hideDistance = 2f; // ¾àÀëÉãÏñ»ú¶àÔ¶Ê±Òþ²Ø¼ýÍ·
        public float detectionAngle = 45;
        private DirectionDetection directionDetection;
        private Renderer[] thisRenderers;
        void Start()
        {
            directionDetection = GetComponent<DirectionDetection>();
            thisRenderers = GetComponentsInChildren<Renderer>();
        }
        private bool RenderState = false;
        void HideRenderer()
        {
            if (!RenderState)
                return;
            foreach (var item in thisRenderers)
            {
                item.enabled = false;
            }
            RenderState = false;
        }
        void ShowRenderer()
        {
            if (RenderState)
                return;
            foreach (var item in thisRenderers)
            {
                item.enabled = true;
            }
            RenderState = true;
        }
        void LateUpdate()
        {

            if (directionDetection.target == null)
            {
                RenderState = true;
                HideRenderer();
                return;
            }

            if (directionDetection.detectionAngle <= detectionAngle)
            {
                HideRenderer();
            }
            else
            {
                ShowRenderer();
            }
        }
    }
}