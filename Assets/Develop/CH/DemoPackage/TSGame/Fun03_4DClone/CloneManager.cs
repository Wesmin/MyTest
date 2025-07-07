using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CloneManager : MonoBehaviour
{
    public Transform mainBall;

    [Header("MainBall Movement Range")]
    private Vector3 targetPosition;
    private float moveSpeed = 20f;
    private float waitTime = 0f;


    public Transform cloneGroup;
    public GameObject clonePrefab;
    public int cloneCount = 10;
    public float delayStep = 0.2f;
    public float disStep = 1f;

    private CloneFollower[] clones;
    private Vector3 lastPosition;
    private LineRenderer lineRenderer;

    void Start()
    {
        clones = new CloneFollower[cloneCount];

        Vector3 pos = mainBall.position;

        for (int i = 0; i < cloneCount; i++)
        {
            GameObject clone = Instantiate(clonePrefab, mainBall.position, Quaternion.identity, cloneGroup);
            clone.name = $"Clone_{i}";
            CloneFollower follower = clone.GetComponent<CloneFollower>();
            follower.delay = delayStep * (i + 1);
            follower.dis = disStep * (i + 1);
            clones[i] = follower;
            clones[i].transform.position = pos + new Vector3(0, 0, follower.dis);
        }

        lastPosition = mainBall.position;

        // 初始化 LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = cloneCount + 1; // 包括主球体
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        //lineRenderer.startColor = Color.white;
        //lineRenderer.endColor = Color.white;
        lineRenderer.useWorldSpace = true;

        // 初始化随机移动
        PickNewTarget();
    }

    void Update()
    {
        SimulateMainBallMovement();


        if (mainBall.position != lastPosition)
        {
            foreach (var clone in clones)
            {
                clone.RecordPosition(mainBall.position);
            }

            lastPosition = mainBall.position;
        }

        DrawSmoothLine();

        //// Step 1: 收集控制点（主球 + 所有分身）
        //List<Vector3> controlPoints = new List<Vector3>();

        //// 为 CatmullRom 提供至少4个点，前后复制边界点
        //Vector3 start = mainBall.position;
        //controlPoints.Add(start); // p0
        //controlPoints.Add(start); // p1

        //foreach (var clone in clones)
        //{
        //    controlPoints.Add(clone.transform.position);
        //}

        //Vector3 end = clones[clones.Length - 1].transform.position;
        //controlPoints.Add(end); // pN+1

        //// Step 2: 生成平滑路径点
        //List<Vector3> smoothed = SplineUtils.GenerateSmoothPath(controlPoints, resolution: 5);

        //// Step 3: 设置到 LineRenderer
        //lineRenderer.positionCount = smoothed.Count;
        //lineRenderer.SetPositions(smoothed.ToArray());
    }
    void SimulateMainBallMovement()
    {
        if (waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        // 动态平滑移动
        mainBall.localPosition = Vector3.MoveTowards(mainBall.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(mainBall.localPosition, targetPosition) < 0.1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        targetPosition = new Vector3(
            Random.Range(-5, 5),
            Random.Range(-3, 3),
            0
        );

        waitTime = Random.Range(0f, 0f); // 随机等待时间
    }

    void DrawSmoothLine()
    {
        List<Vector3> controlPoints = new List<Vector3>();

        Vector3 start = mainBall.position;
        controlPoints.Add(start); // p0
        controlPoints.Add(start); // p1

        foreach (var clone in clones)
        {
            controlPoints.Add(clone.transform.position);
        }

        Vector3 end = clones[clones.Length - 1].transform.position;
        controlPoints.Add(end); // pN+1

        List<Vector3> smoothed = SplineUtils.GenerateSmoothPath(controlPoints, resolution: 5);

        lineRenderer.positionCount = smoothed.Count;
        lineRenderer.SetPositions(smoothed.ToArray());
    }
}
public static class SplineUtils
{
    // Catmull-Rom 插值公式
    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    // 生成平滑路径点序列
    public static List<Vector3> GenerateSmoothPath(List<Vector3> controlPoints, int resolution = 10)
    {
        List<Vector3> smoothedPoints = new List<Vector3>();

        if (controlPoints.Count < 4)
        {
            // 少于4个点时无法使用 Catmull-Rom，直接返回原始点
            return new List<Vector3>(controlPoints);
        }

        for (int i = 0; i < controlPoints.Count - 3; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 point = CatmullRom(
                    controlPoints[i],
                    controlPoints[i + 1],
                    controlPoints[i + 2],
                    controlPoints[i + 3],
                    t);
                smoothedPoints.Add(point);
            }
        }

        // 最后一个关键点
        smoothedPoints.Add(controlPoints[controlPoints.Count - 2]);

        return smoothedPoints;
    }
}