using UnityEngine;

/// <summary>
/// 用于动态挂载至各个光圈位置上
/// </summary>
namespace TS.Tools
{
    public class PlayerTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "PlayerCollider")
            {
                transform.Hide();
                BaseConfig.NextStep();
            }
        }
    }
}