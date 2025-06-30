using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ModelExplode : MonoBehaviour
{
    [Header("合并的物体")]
    public Transform CombineModel;
    [Header("爆炸的物体")]
    public Transform ExplodeModel;

    /// <summary>
    /// 当前爆炸合并事件
    /// </summary>
    private Action<bool> currentAction;
    /// <summary>
    /// 爆炸时间
    /// </summary>
    private float lerpTime = 0.3f; 
    /// <summary>
    /// True：爆炸 False：合并
    /// </summary>
    private bool IsExplode = false;


    private void Start()
    {
        // 所有合并子物体
        List<GameObject> CombineChild = GetChild(CombineModel);
        // 记录所有合并子物体的位置、角度
        List<Vector3> CombinePos = new List<Vector3>();
        List<Quaternion> CombineRot = new List<Quaternion>();
        for (int i = 0; i < CombineChild.Count; i++)
        {
            CombinePos.Add(CombineChild[i].transform.localPosition);
            CombineRot.Add(CombineChild[i].transform.localRotation);
        }

        // 所有爆炸子物体
        List<GameObject> ExplodeChild = GetChild(ExplodeModel);
        // 记录所有爆炸子物体的位置、角度
        List<Vector3> ExplodePos = new List<Vector3>();
        List<Quaternion> ExplodeRot = new List<Quaternion>(); 
        for (int i = 0; i < ExplodeChild.Count; i++)
        {
            ExplodePos.Add(ExplodeChild[i].transform.localPosition);
            ExplodeRot.Add(ExplodeChild[i].transform.localRotation);
        }

        // 当前爆炸合并事件
        currentAction = delegate (bool active)
        {
            for (int i = 0; i < CombineChild.Count; i++)
            {
                Vector3 targetPos = active ? ExplodePos[i] : CombinePos[i];
                Quaternion targetRot = active ? ExplodeRot[i] : CombineRot[i];
                CombineChild[i].transform.localPosition = Vector3.Lerp(CombineChild[i].transform.localPosition, targetPos, Time.deltaTime / lerpTime);
                CombineChild[i].transform.localRotation = Quaternion.Slerp(CombineChild[i].transform.localRotation, targetRot, Time.deltaTime / lerpTime);
            }
        };
    }
    /// <summary>
    /// 获取所有子对象
    /// </summary>
    private List<GameObject> GetChild(Transform obj)
    {
        List<GameObject> tempArrayobj = new List<GameObject>();
        foreach (Transform child in obj)
            tempArrayobj.Add(child.gameObject);
        return tempArrayobj;
    }
    private void Update()
    {
        currentAction(IsExplode);

        if (Input.GetKeyDown(KeyCode.Q)) Play(true);

        if (Input.GetKeyDown(KeyCode.E)) Play(false);
    }
    /// <summary>
    /// 爆炸合并播放
    /// </summary>
    public void Play(bool active, float time = 0.3f)
    {
        lerpTime = time;
        IsExplode = active;
    }


    /// <summary>
    /// GUI提示
    /// </summary>
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 60;
        fontStyle.fontStyle = FontStyle.Bold;
        fontStyle.normal.textColor = Color.cyan;
        GUI.Label(new Rect(20, 20, 60, 60), "按键“Q”爆炸  按键“E”合并", fontStyle);
        GUI.Label(new Rect(20, 100, 60, 60), "鼠标右键旋转", fontStyle);
    }
}
