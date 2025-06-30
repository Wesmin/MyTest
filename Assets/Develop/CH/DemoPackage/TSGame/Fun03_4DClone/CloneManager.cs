using UnityEngine;

public class CloneManager : MonoBehaviour
{
    // 拖拽的小球
    public Transform mainBall;                   
    public Transform cloneGroup;
    // 分身预制体（必须带 CloneFollower 脚本）
    public GameObject clonePrefab;              
    public int cloneCount = 10;
    public float delayStep = 0.2f;

    private CloneFollower[] clones;
    private Vector3 lastPosition;

    void Start()
    {
        // 自动生成分身
        clones = new CloneFollower[cloneCount];

        for (int i = 0; i < cloneCount; i++)
        {
            GameObject clone = Instantiate(clonePrefab, mainBall.position, Quaternion.identity, cloneGroup);
            clone.name = $"Clone_{i}";
            CloneFollower follower = clone.GetComponent<CloneFollower>();
            follower.delay = delayStep * (i + 1);   // 0.2f, 0.4f, ..., 2.0f
            clones[i] = follower;
        }

        lastPosition = mainBall.position;
    }

    void Update()
    {
        if (mainBall.position != lastPosition)
        {
            foreach (var clone in clones)
            {
                clone.RecordPosition(mainBall.position);
            }

            lastPosition = mainBall.position;
        }
    }
}
