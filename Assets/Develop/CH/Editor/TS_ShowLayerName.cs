using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TS_ShowLayerName
{
    static readonly int IgnoreLayer = LayerMask.NameToLayer("Default");

    static readonly GUIStyle _style = new GUIStyle()
    {
        fontSize = 15,
        alignment = TextAnchor.MiddleRight
    };

    static TS_ShowLayerName()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null)
        {
            EditorGUI.LabelField(selectionRect, LayerMask.LayerToName(gameObject.layer), _style);
        }
    }
}
