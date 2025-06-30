using UnityEngine;
using UnityEditor;

namespace TS.Tools 
{
    public enum LabelColor
    {
        Purple,
        Yellow,
        Blue,
        Green,
        Orange
    }

    [ExecuteInEditMode]
    public class CustomHierarchyLabel : MonoBehaviour
    {
        public string customLabel = "Custom Label";
        public LabelColor labelColor = LabelColor.Yellow;
    }

    [CustomPropertyDrawer(typeof(LabelColor))]
    public class LabelColorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 绘制下拉菜单
            property.enumValueIndex = EditorGUI.Popup(position, label.text, property.enumValueIndex, property.enumDisplayNames);

            EditorGUI.EndProperty();
        }
    }

    [InitializeOnLoad]
    public static class CustomHierarchyLabelEditor
    {
        static CustomHierarchyLabelEditor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj != null)
            {
                CustomHierarchyLabel label = obj.GetComponent<CustomHierarchyLabel>();
                if (label != null && !string.IsNullOrEmpty(label.customLabel))
                {
                    // 获取实际颜色
                    Color actualColor = GetColorFromLabelColor(label.labelColor);

                    // 获取对象名字的宽度
                    GUIContent content = new GUIContent(obj.name);
                    float width = EditorStyles.label.CalcSize(content).x;

                    // 创建新的矩形区域在对象名字后面
                    Rect rect = new Rect(selectionRect);
                    rect.x += width + 20; // 在名字后面增加20像素

                    // 保存当前GUI颜色
                    Color oldColor = GUI.color;
                    // 设置新颜色
                    GUI.color = actualColor;

                    // 绘制自定义标签
                    GUI.Label(rect, label.customLabel, EditorStyles.boldLabel);

                    // 恢复原来颜色
                    GUI.color = oldColor;

                    // 绘制复选框，固定位置
                    Rect toggleRect = new Rect(selectionRect);
                    toggleRect.x = selectionRect.xMax - 20; // 将复选框位置固定在一个统一的位置，比如窗口的右侧

                    bool isActive = GUI.Toggle(toggleRect, obj.activeSelf, GUIContent.none);
                    if (isActive != obj.activeSelf)
                    {
                        obj.SetActive(isActive);
                    }
                }
            }
        }

        private static Color GetColorFromLabelColor(LabelColor labelColor)
        {
            switch (labelColor)
            {
                case LabelColor.Purple: return Color.magenta;
                case LabelColor.Yellow: return Color.yellow;
                case LabelColor.Blue: return Color.blue;
                case LabelColor.Green: return Color.green;
                case LabelColor.Orange: return new Color(1.0f, 0.5f, 0.0f); // Unity 没有直接的橙色，可以自定义
                default: return Color.white;
            }
        }
    }
}