using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMotionBlur : MonoBehaviour
{
    public Material motionBlurMaterial; // 引用具有MotionBlur Shader的材质
    float value = 0;
    public bool isUpdate = false;
    void Start()
    {
        // 检查材质和Shader是否正确设置
        if (motionBlurMaterial != null && motionBlurMaterial.shader.name == "TS/MotionBlur")
        {
            // 设置初始的Size值
            motionBlurMaterial.SetFloat("_Size", 1.0f);
        }
        else
        {
            Debug.LogError("MotionBlur材质或Shader未正确设置。");
        }
    }
    void Update()
    {


        if (isUpdate)
        {
            //来回执行 增强 减弱 
            float newSize = Mathf.PingPong(Time.time * 15, 20.0f);
            motionBlurMaterial.SetFloat("_Size", newSize);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //判断是否超出范围
                if (value + 5 <= 20)
                {
                    value += 5;
                    motionBlurMaterial.SetFloat("_Size", value);
                    Debug.Log(string.Format("<color=yellow>{0}</color>", "模糊已增强"));
                }
                else
                {
                    Debug.LogError("超出范围，请先减弱模糊");
                }

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //判断是否超出范围
                if (value - 5 >= 0)
                {
                    value -= 5;
                    motionBlurMaterial.SetFloat("_Size", value);
                    Debug.Log(string.Format("<color=cyan>{0}</color>", "模糊已减弱"));
                }
                else
                {
                    Debug.LogError("超出范围，请先增强模糊");
                }

            }
        }
    }
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 60;
        fontStyle.fontStyle = FontStyle.Bold;
        fontStyle.normal.textColor = new Color(50 / 255f, 50 / 255f, 50 / 255f, 1);
        if (isUpdate==false)
        {
            GUI.Label(new Rect(20, 20, 60, 60), "Q键增强模糊效果 E键减弱模糊效果", fontStyle);
            GUI.Label(new Rect(20, 100, 60, 60), "勾选isUpdate执行限值内自运行", fontStyle);
        }
    }
    /// <summary>
    /// 编辑器模式
    /// </summary>
    public void OnValidate()
    {
        if (isUpdate)
        {
            Debug.Log(string.Format("<color=red>{0}</color>", "开始执行限值内自运行"));
        }
        else
        {
            Debug.Log(string.Format("<color=red>{0}</color>", "停止执行限值内自运行"));
        }
    }
}

