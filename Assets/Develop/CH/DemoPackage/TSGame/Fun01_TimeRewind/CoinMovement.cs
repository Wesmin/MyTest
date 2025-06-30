using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    public float rotationSpeed = 100f;  // 旋转速度
    public float moveSpeed = 0.5f;      // 上下移动的速度
    public float moveHeight = 0.5f;     // 上下移动的幅度

    private Vector3 startPosition;

    void Start()
    {
        // 记录初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 旋转物体
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);  // 每秒旋转rotationSpeed度

        // 上下移动
        float newY = Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
}
