using UnityEngine;
namespace TS.Tools
{
    public class TriggerTag : MonoBehaviour
    {
        private void Awake()
        {
            if (HandExtend.IsPC) transform.ChangeLayer(BaseConfig.TouchLayer);
        }
    }
}