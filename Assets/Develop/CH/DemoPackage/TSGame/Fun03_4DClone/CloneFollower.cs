using System.Collections.Generic;
using UnityEngine;

public class CloneFollower : MonoBehaviour
{
    public float delay = 0.2f;
    public float dis = 1f;
    private Queue<(float time, Vector3 pos)> positionQueue = new Queue<(float, Vector3)>();

    private void Start()
    {
        Material mat = transform.GetComponent<MeshRenderer>().materials[0];
        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(-0.5f, 0.5f);
        // 创建一个 Vector4（z,w可以设为0）
        Vector4 scrollSpeed = new Vector4(x, y, 0f, 0f);

        // 设置到 Shader 中
        mat.SetVector("_ScrollSpeed", scrollSpeed);
    }
    void Update()
    {
        if (positionQueue.Count == 0) return;

        float currentTime = Time.time;

        while (positionQueue.Count > 0 && currentTime - positionQueue.Peek().time >= delay)
        {
            var data = positionQueue.Dequeue();
            transform.position = data.pos + new Vector3(0, 0, dis);
        }
    }

    public void RecordPosition(Vector3 position)
    {
        positionQueue.Enqueue((Time.time, position));
    }
}
