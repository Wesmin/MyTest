using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTracking : MonoBehaviour
{
    [Header("玩家")]
    public GameObject Player;
    [Header("导航箭头3D对象Quad的预制体")]
    public GameObject meshRendererPrefab;
    private GameObject currentArrow;//当前导航箭头
    [Header("箭头的缩放比例")]
    public float xscale = 0.5f;
    public float yscale = 2f;
    [Header("指引点")]
    public List<Transform> targets = new List<Transform>();
    private int CurrentIndex = 0; //当前目标索引
    private bool isActive = false;  //限定条件
    private GameObject CurrentTargets; //当前指引点
    [Header("屏幕追踪预制体")]
    public TargetTipWithoutWindow STK;
    //玩家与指引点距离
    float dis;

    private void Update()
    {
        if (isActive == true) TSDraw();
        if (Input.GetKeyDown(KeyCode.Space)) DrawPath(); //测试方法，正式使用时注释掉，调用DrawPath()方法
    }
    private void TSDraw()
    {
        //更新导航箭头的路径
        DrawPath(new Vector3[] { Player.transform.position, targets[CurrentIndex].position });
        //检查玩家是否到达当前目标
        float distanceToTarget = Vector3.Distance(Player.transform.position, targets[CurrentIndex].position);
        //假设1是到达目标的阈值
        if (distanceToTarget < 0.55f)
        {
            //到达目标后隐藏路径
            currentArrow.SetActive(false);

            //关闭当前指引点
            targets[CurrentIndex].gameObject.SetActive(false);

            //判断是否到达最终目标点
            if (CurrentIndex + 1 < targets.Count)
            {
                Debug.Log(string.Format("<color=yellow>{0}</color>", "到达指引点"));
                //开启下一个指引点
                targets[CurrentIndex + 1].gameObject.SetActive(true);
                CurrentTargets = targets[CurrentIndex + 1].gameObject;
                //切换到下一个目标
                CurrentIndex = (CurrentIndex + 1) % targets.Count;
                //更新导航箭头的路径
                DrawPath(new Vector3[] { Player.transform.position, targets[CurrentIndex].position });
            }
            else
            {
                isActive = false;
                //关闭屏幕追踪
                STK.TargetTransform = null;
                Debug.Log(string.Format("<color=cyan>{0}</color>", "到达最终目标点"));
                return;
            }
        }
        //屏幕追踪
        STK.TargetTransform = CurrentTargets.transform;
        //距离
        dis = (Player.transform.position - CurrentTargets.transform.position).magnitude;
    }

    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 60;
        fontStyle.fontStyle = FontStyle.Bold;
        fontStyle.normal.textColor = new Color(50 / 255f, 50 / 255f, 50 / 255f, 1);
        if (isActive == false)
        {
            GUI.Label(new Rect(20, 20, 60, 60), "按下空格键开始导航指引", fontStyle);
        }
        if (isActive == true)
        {
            GUI.Label(new Rect(20, 20, 60, 60), "鼠标右键旋转", fontStyle);
            GUI.Label(new Rect(20, 100, 60, 60), "键盘WASD移动", fontStyle);
            GUI.Label(new Rect(20, 180, 60, 60), string.Format("距离：{0:0.00}米", dis), fontStyle);
        }
    }
    /// <summary>
    /// 划路径外部调用方法
    /// </summary>
    public void DrawPath()
    {
        isActive = true;
        //设置索引为0
        CurrentIndex = 0;
        //激活第一个指引点
        targets[CurrentIndex].gameObject.SetActive(true);
        //设置屏幕追踪的物体为第一个指引点
        CurrentTargets = targets[CurrentIndex].gameObject;
        //创建箭头
        currentArrow = Instantiate(meshRendererPrefab, Vector3.zero, Quaternion.identity);
        //初始时隐藏导航箭头
        currentArrow.SetActive(false);
        //画路径
        DrawPath(new Vector3[] { Player.transform.position, targets[CurrentIndex].position });
        //屏幕追踪
        STK.TargetTransform = targets[CurrentIndex].transform;
        Debug.Log(string.Format("<color=cyan>{0}</color>", "开始指引"));
    }

    /// <summary>
    /// 画路径的实现
    /// </summary>
    private void DrawPath(Vector3[] points)
    {
        if (points == null || points.Length <= 1)
            return;

        var start = points[0];
        var end = points[1];

        var length = Vector3.Distance(start, end);
        currentArrow.transform.localScale = new Vector3(xscale, length, 1);
        currentArrow.transform.position = (start + end) / 2;
        //指向end
        currentArrow.transform.LookAt(end);
        //旋转偏移
        currentArrow.transform.Rotate(90, 0, 0);
        currentArrow.GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(1, length * yscale);
        currentArrow.SetActive(true);
    }
}
