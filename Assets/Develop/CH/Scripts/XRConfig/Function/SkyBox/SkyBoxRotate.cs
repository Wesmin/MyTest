using UnityEngine;

namespace TS.Tools 
{
    public class SkyBoxRotate : MonoBehaviour
    {
        [Header("旋转速率")]
        public float speed;
        [Header("当前旋转")]
        public float rot;
        void Start()
        {
            rot = RenderSettings.skybox.GetFloat("_Rotation");
        }
        void Update()
        {
            if (RenderSettings.skybox == null)
                return;

            rot += speed * Time.deltaTime;
            rot %= 360;
            RenderSettings.skybox.SetFloat("_Rotation", rot);
        }
    }
}