using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door_Interaction)), CanEditMultipleObjects]
public class Door_Interaction_Editor : Editor
{
    private SerializedProperty open;
    private SerializedProperty locked;
    private SerializedProperty rotateAngle;
    private SerializedProperty speed;
    private SerializedProperty rotateAxis;
    private SerializedProperty startingPosition;
    private SerializedProperty endingPosition;
    private SerializedProperty isActive;
    private SerializedProperty startClip;
    private SerializedProperty endClip;
    private GUIStyle defaultStyle = new GUIStyle();

    public void OnEnable()
    {
        open = serializedObject.FindProperty("open");
        locked = serializedObject.FindProperty("locked");
        rotateAngle = serializedObject.FindProperty("rotateAngle");
        speed = serializedObject.FindProperty("speed");
        rotateAxis = serializedObject.FindProperty("rotateAxis");
        startingPosition = serializedObject.FindProperty("startingPosition");
        endingPosition = serializedObject.FindProperty("endingPosition");
        isActive = serializedObject.FindProperty("isActive");
        startClip = serializedObject.FindProperty("startClip");
        endClip = serializedObject.FindProperty("endClip");
    }
    bool IsButtonDown = false;
    Vector3 CurrentPosition= Vector3.zero;
    Quaternion CurrentRotation = Quaternion.identity;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var myScript = target as Door_Interaction;
        var doorGUIContent = new GUIContent("门类型", "选择不同门的类型参数也有所改变");
        GUIContent[] doorOptions;
        doorOptions = new[] { new GUIContent("旋转门"), new GUIContent("推拉门") };

        myScript.doorMovementType = (Door_Interaction.doorType)EditorGUILayout.Popup(doorGUIContent, (int)myScript.doorMovementType, doorOptions);
        EditorGUILayout.PropertyField(open,new GUIContent("当前状态"));
        EditorGUILayout.PropertyField(locked, new GUIContent("锁"));
        EditorGUILayout.PropertyField(speed, new GUIContent("执行速度"));

        EditorGUILayout.PropertyField(startClip, new GUIContent("打开时的声音"));
        EditorGUILayout.PropertyField(endClip, new GUIContent("关闭时的声音"));
        EditorGUILayout.PropertyField(isActive, new GUIContent("激活"));

        GUILayout.Space(20);//间隔
        if (myScript.doorMovementType == Door_Interaction.doorType.RotatingDoor) {

            EditorGUILayout.PropertyField(rotateAngle, new GUIContent("旋转角度"));
            EditorGUILayout.PropertyField(rotateAxis, new GUIContent("轴", "门的旋转轴。 默认是Y "));
            GUILayout.Space(20);//间隔
            if (GUILayout.RepeatButton(new GUIContent("查看结束位置", "在视图中查看结束位置")))
            {
                if (IsButtonDown == false)
                {
                    IsButtonDown = true;
                    myScript.InitRotation();
                    CurrentRotation = myScript.transform.localRotation;
                }
                myScript.transform.rotation = myScript.openRotation;
            }
            else
            {
                if (IsButtonDown)
                {
                    IsButtonDown = false;
                    myScript.transform.localRotation = CurrentRotation;
                }
            }
        } else if (myScript.doorMovementType == Door_Interaction.doorType.SlidingDoor) {

            defaultStyle.alignment = TextAnchor.UpperCenter; //字体对齐方式： 水平靠左，垂直居中
            defaultStyle.normal.textColor = Color.yellow; //字体颜色：黄色
            defaultStyle.fontSize = 15; //字体大小： 20

            GUILayout.Label("起始位置", defaultStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(startingPosition, new GUIContent(""));
            if (GUILayout.Button(new GUIContent("得到位置", "将当前的局部坐标赋值给开始位置")))
            {
                myScript.startingPosition = myScript.transform.localPosition;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("结束位置", defaultStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(endingPosition, new GUIContent(""));
            if (GUILayout.Button(new GUIContent("得到位置", "将当前的局部坐标赋值给结束位置")))
            {
                myScript.endingPosition = myScript.transform.localPosition;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);//间隔
            if (GUILayout.RepeatButton(new GUIContent("查看结束位置", "在视图中查看结束位置")))
            {
                if (IsButtonDown==false)
                {
                    IsButtonDown = true;
                    CurrentPosition = myScript.transform.localPosition;
                }
                myScript.transform.localPosition = myScript.endingPosition;
            }
            else
            {
                if (IsButtonDown)
                {
                    IsButtonDown = false;
                    myScript.transform.localPosition = CurrentPosition;
                }
            } 
        }
        serializedObject.ApplyModifiedProperties();//顾名思义 应用修改的属性
        if (GUI.changed == true) {
            EditorUtility.SetDirty(target);//这个函数告诉引擎，相关对象所属于的Prefab已经发生了更改。
        }
    }
}
