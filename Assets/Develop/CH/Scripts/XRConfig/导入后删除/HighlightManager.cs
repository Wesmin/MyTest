using System;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    private HighlightingRenderer highlightingRenderer;
    public Camera highlightCamera;
    private Highlighter highlighter;
    private Gradient highlighterColor;
    private float durationSpeed = 1;

    public static HighlightManager Instance;    
    private void Awake()
    {
        Instance = this;
        SetHighlighterColor();
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public Gradient GetHighlighterColor()
    {
        return highlighterColor;
    }

    public float GetDurationSpeed()
    {
        return durationSpeed;
    }

    /// <summary>
    /// 设置颜色  具体可写辅助类重载
    /// </summary>
    public void SetHighlighterColor()
    {
        highlighterColor = new Gradient();
        // 创建渐变的颜色和时间值
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = Color.cyan;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.red;
        colorKeys[1].time = 1f;

        // 创建渐变的透明度和时间值
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 1f;

        // 将颜色和透明度设置到渐变对象中
        highlighterColor.SetKeys(colorKeys, alphaKeys);
    }

    /// <summary>
    /// 设置闪烁时间
    /// </summary>
    /// <param name="speed"></param>
    public void SetDuration(float speed)
    {
        durationSpeed = speed;
    }

    /// <summary>
    /// 判断摄像机上有没有 HighlightingRenderer 组件
    /// </summary>
    /// <returns></returns>
    public bool IsHighlightingRenderer()
    {
        highlightingRenderer = highlightCamera.GetComponent<HighlightingRenderer>();
        if (highlightingRenderer)
        {
            return true;
        }

        Debug.LogError($"摄像机缺少 Highlighting Renderer 组件");
        return false;
    }

    /// <summary>
    /// 刷新高光摄像机问题
    /// </summary>
    /// <param name="camera"></param>
    public void RefreshHighlightCamera(Camera camera)
    {
        highlightCamera = camera;
        highlightCamera.gameObject.AddComponent<HighlightingRenderer>();
    }
}
public static class HighlightHelper
{
    /// <summary>
    /// 高光展示
    /// </summary>
    /// <param name="go"></param>
    public static void HighlightShow(this GameObject go)
    {
        Debug.Log("开启高亮");
        if (!HighlightManager.Instance.IsHighlightingRenderer())
        {
            return;
        }

        Highlighter highlighter = go.GetComponent<Highlighter>();
        if (highlighter == null)
        {
            highlighter = go.AddComponent<Highlighter>();
        }
        highlighter.tweenGradient = HighlightManager.Instance.GetHighlighterColor();
        highlighter.tweenDuration = HighlightManager.Instance.GetDurationSpeed();
        highlighter.tween = true;
        highlighter.overlay = true;
    }

    /// <summary>
    /// 高光关闭
    /// </summary>
    /// <param name="go"></param>
    public static void HighlightHide(this GameObject go)
    {
        if (!HighlightManager.Instance.IsHighlightingRenderer())
        {
            return;
        }

        Highlighter highlighter = go.GetComponent<Highlighter>();
        if (highlighter == null)
        {
            Debug.LogWarning($"对象：{go.name}的高光组件为空！");
            return;
        }

        highlighter.tween = false;
    }
}