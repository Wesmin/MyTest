using System.Collections.Generic;
using UnityEngine;

public class CloneFollower : MonoBehaviour
{
    public float delay = 0.2f;

    private Queue<(float time, Vector3 pos)> positionQueue = new Queue<(float, Vector3)>();

    void Update()
    {
        if (positionQueue.Count == 0) return;

        float currentTime = Time.time;

        while (positionQueue.Count > 0 && currentTime - positionQueue.Peek().time >= delay)
        {
            var data = positionQueue.Dequeue();
            transform.position = data.pos;
        }
    }

    public void RecordPosition(Vector3 position)
    {
        positionQueue.Enqueue((Time.time, position));
    }
}
