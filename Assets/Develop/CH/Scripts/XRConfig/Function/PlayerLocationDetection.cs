using UnityEngine;

public class PlayerLocationDetection : MonoBehaviour
{
    private Transform PlayerCamera;

    private void Start()
    {
        PlayerCamera = BaseManager.Instance.VRMainCamera;
    }

    private void LateUpdate()
    {
        if (PlayerCamera == null) return;
        transform.position = new Vector3(PlayerCamera.position.x, transform.position.y, PlayerCamera.position.z);
    }
}
