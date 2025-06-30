using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform startTransform;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 模型顶部中心点
        Vector3 modelCenter = transform.position;
        if(startTransform.gameObject.activeSelf)
        {
            // 设置连线的起点和终点
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startTransform.position);
            lineRenderer.SetPosition(1, modelCenter);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
