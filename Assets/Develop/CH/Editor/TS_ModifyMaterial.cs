using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class TS_ModifyMaterial : EditorWindow
{  
    private static TS_ModifyMaterial _window; //主要窗口对象
    public Material ModifyMaterial; //材质球
    private GUIStyle defaultStyle = new GUIStyle();//定义 GUIStyle  
    [MenuItem("Tools/【TS】替换材质")] //定义窗口菜单
    static void WindowGUI()
    {      
        Rect window = new Rect(0, 0, 300, 200); //窗口大小
        _window = (TS_ModifyMaterial)GetWindowWithRect(typeof(TS_ModifyMaterial), window, true, "【TSTools】");
        _window.Show();
    }

    /// <summary>
    /// 窗口样式
    /// </summary>
    void OnGUI()
    {
        //文本
        defaultStyle.alignment = TextAnchor.MiddleCenter; //字体对齐方式
        defaultStyle.normal.textColor = Color.yellow; //字体颜色
        defaultStyle.fontSize = 15;//字体大小
        defaultStyle.fontStyle = FontStyle.Bold;//字体格式
        EditorGUILayout.LabelField("【一键替换材质】", defaultStyle); //绘制GUI组件，使用 defaultStyle
        //文本
        defaultStyle.alignment = TextAnchor.MiddleLeft; 
        defaultStyle.normal.textColor = Color.cyan; 
        defaultStyle.fontSize = 12;
        defaultStyle.fontStyle = FontStyle.Normal;
        EditorGUILayout.LabelField("修改当前选中物体下的全部材质球", defaultStyle);
        //材质球
        ModifyMaterial = (Material)EditorGUILayout.ObjectField("用于替换的材质球", ModifyMaterial, typeof(Material), true);
        //按钮
        GUI.color = Color.yellow;
        GUILayout.BeginHorizontal();      
        if (GUILayout.Button(new GUIContent("替换", EditorGUIUtility.FindTexture("PlayButton"))))
        {
            OBJ_ModifyMaterial();
        }
        GUILayout.EndHorizontal();
    }


    /// <summary>
    /// GameObject材质球替换
    /// </summary>
    void OBJ_ModifyMaterial()
    {
        Transform trs = Selection.activeTransform;

        foreach (MeshRenderer it in trs.GetComponentsInChildren<MeshRenderer>(true))
        {
            Material[] newMaterials = new Material[it.materials.Length];

            for (int i = 0; i < it.materials.Length; i++)
            {
                // 直接赋值目标材质，而不是创建新的材质实例
                newMaterials[i] = ModifyMaterial;
            }

            // 将新的材质数组赋值给 MeshRenderer
            it.materials = newMaterials;

            Debug.Log($"{it.name} 的材质已替换成 {ModifyMaterial.name}");
        }
    }
}